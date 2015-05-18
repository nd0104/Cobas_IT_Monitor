using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Work;
using Tool_Class;
using System.Reflection;
using System.Threading;
using IT3000保养监控;
namespace CobasITMonitor
{
    public partial class Main : Form
    {
        public string[] sql_area_bak = {"select details,flag from status_now where para_name = 'db_size' ",
                                       "select details,flag from status_now where para_name = 'table_count' ",
                                       "select details,flag from status_now where para_name = 'db_backup'",
                                        "select details,flag from status_now where para_name = 'para_check'",
                                        "select details,flag from status_now where para_name = 'log_error'"};
                                       /* "select para_value,flag from status_now where para_name = 'syslog_warn'",
                                       "select para_value,flag from status_now where para_name = 'instrument_connection' ",
                                        "select para_value,flag from status_now where para_name = 'disk_size'",
                                       "select para_value,flag from status_now where para_name = 'cpu_running' ",
                                        "select para_value,flag from status_now where para_name = 'memory_running'"*/
        public string[] sql_area = {"select details,flag from status_now where para_name = 'db_size' ",
                                       "select details,flag from status_now where para_name = 'table_count' ",
                                       "select details,flag from status_now where para_name = 'db_backup'",
                                        "select details,flag from status_now where para_name = 'para_check'",
                                        "select details,flag from status_now where para_name = 'log_error'"};

        IO_tool io = new IO_tool();
        Work.Work worker = new Work.Work();
      //  string db_dir = System.Windows.Forms.Application.StartupPath + "\\db.accdb";
        string db_dir = @"E:\code\CobasITMonitor\CobasITMonitor\db.accdb";
        public Main()
        {
            InitializeComponent();
            Main.CheckForIllegalCrossThreadCalls = false;
            Thread[] threads = new Thread[2];
            threads[0] = new Thread(new ThreadStart(main_thread));
            threads[1] = new Thread(new ThreadStart(monitor_thread));
            threads[0].Start();
            threads[1].Start();
        }
        #region 主线程
        void main_thread()
        {
            int counter = 0;
            while (true)
            {
                for (; counter < 5; counter++)
                {
                    if (sql_area[counter] != null)
                    {
                        DataTable dt = io.DbToDatatable(sql_area[counter], db_dir);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
                        string dd = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        char flag = Convert.ToChar(ds.Tables[0].Rows[0].ItemArray[1]);
                        string list_box_text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        Select_Light(counter, flag, list_box_text);
                    }
                }
                counter = 0;
                Thread.Sleep(10000);
            }
        }
        #endregion
        void monitor_thread()
        {
            while (true)
            {
                //worker.Check_database_para();
                // worker.Check_database_tablespace_size();
                //    worker.Check_database_db_backup();
                //worker.Check_database_log_err();
             //   worker.Check_database_table_num();
                Thread.Sleep(10000);
            }
        }
        #region 根据返回值逐行改变灯的颜色，后期考虑用映射动态组成变量名来缩短代码量

        private void Select_Light(int counter, char flag, string list_box_text)
        {
            switch (counter)
            {
                case 0:
                     listBox1.Items.Clear();
                    listBox1.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox1.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox1.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox1.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;
                case 1:
                       listBox2.Items.Clear();
                    listBox2.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox2.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox2.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox2.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;
                case 2:
                       listBox3.Items.Clear();
                    listBox3.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox3.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox3.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox3.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;
                case 3:
                       listBox4.Items.Clear();
                    listBox4.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox4.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox4.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox4.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;
                case 4:
                       listBox5.Items.Clear();
                    listBox5.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox5.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox5.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox5.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;
        /*        case 5:
                    //   listBox2.Items.Clear();
                    listBox14.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox14.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox14.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox14.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;
                case 6:
                    //   listBox2.Items.Clear();
                    listBox5.Items.Add(list_box_text);
                    switch (flag)
                    {
                        case 'E':
                            pictureBox5.Image = CobasITMonitor.Properties.Resources.red;
                            break;
                        case 'N':
                            pictureBox5.Image = CobasITMonitor.Properties.Resources.green;
                            break;
                        case 'W':
                            pictureBox5.Image = CobasITMonitor.Properties.Resources.yellow;
                            break;
                        default:
                            break;
                    };
                    break;*/

                default: break;
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Numoftable not = new Numoftable();
            not.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sizeofdb sdb = new sizeofdb();
            sdb.Show();
        }
        private void checkbox1_changed(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                sql_area[0] = sql_area_bak[0];
            else
            {
                sql_area[0] = null;
                pictureBox1.Image = CobasITMonitor.Properties.Resources.pause_;
            }
        }
        private void checkbox2_changed(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                sql_area[1] = sql_area_bak[1];
            else
            {
                sql_area[1] = null;
                pictureBox2.Image = CobasITMonitor.Properties.Resources.pause_;
            }
        }


        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked)
                sql_area[2] = sql_area_bak[2];
            else
            {
                sql_area[2] = null;
                pictureBox16.Image = CobasITMonitor.Properties.Resources.pause_;
            }
        }


        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
                sql_area[3] = sql_area_bak[3];
            else
            {
                sql_area[3] = null;
                pictureBox10.Image = CobasITMonitor.Properties.Resources.pause_;
            }

        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
                sql_area[4] = sql_area_bak[4];
            else
            {
                sql_area[4] = null;
                pictureBox12.Image = CobasITMonitor.Properties.Resources.pause_;
            }

        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
                sql_area[5] = sql_area_bak[5];
            else
            {
                sql_area[5] = null;
                pictureBox14.Image = CobasITMonitor.Properties.Resources.pause_;
            }

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                sql_area[6] = sql_area_bak[6];
            else
            {
                sql_area[6] = null;
                pictureBox5.Image = CobasITMonitor.Properties.Resources.pause_;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CPUMEM cc = new CPUMEM();
            cc.ShowDialog();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            CPUMEM cc = new CPUMEM();
            cc.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            diskconfig dd = new diskconfig();
            dd.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            连通性参数配置 ip = new 连通性参数配置();
            ip.ShowDialog();

        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
