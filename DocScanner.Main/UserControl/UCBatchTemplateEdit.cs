using DocScanner.LibCommon;
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
        private BatchTemplatedef _curbatch;

        private RadContextMenu _treeContextMenu;

        private RadContextMenu _treeRootNodeContextMenu;

        private RadTreeView _tree;

        private IContainer components = null;

        private Button btnNewTemplate;

        private Button btnSaveTemplate;

        private ListView _listViewTemplates;

        private SplitContainer _splitContainer1;

        private ColumnHeader columnHeaderTemplate;

        public BatchTemplatedef Curbatch
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
            this._tree = new RadTreeView();
            this._tree.Dock = DockStyle.Fill;
            this._tree.AllowEdit = true;
            this._tree.ShowLines = true;
            this._splitContainer1.Panel2.Controls.Add(this._tree);
            this._listViewTemplates.MouseClick += new MouseEventHandler(this.listViewTemplates_MouseClick);
            this._listViewTemplates.DoubleClick += new EventHandler(this.listViewTemplates_DoubleClick);
            List<BatchTemplatedef> templates = BatchTemplateMgr.GetTemplates();
            foreach (BatchTemplatedef current in templates)
            {
                ListViewItem listViewItem = new ListViewItem(current.Name);
                listViewItem.Tag = current;
                this._listViewTemplates.Items.Add(listViewItem);
            }
        }

        private void listViewTemplates_DoubleClick(object sender, EventArgs e)
        {
            bool flag = this._listViewTemplates.SelectedItems.Count > 0;
            if (flag)
            {
                BatchTemplatedef batchTemplatedef = this._listViewTemplates.SelectedItems[0].Tag as BatchTemplatedef;
                bool flag2 = batchTemplatedef != null;
                if (flag2)
                {
                    this._tree.Nodes.Clear();
                    this.Curbatch = batchTemplatedef;
                    RadTreeNode radTreeNode = BatchTemplatedef.CreateRadTreeFromTemplate(this._tree, batchTemplatedef, "根节点");
                    radTreeNode.ExpandAll();
                    bool flag3 = this._tree.Nodes.Count > 0;
                    if (flag3)
                    {
                        this.SetupNodeMenu(this._tree.Nodes[0]);
                    }
                }
            }
        }

        private void SetupNodeMenu(RadTreeNode node)
        {
            bool flag = this._treeContextMenu == null;
            if (flag)
            {
                this._treeContextMenu = new RadContextMenu();
                this._treeRootNodeContextMenu = new RadContextMenu();
                RadMenuItem radMenuItem = new RadMenuItem();
                RadMenuItem radMenuItem2 = new RadMenuItem();
                RadMenuItem radMenuItem3 = new RadMenuItem();
                RadMenuItem radMenuItem4 = new RadMenuItem();
                this._treeContextMenu.Items.Add(radMenuItem2);
                this._treeContextMenu.Items.Add(radMenuItem3);
                this._treeContextMenu.Items.Add(radMenuItem);
                this._treeRootNodeContextMenu.Items.Add(radMenuItem4);
                radMenuItem.Text = "新增";
                radMenuItem.Click += delegate (object sender, EventArgs e)
                {
                    RadTreeNode radTreeNode = this._tree.SelectedNode.Nodes.Add("新分类");
                    node.ExpandAll();
                    radTreeNode.ContextMenu = this._treeContextMenu;
                };
                radMenuItem2.Text = "删除";
                radMenuItem2.Click += delegate (object sender, EventArgs e)
                {
                    this._tree.SelectedNode.Remove();
                };
                radMenuItem3.Text = "修改";
                radMenuItem3.Click += delegate (object sender, EventArgs e)
                {
                    this._tree.BeginEdit();
                };
                radMenuItem4.Text = "新增";
                radMenuItem4.Click += delegate (object sender, EventArgs e)
                {
                    RadTreeNode radTreeNode = this._tree.SelectedNode.Nodes.Add("新分类");
                    node.ExpandAll();
                    radTreeNode.ContextMenu = this._treeContextMenu;
                };
            }
            bool flag2 = node.Parent == null;
            if (flag2)
            {
                node.ContextMenu = this._treeRootNodeContextMenu;
            }
            else
            {
                node.ContextMenu = this._treeContextMenu;
            }
            node.GetChildren().ForEach(delegate (RadTreeNode o)
            {
                o.ContextMenu = this._treeContextMenu;
            });
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
                BatchTemplateMgr.RemoveTemplate(text);
                this._listViewTemplates.Items.Remove(this._listViewTemplates.SelectedItems[0]);
                this._tree.Nodes.Clear();
                this.Curbatch = null;
            }
        }

        private void UpdateTitle()
        {
        }

        private void btnNewTemplate_Click(object sender, EventArgs e)
        {
            UCBatchTempalteName uCBatchTempalteName = new UCBatchTempalteName();
            bool flag = uCBatchTempalteName.ShowInContainer() == DialogResult.OK;
            if (flag)
            {
                this._tree.Nodes.Clear();
                BatchTemplatedef batchTemplatedef = new BatchTemplatedef();
                batchTemplatedef.Name = uCBatchTempalteName.TemplateName;
                RadTreeNode radTreeNode = this._tree.Nodes.Add("新批次");
                this._tree.Refresh();
                this.Curbatch = batchTemplatedef;
                radTreeNode.ExpandAll();
                batchTemplatedef.FromRadTree(this._tree.Nodes[0]);
                this.SetupNodeMenu(radTreeNode);
                ListViewItem listViewItem = new ListViewItem(uCBatchTempalteName.TemplateName);
                listViewItem.Tag = batchTemplatedef;
                this._listViewTemplates.Items.Insert(0, listViewItem);
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            bool flag = this._tree.Nodes.Count == 0;
            if (!flag)
            {
                BatchTemplatedef curbatch = this.Curbatch;
                curbatch.FromRadTree(this._tree.Nodes[0]);
                BatchTemplateMgr.AddUpdateTemplate(curbatch);
            }
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
            this.btnNewTemplate = new Button();
            this.btnSaveTemplate = new Button();
            this._listViewTemplates = new ListView();
            this.columnHeaderTemplate = new ColumnHeader();
            this._splitContainer1 = new SplitContainer();
            ((ISupportInitialize)this._splitContainer1).BeginInit();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            base.SuspendLayout();
            this.btnNewTemplate.Location = new Point(73, 39);
            this.btnNewTemplate.Name = "btnNewTemplate";
            this.btnNewTemplate.Size = new Size(123, 46);
            this.btnNewTemplate.TabIndex = 0;
            this.btnNewTemplate.Text = "新建模版";
            this.btnNewTemplate.UseVisualStyleBackColor = true;
            this.btnNewTemplate.Click += new EventHandler(this.btnNewTemplate_Click);
            this.btnSaveTemplate.Location = new Point(73, 107);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new Size(123, 41);
            this.btnSaveTemplate.TabIndex = 1;
            this.btnSaveTemplate.Text = "保存";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new EventHandler(this.btnSaveTemplate_Click);
            this._listViewTemplates.Columns.AddRange(new ColumnHeader[]
            {
                this.columnHeaderTemplate
            });
            this._listViewTemplates.Dock = DockStyle.Bottom;
            this._listViewTemplates.FullRowSelect = true;
            this._listViewTemplates.GridLines = true;
            this._listViewTemplates.Location = new Point(0, 279);
            this._listViewTemplates.Name = "_listViewTemplates";
            this._listViewTemplates.Size = new Size(286, 340);
            this._listViewTemplates.TabIndex = 2;
            this._listViewTemplates.UseCompatibleStateImageBehavior = false;
            this._listViewTemplates.View = View.Details;
            this.columnHeaderTemplate.Text = "模版";
            this.columnHeaderTemplate.Width = 282;
            this._splitContainer1.Dock = DockStyle.Fill;
            this._splitContainer1.FixedPanel = FixedPanel.Panel1;
            this._splitContainer1.Location = new Point(0, 0);
            this._splitContainer1.Name = "_splitContainer1";
            this._splitContainer1.Panel1.Controls.Add(this._listViewTemplates);
            this._splitContainer1.Panel1.Controls.Add(this.btnNewTemplate);
            this._splitContainer1.Panel1.Controls.Add(this.btnSaveTemplate);
            this._splitContainer1.Size = new Size(1031, 619);
            this._splitContainer1.SplitterDistance = 286;
            this._splitContainer1.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this._splitContainer1);
            base.Name = "UCBatchTemplateEdit";
            base.Size = new Size(1031, 619);
            this._splitContainer1.Panel1.ResumeLayout(false);
            ((ISupportInitialize)this._splitContainer1).EndInit();
            this._splitContainer1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
