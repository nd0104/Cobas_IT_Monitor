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
namespace CobasITMonitor
{
    public partial class Main : Form
    {
        public string[] sql_area_bak = {"select para_value,flag from status_now where para_name = 'Rows_Lis_Message' ",
                                       "select para_value,flag from status_now where para_name = 'Table_Space_Size'"};
        public string[] sql_area = {"select para_value,flag from status_now where para_name = 'Rows_Lis_Message' ",
                                       "select para_value,flag from status_now where para_name = 'Table_Space_Size'"};
        
        IO_tool io = new IO_tool();
        Work.Work worker = new Work.Work();
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
                for (; counter < 2; counter++)
                {
                    if (sql_area[counter] != null)
                    {
                        DataTable dt = io.DbToDatatable(sql_area[counter], db_dir);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
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
            worker.Check_database_para();
            Thread.Sleep(10000);
        }
       #region 根据返回值逐行改变灯的颜色，后期考虑用映射动态组成变量名来缩短代码量
        //
    private void Select_Light(int counter,char flag,string list_box_text)
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

    private void Main_FormClosed(object sender, FormClosedEventArgs e)
    {
        System.Environment.Exit(0);
    }

    }
}
