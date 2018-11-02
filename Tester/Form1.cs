using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SHWY.Utility;
using System.Threading;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int threadid = 0;
        int s = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            AsyncHelper.RunAsync(delegate ()
            {
                threadid = Thread.CurrentThread.ManagedThreadId;
                while (s<10)
                {
                    s++;

                    Callback();
                    Thread.Sleep(1000);
                }
            }, Callback);
        }
        private void Callback()
        {
            threadid = Thread.CurrentThread.ManagedThreadId;
            MessageBox.Show("3:S：" + s);
        }
    }
}
