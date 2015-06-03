namespace CobasITMonitor
{
    partial class 主界面
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(主界面));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示状态监控界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.软件设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.客户信息设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.监控参数设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.硬盘监控参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpu内存监控参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.网络监控参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成服务器状态报告ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示状态监控界面ToolStripMenuItem,
            this.软件设置ToolStripMenuItem,
            this.客户信息设置ToolStripMenuItem,
            this.监控参数设置ToolStripMenuItem,
            this.生成服务器状态报告ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 136);
            // 
            // 显示状态监控界面ToolStripMenuItem
            // 
            this.显示状态监控界面ToolStripMenuItem.Name = "显示状态监控界面ToolStripMenuItem";
            this.显示状态监控界面ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.显示状态监控界面ToolStripMenuItem.Text = "显示状态监控界面";
            this.显示状态监控界面ToolStripMenuItem.Click += new System.EventHandler(this.显示状态监控界面ToolStripMenuItem_Click);
            // 
            // 软件设置ToolStripMenuItem
            // 
            this.软件设置ToolStripMenuItem.Name = "软件设置ToolStripMenuItem";
            this.软件设置ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.软件设置ToolStripMenuItem.Text = "软件设置";
            this.软件设置ToolStripMenuItem.Click += new System.EventHandler(this.软件设置ToolStripMenuItem_Click);
            // 
            // 客户信息设置ToolStripMenuItem
            // 
            this.客户信息设置ToolStripMenuItem.Name = "客户信息设置ToolStripMenuItem";
            this.客户信息设置ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.客户信息设置ToolStripMenuItem.Text = "客户信息设置";
            this.客户信息设置ToolStripMenuItem.Click += new System.EventHandler(this.客户信息设置ToolStripMenuItem_Click);
            // 
            // 监控参数设置ToolStripMenuItem
            // 
            this.监控参数设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.硬盘监控参数ToolStripMenuItem,
            this.cpu内存监控参数ToolStripMenuItem,
            this.网络监控参数ToolStripMenuItem});
            this.监控参数设置ToolStripMenuItem.Name = "监控参数设置ToolStripMenuItem";
            this.监控参数设置ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.监控参数设置ToolStripMenuItem.Text = "监控参数设置";
            this.监控参数设置ToolStripMenuItem.Click += new System.EventHandler(this.监控参数设置ToolStripMenuItem_Click);
            // 
            // 硬盘监控参数ToolStripMenuItem
            // 
            this.硬盘监控参数ToolStripMenuItem.Name = "硬盘监控参数ToolStripMenuItem";
            this.硬盘监控参数ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.硬盘监控参数ToolStripMenuItem.Text = "硬盘监控参数";
            this.硬盘监控参数ToolStripMenuItem.Click += new System.EventHandler(this.硬盘监控参数ToolStripMenuItem_Click);
            // 
            // cpu内存监控参数ToolStripMenuItem
            // 
            this.cpu内存监控参数ToolStripMenuItem.Name = "cpu内存监控参数ToolStripMenuItem";
            this.cpu内存监控参数ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.cpu内存监控参数ToolStripMenuItem.Text = "cpu/内存监控参数";
            this.cpu内存监控参数ToolStripMenuItem.Click += new System.EventHandler(this.cpu内存监控参数ToolStripMenuItem_Click);
            // 
            // 网络监控参数ToolStripMenuItem
            // 
            this.网络监控参数ToolStripMenuItem.Name = "网络监控参数ToolStripMenuItem";
            this.网络监控参数ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.网络监控参数ToolStripMenuItem.Text = "网络监控参数";
            this.网络监控参数ToolStripMenuItem.Click += new System.EventHandler(this.网络监控参数ToolStripMenuItem_Click);
            // 
            // 生成服务器状态报告ToolStripMenuItem
            // 
            this.生成服务器状态报告ToolStripMenuItem.Name = "生成服务器状态报告ToolStripMenuItem";
            this.生成服务器状态报告ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.生成服务器状态报告ToolStripMenuItem.Text = "生成服务器状态报告";
            this.生成服务器状态报告ToolStripMenuItem.Click += new System.EventHandler(this.生成服务器状态报告ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(118, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(33, 16);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(193, 52);
            this.textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(104, 232);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 5;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(13, 89);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(267, 66);
            this.textBox4.TabIndex = 6;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridView1.Location = new System.Drawing.Point(341, 75);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(348, 150);
            this.dataGridView1.TabIndex = 7;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(374, 16);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "jiemi";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(568, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "jiami";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // 主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 273);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "主界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IT3000保养监控";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.主界面_FormClosed);
            this.Load += new System.EventHandler(this.主界面_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示状态监控界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 软件设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 客户信息设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 监控参数设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成服务器状态报告ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolStripMenuItem 硬盘监控参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cpu内存监控参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 网络监控参数ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}

