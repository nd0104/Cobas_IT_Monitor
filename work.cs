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
        private string[] SQL_stat2 = {"SELECT round(bytes / (1024 * 1024), 0) MB FROM dba_data_files where tablespace_name = 'TS_DATEN' ",
                                     };
        public void Check_database_para()
        {
            #region 检查数据表是否和设置的参数一致
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
