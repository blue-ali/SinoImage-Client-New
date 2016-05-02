using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCLeftPane : UserControl
    {
        private IContainer components = null;

        private ToolStrip toolStrip1;

        private ToolStripButton tspBtn_NewBatch;

        private ToolStripButton tspBtn_ClearBatchs;

        private ToolStripButton tspBtn_Filter;

        private ToolStripButton tspBtn_View;

        private UCNavigatorBar ucNavigatorBar1;

        public UCLeftPane()
        {
            this.InitializeComponent();
            //UCNavigatorBar uCNavigatorBar = new UCNavigatorBar();
            //base.Controls.Add(uCNavigatorBar);
            //uCNavigatorBar.Dock = DockStyle.Fill;
        }

        public UCNavigatorBar GetBar()
        {
            return this.ucNavigatorBar1;
        }

        private void tspBtn_NewBatch_Click(object sender, EventArgs e)
        {
            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("NewBatch", null);
        }

        private void tspBtn_ClearBatchs_Click(object sender, EventArgs e)
        {
            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("ClearBatchs", null);
        }

        private void tspBtn_Filter_Click(object sender, EventArgs e)
        {
            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("FilterImg", null);
        }

        private void tspBtn_View_Click(object sender, EventArgs e)
        {
            LibCommon.AppContext.Cur.GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD("ChangeView", null);
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tspBtn_NewBatch = new System.Windows.Forms.ToolStripButton();
            this.tspBtn_ClearBatchs = new System.Windows.Forms.ToolStripButton();
            this.tspBtn_View = new System.Windows.Forms.ToolStripButton();
            this.tspBtn_Filter = new System.Windows.Forms.ToolStripButton();
            this.ucNavigatorBar1 = new DocScanner.Main.UCNavigatorBar();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspBtn_NewBatch,
            this.tspBtn_ClearBatchs,
            this.tspBtn_View,
            this.tspBtn_Filter});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(235, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tspBtn_NewBatch
            // 
            this.tspBtn_NewBatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tspBtn_NewBatch.Image = global::DocScanner.Main.Properties.Resources.newbatch;
            this.tspBtn_NewBatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspBtn_NewBatch.Name = "tspBtn_NewBatch";
            this.tspBtn_NewBatch.Size = new System.Drawing.Size(24, 24);
            this.tspBtn_NewBatch.Text = "新建批次";
            this.tspBtn_NewBatch.Click += new System.EventHandler(this.tspBtn_NewBatch_Click);
            // 
            // tspBtn_ClearBatchs
            // 
            this.tspBtn_ClearBatchs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tspBtn_ClearBatchs.Image = global::DocScanner.Main.Properties.Resources.del;
            this.tspBtn_ClearBatchs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspBtn_ClearBatchs.Name = "tspBtn_ClearBatchs";
            this.tspBtn_ClearBatchs.Size = new System.Drawing.Size(24, 24);
            this.tspBtn_ClearBatchs.Text = "清空批次";
            this.tspBtn_ClearBatchs.Click += new System.EventHandler(this.tspBtn_ClearBatchs_Click);
            // 
            // tspBtn_View
            // 
            this.tspBtn_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tspBtn_View.Image = global::DocScanner.Main.Properties.Resources.view;
            this.tspBtn_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspBtn_View.Name = "tspBtn_View";
            this.tspBtn_View.Size = new System.Drawing.Size(24, 24);
            this.tspBtn_View.Text = "视图";
            this.tspBtn_View.Click += new System.EventHandler(this.tspBtn_View_Click);
            // 
            // tspBtn_Filter
            // 
            this.tspBtn_Filter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tspBtn_Filter.Image = global::DocScanner.Main.Properties.Resources.Filter;
            this.tspBtn_Filter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspBtn_Filter.Name = "tspBtn_Filter";
            this.tspBtn_Filter.Size = new System.Drawing.Size(24, 24);
            this.tspBtn_Filter.Text = "采集过滤转换";
            this.tspBtn_Filter.Click += new System.EventHandler(this.tspBtn_Filter_Click);
            // 
            // ucNavigatorBar1
            // 
            this.ucNavigatorBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavigatorBar1.Location = new System.Drawing.Point(0, 27);
            this.ucNavigatorBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucNavigatorBar1.Name = "ucNavigatorBar1";
            this.ucNavigatorBar1.Size = new System.Drawing.Size(235, 495);
            this.ucNavigatorBar1.TabIndex = 1;
            // 
            // UCLeftPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucNavigatorBar1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UCLeftPane";
            this.Size = new System.Drawing.Size(235, 522);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
