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
        string db_dir = @"E:\code\CobasITMonitor\CobasITMonitor\db.accdb";
        IO_tool io = new IO_tool();
        private ReadOracleData ROD = new ReadOracleData();
        private string insert_sql = "";
        private bool in_or_up;
        private char show_flag = 'E';
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
        private string SQL_stat2 = "SELECT round(bytes / (1024 * 1024 * 1024), 0) GB FROM dba_data_files where tablespace_name = 'TS_DATEN'";
        private string SQL_stat3 = "select log_date,status,error# from dba_scheduler_job_run_details where job_name  = 'EXPORTDB' order by log_id desc";
        private string SQL_stat4 = "select count(1)  FROM SY_ERROR_LOGS where fehlerkategorie = 30 union   select count(1)  FROM SY_ERROR_LOGS where fehlerkategorie = 20";
        private string[] SQL_stat5 = { "select count(1) from LIS_RESULTS", "select count(1) from RESULTATE", "select count(1) from LIS_MESSAGE", "select count(1) from HIST_SAMPLES" 
                                         ,"select count(1) from SAMPLE_TEST_ASSIGNMENTS","select count(1) from SAMPLE_IMAGES","select count(1) from TEST_REQUESTS" };
        private string[] output_stat2 = { "LIS_RESULTS", "RESULTATE", "LIS_MESSAGE", "HIST_SAMPLES", "SAMPLE_TEST_ASSIGNMENTS",  "SAMPLE_IMAGES",  "TEST_REQUESTS" };
        private int[] SQL5_refrence = { 30000000, 30000000, 1000000, 30000000, 30000000, 100000, 30000000, 50000000, 50000000, 2000000, 50000000, 50000000, 2000000, 50000000 };
        #region 检查数据表是否和设置的参数一致
        public void Check_database_para()
        {
            
            int ini_diff = 0, table_diff = 0;
            string result = "False", output = "";
             OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;

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
                output += output_stat[i] + "in talbe has " + table_diff + "days data, in parameter is " + ini_diff + "days, result:" + result + ". ";
                Table_DataSet.Reset();
                result = "Flase";
                table_diff = ini_diff = 0;
            }
            conn.Close();
            if (output.Length > 255)
                output = output.Substring(0, 254);
            if(result == "True")
                show_flag = 'N';
            else
                show_flag = 'E';
            in_or_up = insert_or_update("para_check");
            if (in_or_up)
            {
                insert_sql = "insert into status_now(para_name,para_value,para_group,flag,description,create_date,para_title,details) values ('para_check','" + result + "','IT3K_Para','"+show_flag+"','','" + DateTime.Now.ToString() + "','IT3K_para','" + output + "')";
                io.AccessDbclass(insert_sql, db_dir);
            }
            else
            {
                insert_sql = "insert into status_histrory select * from (select para_name,para_value,para_group,flag,description,create_date,para_title,details from status_now where para_name = 'para_check')";
                io.AccessDbclass(insert_sql, db_dir);
                insert_sql = "update status_now set para_value='" + result + "',flag = '" + show_flag + "',create_date = '" + DateTime.Now.ToString() + "',details = '" + output + "' where para_name = 'para_check'";
                io.AccessDbclass(insert_sql, db_dir);
            }
        }
            #endregion
        #region 检查数据文件大小
        public void Check_database_tablespace_size()
        {
            string result = "False", output = "The size of dababase check is";
            float size_para = 31;
            float size_db = 32;
            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;
            Table_DataSet = ROD.ReadDataToDataSet(conn, SQL_stat2, "");
            size_db = Convert.ToSingle(Table_DataSet.Tables[0].Rows[0].ItemArray[0]);
            conn.Close();
            if (size_db < size_para)
            {
                result = "True";
                output += result + "in parameter is:" + size_para + "is" + size_db + "while checked the db.";
                show_flag = 'N';
            }
            else
            {
                result = "Flase";
                output += result + "in parameter is:" + size_para + "but is" + size_db + "while checked the db.";
                show_flag = 'E';
            }
            in_or_up = insert_or_update("db_size");
            if (in_or_up)
            {
                insert_sql = "insert into status_now(para_name,para_value,para_group,flag,description,create_date,para_title,details) values ('db_size','" + result + "','Oracle','" + show_flag + "','','" + DateTime.Now.ToString() + "','Oracle','" + output + "')";
                io.AccessDbclass(insert_sql, db_dir);
            }
            else
            {
                insert_sql = "insert into status_histrory select * from (select para_name,para_value,para_group,flag,description,create_date,para_title,details from status_now where para_name = 'db_size')";
                io.AccessDbclass(insert_sql, db_dir);
                insert_sql = "update status_now set para_value='" + result + "',flag = '" + show_flag + "',create_date = '" + DateTime.Now.ToString() + "',details = '" + output + "' where para_name = 'db_size'";
                io.AccessDbclass(insert_sql, db_dir);
            }
        }
        #endregion
        #region 检查数据备份
        public void Check_database_db_backup()
        {
            string result = "False", output = "The back up of db is:";
            string db_back_para = "SUCCEEDED";
            string db_back = "";
            string log_time = "";
            int error_num = 0;
            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;
            Table_DataSet = ROD.ReadDataToDataSet(conn, SQL_stat3, "");
            log_time = Table_DataSet.Tables[0].Rows[0].ItemArray[0].ToString();
            db_back = Table_DataSet.Tables[0].Rows[0].ItemArray[1].ToString();
            error_num = Convert.ToInt32(Table_DataSet.Tables[0].Rows[0].ItemArray[2]);
            conn.Close();
            if (db_back_para == db_back)
            {
                result = "True";
                output += result + "  backup executed succeeded in " + log_time;
                show_flag = 'N';
            }
            else
            {
                result = "Flase";
                output += result + " there is" + error_num + " errors; executed in " + log_time;
                show_flag = 'E';
            }
            in_or_up = insert_or_update("db_backup");
            if (in_or_up)
            {
                insert_sql = "insert into status_now(para_name,para_value,para_group,flag,description,create_date,para_title,details) values ('db_backup','" + result + "','Oracle','" + show_flag + "','','" + DateTime.Now.ToString() + "','Oracle','" + output + "')";
                io.AccessDbclass(insert_sql, db_dir);
            }
            else
            {
                insert_sql = "insert into status_histrory select * from (select para_name,para_value,para_group,flag,description,create_date,para_title,details from status_now where para_name = 'db_backup')";
                io.AccessDbclass(insert_sql, db_dir);
                insert_sql = "update status_now set para_value='" + result + "',flag = '" + show_flag + "',create_date = '" + DateTime.Now.ToString() + "',details = '" + output + "' where para_name = 'db_backup'";
                io.AccessDbclass(insert_sql, db_dir);
            }
        }
        #endregion
        #region 检查Log日志报错否
        public void Check_database_log_err()
        {
            string result = "False", output = "The result of check error(warning) is ";
            int diff_num = 3;
            int error_num = 5,error_num2 = 5;
            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;
            Table_DataSet = ROD.ReadDataToDataSet(conn, SQL_stat4, "");
            if (Table_DataSet.Tables[0].Rows.Count == 2)
            {
                error_num = Convert.ToInt32(Table_DataSet.Tables[0].Rows[0].ItemArray[0]);
                error_num2 = Convert.ToInt32(Table_DataSet.Tables[0].Rows[1].ItemArray[0]);
            }
            if (Table_DataSet.Tables[0].Rows.Count == 1)
            {
                error_num = Convert.ToInt32(Table_DataSet.Tables[0].Rows[0].ItemArray[0]);
                error_num2 = 0;
            }
            conn.Close();
            if (error_num == 0 && error_num2 < diff_num)
            {
                result = "True";
                output += result + "there is  " + error_num + " errors and " + error_num2 + " warnings.";
                show_flag = 'N';
            }
            else
            {
                result = "Flase";
                output += result + "there is  " + error_num + " errors and " + error_num2 + " warnings.";
                show_flag = 'E';
            }
            in_or_up = insert_or_update("log_error");
            if (in_or_up)
            {
                insert_sql = "insert into status_now(para_name,para_value,para_group,flag,description,create_date,para_title,details) values ('log_error','" + result + "','Log','" + show_flag + "','','" + DateTime.Now.ToString() + "','Log','" + output + "')";
                io.AccessDbclass(insert_sql, db_dir);
            }
            else
            {
                insert_sql = "insert into status_histrory select * from (select para_name,para_value,para_group,flag,description,create_date,para_title,details from status_now where para_name = 'log_error')";
                io.AccessDbclass(insert_sql, db_dir);
                insert_sql = "update status_now set para_value='" + result + "',flag = '" + show_flag + "',create_date = '" + DateTime.Now.ToString() + "',details = '" + output + "' where para_name = 'log_error'";
                io.AccessDbclass(insert_sql, db_dir);
            }
        }
        #endregion
        #region 检查关键表数量是否超出
        public void Check_database_table_num()
        {
            string result = "False", output = "The result of checking key tables is: ";
            int num_count = 50000001;
            OracleConnection conn = ROD.NewConn();
            DataSet Table_DataSet;
            for (int i = 0; i < 7; i++)
            {
                Table_DataSet = ROD.ReadDataToDataSet(conn, SQL_stat5[i], "");
                num_count = Convert.ToInt32(Table_DataSet.Tables[0].Rows[0].ItemArray[0]);
                output += output_stat2[i] + ": " + num_count + "./n";
                if (num_count < SQL5_refrence[i])
                {
                    result = "True";
                    show_flag = 'N';
                }
                if (num_count > SQL5_refrence[i] && num_count < SQL5_refrence[7 + i])
                {
                    result = "True";
                    show_flag = 'W';
                }
                if (num_count > SQL5_refrence[7 + i])
                {
                    result = "False";
                    show_flag = 'E';
                }
            }
            conn.Close();
            if (output.Length > 255)
                output = output.Substring(0, 254);
            in_or_up = insert_or_update("table_count");
            if (in_or_up)
            {
                insert_sql = "insert into status_now(para_name,para_value,para_group,flag,description,create_date,para_title,details) values ('table_count','" + result + "','Oracle','" + show_flag + "','','" + DateTime.Now.ToString() + "','Oracle','" + output + "')";
                io.AccessDbclass(insert_sql, db_dir);
            }
            else
            {
                insert_sql = "insert into status_histrory select * from (select para_name,para_value,para_group,flag,description,create_date,para_title,details from status_now where para_name = 'table_count')";
                io.AccessDbclass(insert_sql, db_dir);
                insert_sql = "update status_now set para_value='" + result + "',flag = '" + show_flag + "',create_date = '" + DateTime.Now.ToString() + "',details = '" + output + "' where para_name = 'table_count'";
                io.AccessDbclass(insert_sql, db_dir);
            }
        }
        #endregion
        public bool insert_or_update(string key)
        {
            string sql_temp = "select count(1) from status_now where para_name = '" + key + "'";
            DataTable dt = io.DbToDatatable(sql_temp, db_dir);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            int flag = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
            if (flag == 0)
                return true;
            else
                return false;
        }
    } 
   
}
