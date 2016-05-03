using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public partial class TestFormMain : Form
    {
        public TestFormMain()
        {
            //DocTestClass test = new TestClass();
            DocScanner.OCR.TestClass test = new OCR.TestClass();
            //test.Main();
            this.InitializeComponent();
            this.Text = "影像文档客户端";
            base.Load += new EventHandler(this.TestFormMain_Load);
        }
        private void TestFormMain_Load(object sender, EventArgs e)
        {
            string path = @"F:\_picture\4.jpg";
            if (File.Exists(path))
            {
            }
        }

    }
}
