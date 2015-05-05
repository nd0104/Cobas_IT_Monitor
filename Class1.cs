using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Tool_Class
{
   /* #region DB
    //如何不使用Oracle请注释掉
    class ReadOracleData
    {
        /// <summary>
        /// Oracle 的数据库连接字符串.
        /// </summary>
        IO_tool io = new IO_tool();
        private String connString =
            @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Program.server_ip + ")(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=LIS)));User Id=datos_prj;Password=prj_bmg";
        public OracleConnection NewConn()
        {
            try
            {
                OracleConnection conn = new OracleConnection(connString);
                return conn;
            }
            catch (Exception err)
            {
                io.Write2log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "       " + err.Message.ToString());
                return null;
            }

        }
        public DataSet ReadDataToDataSet(OracleConnection conn, String SQL, string diff)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(SQL, conn);
            DataSet testDataSet = new DataSet();
            adapter.Fill(testDataSet);
            return testDataSet;
        }
    }
    #endregion*/
   
    public class CryptoHelpException : ApplicationException
    {
        public CryptoHelpException(string msg) : base(msg) { }
    }
    public class DESFileClass
    {
        private const ulong FC_TAG = 0xFC010203040506CF;
        private const int BUFFER_SIZE = 128 * 1024;
        /// <summary>
        /// 检验两个Byte数组是否相同
        /// </summary>
        /// <param name="b1">Byte数组</param>
        /// <param name="b2">Byte数组</param>
        /// <returns>true－相等</returns>
        private static bool CheckByteArrays(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; ++i)
                {
                    if (b1[i] != b2[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建DebugLZQ ,http://www.cnblogs.com/DebugLZQ
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt"></param>
        /// <returns>加密对象</returns>
        private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);

            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }

        /// <summary>
        /// 加密文件随机数生成
        /// </summary>
        private static RandomNumberGenerator rand = new RNGCryptoServiceProvider();

        /// <summary>
        /// 生成指定长度的随机Byte数组
        /// </summary>
        /// <param name="count">Byte数组长度</param>
        /// <returns>随机Byte数组</returns>
        private static byte[] GenerateRandomBytes(int count)
        {
            byte[] bytes = new byte[count];
            rand.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inFile">待加密文件</param>
        /// <param name="outFile">加密后输入文件</param>
        /// <param name="password">加密密码</param>
        public static void EncryptFile(string inFile, string outFile, string password)
        {
            using (FileStream fin = File.OpenRead(inFile),
                fout = File.OpenWrite(outFile))
            {
                long lSize = fin.Length; // 输入文件长度
                int size = (int)lSize;
                byte[] bytes = new byte[BUFFER_SIZE]; // 缓存
                int read = -1; // 输入文件读取数量
                int value = 0;

                // 获取IV和salt
                byte[] IV = GenerateRandomBytes(16);
                byte[] salt = GenerateRandomBytes(16);

                // 创建加密对象
                SymmetricAlgorithm sma = DESFileClass.CreateRijndael(password, salt);
                sma.IV = IV;

                // 在输出文件开始部分写入IV和salt
                fout.Write(IV, 0, IV.Length);
                fout.Write(salt, 0, salt.Length);

                // 创建散列加密
                HashAlgorithm hasher = SHA256.Create();
                using (CryptoStream cout = new CryptoStream(fout, sma.CreateEncryptor(), CryptoStreamMode.Write),
                    chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    BinaryWriter bw = new BinaryWriter(cout);
                    bw.Write(lSize);

                    bw.Write(FC_TAG);

                    // 读写字节块到加密流缓冲区
                    while ((read = fin.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                    }
                    // 关闭加密流
                    chash.Flush();
                    chash.Close();

                    // 读取散列
                    byte[] hash = hasher.Hash;

                    // 输入文件写入散列
                    cout.Write(hash, 0, hash.Length);

                    // 关闭文件流
                    cout.Flush();
                    cout.Close();
                }
            }
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inFile">待解密文件</param>
        /// <param name="outFile">解密后输出文件</param>
        /// <param name="password">解密密码</param>
        public static void DecryptFile(string inFile, string outFile, string password)
        {
            // 创建打开文件流
            using (FileStream fin = File.OpenRead(inFile),
                fout = File.OpenWrite(outFile))
            {
                int size = (int)fin.Length;
                byte[] bytes = new byte[BUFFER_SIZE];
                int read = -1;
                int value = 0;
                int outValue = 0;

                byte[] IV = new byte[16];
                fin.Read(IV, 0, 16);
                byte[] salt = new byte[16];
                fin.Read(salt, 0, 16);

                SymmetricAlgorithm sma = DESFileClass.CreateRijndael(password, salt);
                sma.IV = IV;

                value = 32;
                long lSize = -1;

                // 创建散列对象, 校验文件
                HashAlgorithm hasher = SHA256.Create();

                using (CryptoStream cin = new CryptoStream(fin, sma.CreateDecryptor(), CryptoStreamMode.Read),
                    chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    // 读取文件长度
                    BinaryReader br = new BinaryReader(cin);
                    lSize = br.ReadInt64();
                    ulong tag = br.ReadUInt64();

                    if (FC_TAG != tag)
                        throw new CryptoHelpException("文件被破坏");

                    long numReads = lSize / BUFFER_SIZE;

                    long slack = (long)lSize % BUFFER_SIZE;

                    for (int i = 0; i < numReads; ++i)
                    {
                        read = cin.Read(bytes, 0, bytes.Length);
                        fout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }

                    if (slack > 0)
                    {
                        read = cin.Read(bytes, 0, (int)slack);
                        fout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }

                    chash.Flush();
                    chash.Close();

                    fout.Flush();
                    fout.Close();

                    byte[] curHash = hasher.Hash;

                    // 获取比较和旧的散列对象
                    byte[] oldHash = new byte[hasher.HashSize / 8];
                    read = cin.Read(oldHash, 0, oldHash.Length);
                    if ((oldHash.Length != read) || (!CheckByteArrays(oldHash, curHash)))
                        throw new CryptoHelpException("文件被破坏");
                }

                if (outValue != lSize)
                    throw new CryptoHelpException("文件大小不匹配");
            }
        }
    }
    //数据库操作
    public class AccessDbClass1
   {


    public OleDbConnection Conn;
    public string ConnString;



    /// <summary>
    /// 打开 数据库 
    /// </summary>
    /// <param name="Dbpath">数据库路径</param>
    public void AccessDbClass2(string Dbpath)
    {
        ConnString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=";
        ConnString += Dbpath;
        Conn = new OleDbConnection(ConnString);
        Conn.Open();
        
    }


    public OleDbConnection DbConn()
    {
        Conn.Open();
        return Conn;
    }


    public void Close()
    {
        Conn.Close();
    }
    /// <summary>
    /// sql查询
    /// </summary>
    /// <param name="SQL">sql语句</param>
    public DataTable SelectToDataTable(string SQL)
    {
        OleDbDataAdapter adapter = new OleDbDataAdapter();
        OleDbCommand command = new OleDbCommand(SQL, Conn);
        adapter.SelectCommand = command;
        DataTable Dt = new DataTable();
        adapter.Fill(Dt);
        return Dt;
    }
    /// <summary>
    /// sql语句执行
    /// </summary>
    /// <param name="SQL">sql语句</param>
    public bool ExecuteSQLNonquery(string SQL)
    {
        OleDbCommand cmd = new OleDbCommand(SQL, Conn);
        try
        {
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }
    

    

}
    //配置文件操作
    /// <summary>
    /// 配置文件名为config.txt,路径在根目录Debug文件夹下
    ///事例：
    ///ip
    ///P612=127.0.0.1;
    ///C8K=192.168.1.1;
    ///ip
    ///mt
    ///disk=60;
    ///cpu=50;
    ///nem=100;
    ///mt
    /// </summary>
    public class config
    {
        /// <summary>
        /// 周伟承ip配置专用
        /// </summary>
        public string[] readparameter(string node)
        {
            Tool_Class.DESFileClass.DecryptFile("config.txt", "345.txt", "123");
            FileStream fs = new FileStream(@"345.txt", FileMode.Open);
            Tool_Class.IO_tool dd = new Tool_Class.IO_tool();
            string w = dd.Readfile(fs);
            string[] s = Regex.Split(w, node, RegexOptions.IgnoreCase);
            string ee = s[1];
            string[] ss = Regex.Split(ee, ";", RegexOptions.IgnoreCase);
            string sourceDir = @"..\Debug";
            string[] txtList = Directory.GetFiles(sourceDir, "345.txt");
            foreach (string f in txtList)
            {
                File.Delete(f);
            }
            return ss;
        }
        public class eeeee
        {
            public string eee { get; set; }
        }
        eeeee bbb = new eeeee();
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="jiedian">大节点</param>
        /// <param name="set">小节点</param>
        public string readconfig(string node, string set)
        {
            Tool_Class.DESFileClass.DecryptFile("config.txt", "345.txt", "123");
            FileStream fs = new FileStream(@"345.txt", FileMode.Open);
            Tool_Class.IO_tool dd = new Tool_Class.IO_tool();
            string w = dd.Readfile(fs);
            string[] s = Regex.Split(w, node, RegexOptions.IgnoreCase);
            string ee = s[1];
            string[] ss = Regex.Split(ee, ";", RegexOptions.IgnoreCase);
            List<string> ipList = new List<string>();
            foreach (string aa in ss)
            {
                ipList.Add(aa);
            }
            string ttttt = "\n" + set;
            foreach (string ttt in ipList)
            {
                string[] ssss = Regex.Split(ttt, "=", RegexOptions.IgnoreCase);
                string tttt = ttttt;

                if (ssss[0].Substring(1).ToString() == tttt)
                {

                    bbb.eee = ssss[1];
                }

                //return ssss[0].Substring(1).ToString();

            }
            string sourceDir = @"..\Debug";
            string[] txtList = Directory.GetFiles(sourceDir, "345.txt");
            foreach (string f in txtList)
            {
                File.Delete(f);
            }
            return bbb.eee;
        }
        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="jiedian">大节点</param>
        /// <param name="set">小节点</param>
        /// <param name="value">修改成value</param>
        public void writeconfig(string node, string set, string value)
        {
            Tool_Class.DESFileClass.DecryptFile("config.txt", "345.txt", "123");
            FileStream fs = new FileStream(@"345.txt", FileMode.Open);
            Tool_Class.IO_tool dd = new Tool_Class.IO_tool();
            string w = dd.Readfile(fs);
            string[] s = Regex.Split(w, node, RegexOptions.IgnoreCase);
            string ee = s[1];
            string[] ss = Regex.Split(ee, ";", RegexOptions.IgnoreCase);

            List<string> ipList = new List<string>();
            foreach (string aa in ss)
            {
                ipList.Add(aa);
            }
            string ttttt = "\n" + set;
            foreach (string ttt in ipList)
            {
                string[] ssss = Regex.Split(ttt, "=", RegexOptions.IgnoreCase);
                string tttt = ttttt;

                if (ssss[0].Substring(1).ToString() == tttt)
                {
                    bbb.eee = ssss[1];
                }

            }
            string values = bbb.eee;
            string[] wt = Regex.Split(w, values, RegexOptions.IgnoreCase);
            string total = wt[0] + value + wt[1];
            string sourceDir = @"..\Debug";
            string[] txtList = Directory.GetFiles(sourceDir, "345.txt");
            foreach (string f in txtList)
            {
                File.Delete(f);
            }
            dd.Write2file(@"345.txt", total);
            string[] configList = Directory.GetFiles(sourceDir, "config.txt");
            foreach (string f in configList)
            {
                File.Delete(f);
            }
            Tool_Class.DESFileClass.EncryptFile("345.txt", "config.txt", "123");
            string[] txtList2 = Directory.GetFiles(sourceDir, "345.txt");
            foreach (string f in txtList2)
            {
                File.Delete(f);
            }
        }
        /// <summary>
        /// 加密配置文件
        /// </summary>
        public void Encryptconfig()
        {
            Tool_Class.DESFileClass.EncryptFile("345.txt", "config.txt", "123");
            string sourceDir = @"..\Debug";
            string[] txtList2 = Directory.GetFiles(sourceDir, "345.txt");

            foreach (string f in txtList2)
            {
                File.Delete(f);
            }
        }
        /// <summary>
        /// 解密配置文件
        /// </summary>
        public void Decryptconfig()
        {
            Tool_Class.DESFileClass.DecryptFile("config.txt", "345.txt", "123");
            string sourceDir = @"..\Debug";
            string[] txtList2 = Directory.GetFiles(sourceDir, "config.txt");
            foreach (string f in txtList2)
            {
                File.Delete(f);
            }

        }
    }

    #region 文件操作/写日志/加密解密入口函数
    class IO_tool
    {
        public void Write2file(string fname, string details)
        {
            FileStream fs = new FileStream(fname, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.WriteLine(details);
            sw.Flush();
            sw.Close();
            fs.Close();
            fs.Dispose();
        }
        public string Readfile(FileStream fs)
        {
            try
            {
                StreamReader reader = new StreamReader(fs, Encoding.Default);
                string content = reader.ReadToEnd();
                reader.Close();
                return content;
            }
            catch (Exception err)
            {
                Write2log(err.Message.ToString());
                return null;
            }
        }
        public void Write2log(string details)
        {
            string real_details = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + details;
            Write2file("D:\\ini_diff\\diff_log.txt", real_details);
        }
        public string EncryptFile(string full_file_name)
        {
            string inFile = full_file_name;
            if (inFile.Length > 1)
            {
                string outFile = inFile + ".dat";
                string password = "Rochesrv123";
                DESFileClass.EncryptFile(inFile, outFile, password);//加密文件
                File.Delete(inFile);
                return outFile;
            }
            else
                return null;
        }
        public void DecryptFile(string full_file_name)
        {
            string inFile = full_file_name;
            if (inFile.Length > 1)
            {
                string outFile = inFile.Substring(0, inFile.Length - 4);
                string password = "Rochesrv123";
                DESFileClass.DecryptFile(inFile, outFile, password);//解密文件
            }
            //     File.Delete(inFile);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        public void AccessDbclass(string sql,string file_dir)
        {
            AccessDbClass1 db = new AccessDbClass1();
            db.AccessDbClass2(file_dir);
            bool dd = db.ExecuteSQLNonquery(sql);
            db.Close();

        }
        /// <summary>
        /// 将查询结果返回给datatable
        /// </summary>
        public DataTable DbToDatatable(string sql,string file_dir)
        {
            AccessDbClass1 db = new AccessDbClass1();
            db.AccessDbClass2(file_dir);
            DataTable table = db.SelectToDataTable(sql);
            db.Close();
            return table;
 
        }
        /// <summary>
        /// 配置文件名为config.txt,路径在根目录Debug文件夹下
        ///事例：
        ///ip
        ///P612=127.0.0.1;
        ///C8K=192.168.1.1;
        ///ip
        ///mt
        ///disk=60;
        ///cpu=50;
        ///nem=100;
        ///mt
        /// </summary>
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="jiedian">大节点</param>
        /// <param name="set">小节点</param>
        public string readconfig(string node, string set)
        {
            config config1 = new config();
            string value = config1.readconfig(node, set);
            return value;

        }
        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="jiedian">大节点</param>
        /// <param name="set">小节点</param>
        /// <param name="value">修改成value</param>
        public void writeconfig(string node, string set, string value)
        {
            config config1 = new config();
            config1.writeconfig(node,set,value);
            

        }
        /// <summary>
        /// 加密配置文件
        /// </summary>
        public void Encryptconfig()
        {
            config config1 = new config();
            config1.Encryptconfig();
 
        }
        /// <summary>
        /// 解密配置文件
        /// </summary>
        public void Decryptconfig()
        {
            config config1 = new config();
            config1.Decryptconfig();
        }

    }
}
    #endregion

   

   
