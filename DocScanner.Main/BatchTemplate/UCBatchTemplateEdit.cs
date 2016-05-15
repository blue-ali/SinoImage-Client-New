using DocScanner.LibCommon;
using DocScanner.Main.BatchTemplate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCBatchTemplateEdit : UserControl
    {
        private BatchTemplateDef _curbatch;

        private RadContextMenu _treeContextMenu;

        private RadContextMenu _treeRootNodeContextMenu;

        //private RadTreeView _tree;
        private BatchTemplateTree currentTemplateTree;

        private IContainer components = null;

        private Button btnNewTemplate;

        private Button btnSaveTemplate;

        private ListView _listViewTemplates;

        private SplitContainer _splitContainer1;
        private ColumnHeader columnHeaderTemplate;



        public BatchTemplateDef Curbatch
        {
            get
            {
                return this._curbatch;
            }
            set
            {
                this._curbatch = value;
                base.Parent.Text = ((this.Title + this._curbatch == null) ? "" : this._curbatch.Name);
            }
        }

        public string Title
        {
            get
            {
                return "模版编辑";
            }
        }

        public UCBatchTemplateEdit()
        {
            this.InitializeComponent();
            currentTemplateTree = BatchTemplateTree.CreateTreeFromTemplate(null);
              //tree.Dock = DockStyle.Fill;
            //tree.AllowEdit = true;
            //tree.ShowLines = true;
            //this._splitContainer1.Panel2.Controls.Add(cutree
            this._splitContainer1.Panel2.Controls.Add(currentTemplateTree.GetRadTreeView());
            this._listViewTemplates.MouseClick += new MouseEventHandler(this.listViewTemplates_MouseClick);
            this._listViewTemplates.DoubleClick += new EventHandler(this.listViewTemplates_DoubleClick);
            List<BatchTemplateDef> templates = BatchTemplateMgr.GetTemplates();
            foreach (BatchTemplateDef template in templates)
            {
                ListViewItem listViewItem = new ListViewItem(template.Name);
                listViewItem.Tag = BatchTemplateTree.CreateTreeFromTemplate(template);
                this._listViewTemplates.Items.Add(listViewItem);
            }
        }

        private void listViewTemplates_DoubleClick(object sender, EventArgs e)
        {
            if (this._listViewTemplates.SelectedItems.Count > 0)
            {
                BatchTemplateTree templateTree = this._listViewTemplates.SelectedItems[0].Tag as BatchTemplateTree;
                if (templateTree != null)
                {
                    this._splitContainer1.Panel2.Controls.Clear();
                    currentTemplateTree = templateTree;
                    currentTemplateTree.ExpandAll();
                    //this._splitContainer1.Panel2.Controls.Remove(currentTemplateTree);
                    this._splitContainer1.Panel2.Controls.Add(currentTemplateTree.GetRadTreeView());
                    this._splitContainer1.Panel2.Refresh();
                }
                //BatchTemplateDef batchTemplate = this._listViewTemplates.SelectedItems[0].Tag as BatchTemplateDef;
                //if (batchTemplate != null)
                //{
                //    //templateTree.ClearNodes();
                //    this.Curbatch = batchTemplate;
                //    //templateTree.CreateTreeFromTemplate(batchTemplate);
                //    BatchTemplateTree templateTree = BatchTemplateTree.CreateTreeFromTemplate(batchTemplate);
                //    currentTemplateTree = templateTree;
                //    this._splitContainer1.Panel2.Controls.Remove(currentTemplateTree);
                //    this._splitContainer1.Panel2.Controls.Add(currentTemplateTree);
                //    currentTemplateTree.ExpandAll();
                //    //TODO Expand Node
                //}
            }
        }

        private void listViewTemplates_MouseClick(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Right;
            if (flag)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem();
                menuItem.Click += new EventHandler(this.MenuTemplateDel_Click);
                menuItem.Text = "删除";
                contextMenu.MenuItems.Add(menuItem);
                contextMenu.Show(this._listViewTemplates, e.Location);
            }
        }

        private void MenuTemplateDel_Click(object sender, EventArgs e)
        {
            bool flag = this._listViewTemplates.SelectedItems.Count > 0;
            if (flag)
            {
                string text = this._listViewTemplates.SelectedItems[0].Text;
                //BatchTemplateMgr.RemoveTemplate(text);
                this._listViewTemplates.Items.Remove(this._listViewTemplates.SelectedItems[0]);
                currentTemplateTree.Clear();
                //this.Curbatch = null;
            }
        }

        private void btnNewTemplate_Click(object sender, EventArgs e)
        {
            UCBatchTempalteName uCBatchTempalteName = new UCBatchTempalteName();
            if (uCBatchTempalteName.ShowInContainer() == DialogResult.OK)
            {
                BatchTemplateDef batchTemplate = new BatchTemplateDef();
                batchTemplate.Name = uCBatchTempalteName.TemplateName;
                //根据模板创建树
                BatchTemplateTree templateTree = BatchTemplateTree.CreateTreeFromTemplate(batchTemplate);

                //在右边视图中重新生成树
                this._splitContainer1.Panel2.Controls.Clear();
                currentTemplateTree = templateTree;
                this._splitContainer1.Panel2.Controls.Add(currentTemplateTree.GetRadTreeView());
                this._splitContainer1.Panel2.Refresh();
                //currentTemplateTree.Refresh();

                //列表中添加新项，并保存树
                ListViewItem listViewItem = new ListViewItem(uCBatchTempalteName.TemplateName);
                listViewItem.Tag = templateTree;
                this._listViewTemplates.Items.Add(listViewItem);
                //this._listViewTemplates.Items.Insert(0, listViewItem);
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {   
            BatchTemplateMgr.GetTemplates().Clear();
            foreach (ListViewItem item in this._listViewTemplates.Items)
            {
                BatchTemplateMgr.AddUpdateTemplate(BatchTemplateDef.FromBatchTemplateTree((item.Tag as BatchTemplateTree), item.Text));
            }
            BatchTemplateMgr.SaveTemplates();
        }

        public static void DoTest(object param)
        {
            FormContainer formContainer = new FormContainer();
            formContainer.SetControl(typeof(UCBatchTemplateEdit));
            formContainer.TopMost = true;
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
            this.btnNewTemplate = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this._listViewTemplates = new System.Windows.Forms.ListView();
            this.columnHeaderTemplate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).BeginInit();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNewTemplate
            // 
            this.btnNewTemplate.Location = new System.Drawing.Point(55, 31);
            this.btnNewTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnNewTemplate.Name = "btnNewTemplate";
            this.btnNewTemplate.Size = new System.Drawing.Size(92, 37);
            this.btnNewTemplate.TabIndex = 0;
            this.btnNewTemplate.Text = "新建模版";
            this.btnNewTemplate.UseVisualStyleBackColor = true;
            this.btnNewTemplate.Click += new System.EventHandler(this.btnNewTemplate_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Location = new System.Drawing.Point(55, 86);
            this.btnSaveTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(92, 33);
            this.btnSaveTemplate.TabIndex = 1;
            this.btnSaveTemplate.Text = "保存";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // _listViewTemplates
            // 
            this._listViewTemplates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTemplate});
            this._listViewTemplates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._listViewTemplates.FullRowSelect = true;
            this._listViewTemplates.GridLines = true;
            this._listViewTemplates.Location = new System.Drawing.Point(0, 222);
            this._listViewTemplates.Margin = new System.Windows.Forms.Padding(2);
            this._listViewTemplates.Name = "_listViewTemplates";
            this._listViewTemplates.Size = new System.Drawing.Size(286, 273);
            this._listViewTemplates.TabIndex = 2;
            this._listViewTemplates.UseCompatibleStateImageBehavior = false;
            this._listViewTemplates.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTemplate
            // 
            this.columnHeaderTemplate.Text = "模版";
            this.columnHeaderTemplate.Width = 282;
            // 
            // _splitContainer1
            // 
            this._splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer1.Location = new System.Drawing.Point(0, 0);
            this._splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this._splitContainer1.Name = "_splitContainer1";
            // 
            // _splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Controls.Add(this._listViewTemplates);
            this._splitContainer1.Panel1.Controls.Add(this.btnNewTemplate);
            this._splitContainer1.Panel1.Controls.Add(this.btnSaveTemplate);
            this._splitContainer1.Size = new System.Drawing.Size(773, 495);
            this._splitContainer1.SplitterDistance = 286;
            this._splitContainer1.SplitterWidth = 3;
            this._splitContainer1.TabIndex = 3;
            // 
            // UCBatchTemplateEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCBatchTemplateEdit";
            this.Size = new System.Drawing.Size(773, 495);
            this._splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).EndInit();
            this._splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
