using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool_Class;
namespace CobasITMonitor
{
    public partial class sizeofdb : Form
    {
        public sizeofdb()
        {
            InitializeComponent();
        }
        Tool_Class.IO_tool io = new Tool_Class.IO_tool();
        private void button1_Click(object sender, EventArgs e)
        {
            int out_size = 32;
            bool result = io.isNumberic(textBox1.Text.ToString(), out out_size);
            if(!result)
                textBox1.Text = "只能输入数字，默认单位G";
        }
    }
}
