using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CobasITMonitor
{
    public partial class customerconfig : Form
    {
        public customerconfig()
        {
            InitializeComponent();
        }
        Tool_Class.IO_tool tool = new Tool_Class.IO_tool();
        private void customerconfig_Load(object sender, EventArgs e)
        {
            textBox1.Text = tool.readconfig("customernode", "hospitalname");
            textBox2.Text = tool.readconfig("customernode", "person");
            textBox3.Text = tool.readconfig("customernode", "phone");
            textBox4.Text = tool.readconfig("customernode", "emailaddress");
            string customerarea = tool.readconfig("customernode", "customerarea");
            switch (customerarea)
            {
                case "east":
                    comboBox1.Text = "east";
                    break;
                case "west":
                    comboBox1.Text = "west";
                    break;
                case "north":
                    comboBox1.Text = "north";
                    break;
                case "south":
                    comboBox1.Text = "south";
                    break;

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tool.writeconfig("customernode", "hospitalname", textBox1.Text);
            tool.writeconfig("customernode", "person", textBox2.Text);
            tool.writeconfig("customernode", "phone", textBox3.Text);
            tool.writeconfig("customernode", "emailaddress", textBox4.Text);
            tool.writeconfig("customernode", "customerarea", comboBox1.Text);
            MessageBox.Show("修改成功");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
