using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Zzh.Backend.Report
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport1()
        {
            InitializeComponent();
            BindData();
        }
        private void BindData()
        {
            this.xrLabel7.Text = "朱振华1aa";
            this.xrLabel2.Text = "aaaaaaaaaaaaaaaa";
        }
    }
}
