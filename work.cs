using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using Tool_Class;
using System.IO;
using System.Data.OleDb;

namespace Work
{
    public class Work
    {
        #region test
         private string base_dir = "D:\\ini_diff\\";
        private string ini_file = "ini.txt";
        // private string log_file = "diff_log.txt";
        private string result_file = "result_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
        private string err_file = "err_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
        IO_tool io = new IO_tool();
        private ReadOracleData ROD = new ReadOracleData();
        private string[] output_stat = { "Lis Message", "", "样本总量", "", "LOG\t", "", "Image\t", "", "无申请的样本" };
        private string[] SQL_stat = { "select trunc(sysdate) - trunc(min(createdate)) from lis_message",
                                    "select INHALT from datos_ini where item = 'DAYS_AFTER_DELETING_LIS_MESSAGES'",
                                     "select trunc(sysdate) - trunc(min(scan_datum)) from samples",
                                     "select INHALT from datos_ini where item='DAYS_AFTER_DELETING_OPEN_REQUESTS'",
                                     "select trunc(sysdate) - trunc(min(datum)) from SY_ERROR_LOGS",
                                     "select INHALT from datos_ini where item = 'LOG_TAGE'",
                                     "select trunc(sysdate) - trunc(min(create_date)) from SAMPLE_IMAGES",
                                     "select INHALT from datos_ini where item = 'DAYS_AFTER_DELETING_IMAGES'",
                                     "select (trunc(sysdate) - trunc(min(scan_datum))) * 24 + to_char(sysdate, 'hh24') - to_char(min(scan_datum), 'hh24') from samples where UNSOLICITED = 1",
                                     "select INHALT  from datos_ini where item = 'NUM_HOUR_KEEP_UNSOLICITED_SAMPLES'"
                                     };
        private string[] SQL_stat2 = {"SELECT round(bytes / (1024 * 1024), 0) MB FROM dba_data_files where tablespace_name = 'TS_DATEN' ",
                                     };
        public string Check_database()
        {
            int ini_diff = 0, table_diff = 0;
            string result = "False";
            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;
            #region 检查数据表是否和设置的参数一致
            for (int i = 0; i < 9; i += 2)
            {
                Table_DataSet = ROD.ReadDataToDataSet(conn, SQL_stat[i], "");
                if ((i == 6 || i == 8) && Table_DataSet.Tables[0].Rows[0].ItemArray[0].ToString() == "")
                    table_diff = -1;
                else
                {
                    if (Table_DataSet != null && !Table_DataSet.HasErrors && Table_DataSet.Tables.Count == 1)
                        table_diff = Convert.ToInt32(Table_DataSet.Tables[0].Rows[0].ItemArray[0]);
                }
                Table_DataSet.Reset();
                Table_DataSet = ROD.ReadDataToDataSet(conn, SQL_stat[i + 1], "");
                if (Table_DataSet != null && !Table_DataSet.HasErrors && Table_DataSet.Tables.Count == 1)
                    ini_diff = Convert.ToInt32(Table_DataSet.Tables[0].Rows[0].ItemArray[0]);
                if (table_diff < ini_diff)
                    result = "True";
                // public_output(output_stat[i] + "\t     " + ini_diff + "    \t\t\t\t\t" + table_diff + "    \t\t\t\t\t\t" + result);
                io.Write2file(base_dir + result_file, output_stat[i] + "\t     " + ini_diff + "    \t\t\t\t\t" + table_diff + "    \t\t\t\t\t\t" + result);
                Table_DataSet.Reset();
                result = "Flase";
                table_diff = ini_diff = 0;
            }
            //   Table_DataSet.Dispose();
            conn.Close();
            return base_dir + result_file;

        }
            #endregion
        public void ReadData2File()
        {
            if (!Directory.Exists(base_dir))
                Directory.CreateDirectory(base_dir);
            // output.public_output("第一次运行程序将收集参数信息，但不会对比最近一次的更改。");
            // io.Write2file("D:\\ini_diff\\result.txt", "第一次运行程序将收集参数信息，但不会对比最近一次的更改。");

            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;
            FileStream fs = new FileStream(base_dir + ini_file, FileMode.Create);
            fs.Close();
            Table_DataSet = ROD.ReadDataToDataSet(conn, "select item,inhalt,objectid from datos_ini", "");
            string item, inhalt, objectid;
            foreach (DataRow ini_Row in Table_DataSet.Tables[0].Rows)
            {
                item = ini_Row["item"].ToString();
                inhalt = ini_Row["inhalt"].ToString();
                objectid = ini_Row["objectid"].ToString();

                //objectid = ini_Rini_Row["inhalt"].ToString();ow["objectid"].ToString();
                // Write2file("D:\\ini_diff\\ini.txt", item + "=" + inhalt + "#" + objectid);

                io.Write2file(base_dir + ini_file, item + "=" + inhalt + objectid.ToString());
            }
            Table_DataSet.Dispose();
            //output.public_output("参数配置已经写到文件ini.txt");
            io.Write2log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":    " + "参数配置已经写到文件ini.txt。");
        }

        public string CheckIniTable()
        {
            int counter = 0, flag = 0;//1匹配到，0未匹配到;
            string item, inhalt, gruppe;
            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet = ROD.ReadDataToDataSet(conn, "select item,inhalt,gruppe from datos_ini", "");
            FileStream fs = new FileStream(base_dir + ini_file, FileMode.Open);
            FileStream fs_err = new FileStream(base_dir + err_file, FileMode.Create);
            fs_err.Close();
            string content = io.Readfile(fs);
            var lines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (lines.Count() > 1)
            {

                foreach (DataRow ini_Row in Table_DataSet.Tables[0].Rows)
                {
                    item = ini_Row["item"].ToString();
                    inhalt = ini_Row["inhalt"].ToString();
                    gruppe = ini_Row["gruppe"].ToString();
                    foreach (var line in lines)
                    {
                        var nameValue = line.Split('=');
                        counter++;
                        if (nameValue[0] != "")
                        {
                            if (item == nameValue[0])
                            {
                                flag = 1;
                                if (nameValue[0] == "V_F_STAT_LIS_WHERE_CLAUSE" || nameValue[0] == "V_F_STAT_BLD_WHERE_CLAUSE")
                                    nameValue[1] = nameValue[1] + "=" + nameValue[2];
                                //   if (nameValue[0] == "CLEANUP")
                                //       nameValue[1] = nameValue
                                if (inhalt + gruppe == nameValue[1])
                                    break;
                                else
                                {
                                    io.Write2file(base_dir + err_file, "item:" + item + " in database is \r\n" + inhalt + "\r\nbut in ini.txt file is\r\n" + nameValue[1]);
                                    break;
                                }
                            }
                        }
                    }
                    if (counter >= lines.Count() && flag == 0)
                        io.Write2file(base_dir + err_file, "item:" + item + " in database is \r\n" + inhalt + "\r\nbut in ini.txt file is NOT EXIST");
                    counter = 0;
                    flag = 0;
                }
                if (Table_DataSet.Tables[0].Rows.Count < lines.Count())//数据库小于文件的参数个数说明数据库中参数有删除过
                {
                    foreach (var line in lines)
                    {
                        var nameValue = line.Split('=');
                        foreach (DataRow ini_Row in Table_DataSet.Tables[0].Rows)
                        {
                            item = ini_Row["item"].ToString();
                            inhalt = ini_Row["inhalt"].ToString();
                            if (nameValue[0] != "")
                            {
                                if (item == nameValue[0])
                                    flag = 1;
                            }
                        }
                        if (flag == 0 && nameValue[0] != "")
                            io.Write2file(base_dir + err_file, "item:" + nameValue[0] + " in ini file  is \r\n" + nameValue[1] + "\r\nbut in Database file is NOT EXIST");
                        flag = 0;
                    }
                }
            }
            Table_DataSet.Dispose();
            conn.Close();
            fs.Close();
            return base_dir + err_file;

        }
    }
#endregion
    
}
