using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public class TableLayoutPanelEx : TableLayoutPanel
    {
        // Fields
        private float[] _columnsWidths = null;
        private List<int> _hborders = new List<int>();
        private float[] _rowsHeights = null;
        private int _selColumn = -1;
        private int _selRow = -1;
        private List<int> _vborders = new List<int>();
        private const int MK_LBUTTON = 1;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_NCHITTEST = 0x84;

        // Methods
        public TableLayoutPanelEx()
        {
            this.DoubleBuffered = true;
            base.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            string configParamValue = IniConfigSetting.Cur.GetConfigParamValue("UISetting", "TablayoutWidths");
            string str2 = IniConfigSetting.Cur.GetConfigParamValue("UISetting", "TablayoutHeights");
            if (!string.IsNullOrEmpty(configParamValue))
            {
                char[] separator = new char[] { ',' };
                //this._columnsWidths = configParamValue.Split(separator).Select<string, float>((<> c.<> 9__0_0 ?? (<> c.<> 9__0_0 = new Func<string, float>(<> c.<> 9.<.ctor > b__0_0)))).ToArray<float>();
                this._columnsWidths = configParamValue.Split(separator).Select<string, float>(o => o.ToFloat()).ToArray<float>();
            }
            if (!string.IsNullOrEmpty(str2))
            {
                char[] chArray2 = new char[] { ',' };
                //this._rowsHeights = str2.Split(chArray2).Select<string, float>((<> c.<> 9__0_1 ?? (<> c.<> 9__0_1 = new Func<string, float>(<> c.<> 9.<.ctor > b__0_1)))).ToArray<float>();
                this._rowsHeights = str2.Split(chArray2).Select<string, float>(o => o.ToFloat()).ToArray<float>();
            }
            base.HandleDestroyed += new EventHandler(this.TableLayoutPanelEx_HandleDestroyed);
        }

        private void TableLayoutPanelEx_HandleDestroyed(object sender, EventArgs e)
        {
            if (this._columnsWidths == null)
            {
                this._columnsWidths = new float[base.GetColumnWidths().Length];
            }
            if (this._rowsHeights == null)
            {
                this._rowsHeights = new float[base.GetRowHeights().Length];
            }
            for (int i = 0; i < base.GetColumnWidths().Length; i++)
            {
                this._columnsWidths[i] = base.ColumnStyles[i].Width;
            }
            for (int j = 0; j < base.GetRowHeights().Length; j++)
            {
                this._rowsHeights[j] = base.RowStyles[j].Height;
            }
            //string str = string.Join(",", this._columnsWidths.Select<float, string>((<> c.<> 9__1_0 ?? (<> c.<> 9__1_0 = new Func<float, string>(<> c.<> 9.< TableLayoutPanelEx_HandleDestroyed > b__1_0)))).ToArray<string>());
            string str = string.Join(",", this._columnsWidths.Select<float, string>(o => o.ToString()).ToArray<string>());
            //string str2 = string.Join(",", this._rowsHeights.Select<float, string>((<> c.<> 9__1_1 ?? (<> c.<> 9__1_1 = new Func<float, string>(<> c.<> 9.< TableLayoutPanelEx_HandleDestroyed > b__1_1)))).ToArray<string>());
            string str2 = string.Join(",", this._rowsHeights.Select<float, string>(o => o.ToString()).ToArray<string>());
            IniConfigSetting.Cur.SetConfigParamValue("UISetting", "TablayoutWidths", str);
            IniConfigSetting.Cur.SetConfigParamValue("UISetting", "TablayoutHeights", str2);
        }

        public bool LoadLayout()
        {
            if (this._columnsWidths != null)
            {
                for (int i = 0; i < base.GetColumnWidths().Length; i++)
                {
                    base.ColumnStyles[i].Width = this._columnsWidths[i];
                }
            }
            if (this._rowsHeights != null)
            {
                for (int j = 0; j < base.GetRowHeights().Length; j++)
                {
                    base.RowStyles[j].Height = this._rowsHeights[j];
                }
            }
            return true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            this.LoadLayout();
            base.OnHandleCreated(e);
            if (!base.DesignMode)
            {
                this.ResetSizeAndSizeTypes();
            }
        }

        public void ResetSizeAndSizeTypes()
        {
            float num = Convert.ToSingle((int)((base.ClientSize.Width / base.GetColumnWidths().Length) - 1));
            for (int i = 0; i <= (base.GetColumnWidths().Length - 1); i++)
            {
                base.ColumnStyles[i].SizeType = SizeType.Absolute;
            }
            float num2 = Convert.ToSingle((int)((base.ClientSize.Height / base.GetRowHeights().Length) - 1));
            for (int j = 0; j <= (base.GetRowHeights().Length - 1); j++)
            {
                base.RowStyles[j].SizeType = SizeType.Absolute;
            }
        }

       


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (base.Created & !base.Disposing)
            {
                if (m.Msg == 0x84)
                {
                    Point point = base.PointToClient(Control.MousePosition);
                    this._vborders.Clear();
                    this._hborders.Clear();
                    if (this.ColumnCount > 1)
                    {
                        for (int i = 0; i <= (base.GetColumnWidths().Length - 2); i++)
                        {
                            if (i == 0)
                            {
                                this._vborders.Add(base.GetColumnWidths()[i]);
                            }
                            else
                            {
                                this._vborders.Add(this._vborders[this._vborders.Count - 1] + base.GetColumnWidths()[i]);
                            }
                        }
                    }
                    if (this.RowCount > 1)
                    {
                        for (int j = 0; j <= (base.GetRowHeights().Length - 2); j++)
                        {
                            if (j == 0)
                            {
                                this._hborders.Add(base.GetRowHeights()[j]);
                            }
                            else
                            {
                                this._hborders.Add(this._hborders[this._hborders.Count - 1] + base.GetRowHeights()[j]);
                            }
                        }
                    }
                    bool flag3 = (this._vborders.Contains(point.X) | this._vborders.Contains(point.X - 1)) | this._vborders.Contains(point.X + 1);
                    bool flag4 = (this._hborders.Contains(point.Y) | this._hborders.Contains(point.Y - 1)) | this._hborders.Contains(point.Y + 1);
                    if (flag3 & flag4)
                    {
                        this.Cursor = Cursors.SizeAll;
                    }
                    else if (flag3)
                    {
                        this.Cursor = Cursors.VSplit;
                    }
                    else if (flag4)
                    {
                        this.Cursor = Cursors.HSplit;
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (!((m.Msg == 0x201) & (this.Cursor != Cursors.Default)))
                {
                    if ((m.Msg == 0x200) & (m.WParam.ToInt32() == 1))
                    {
                        Point point3 = base.PointToClient(Control.MousePosition);
                        if (this.Cursor != Cursors.Default)
                        {
                            if (((this._selRow > -1) & (point3.Y >= 1)) & (point3.Y <= (base.ClientSize.Height - 2)))
                            {
                                base.RowStyles[this._selRow].SizeType = SizeType.Absolute;
                                float num5 = point3.Y - base.RowStyles[this._selRow].Height;
                                if (this._selRow > 0)
                                {
                                    num5 -= (float)this._hborders[this._selRow - 1];
                                }
                                if ((base.RowStyles[this._selRow].Height + num5) > 0f)
                                {
                                    if (this.RowCount > (this._selRow + 1))
                                    {
                                        if ((base.RowStyles[this._selRow + 1].Height - num5) < 1f)
                                        {
                                            return;
                                        }
                                        RowStyle style1 = base.RowStyles[this._selRow + 1];
                                        style1.Height -= num5;
                                    }
                                    RowStyle style2 = base.RowStyles[this._selRow];
                                    style2.Height += num5;
                                }
                            }
                            if (((this._selColumn > -1) & (point3.X >= 1)) & (point3.X <= (base.ClientSize.Width - 2)))
                            {
                                base.ColumnStyles[this._selColumn].SizeType = SizeType.Absolute;
                                float num6 = point3.X - base.ColumnStyles[this._selColumn].Width;
                                if (this._selColumn > 0)
                                {
                                    num6 -= (float)this._vborders[this._selColumn - 1];
                                }
                                if ((base.ColumnStyles[this._selColumn].Width + num6) > 0f)
                                {
                                    if (this.ColumnCount > (this._selColumn + 1))
                                    {
                                        if ((base.ColumnStyles[this._selColumn + 1].Width - num6) < 1f)
                                        {
                                            return;
                                        }
                                        ColumnStyle style3 = base.ColumnStyles[this._selColumn + 1];
                                        style3.Width -= num6;
                                    }
                                    ColumnStyle style4 = base.ColumnStyles[this._selColumn];
                                    style4.Width += num6;
                                }
                            }
                        }
                    }
                    else if (m.Msg == 0x202)
                    {
                        this._selColumn = -1;
                        this._selRow = -1;
                    }
                }
                else
                {
                    Point point2 = base.PointToClient(Control.MousePosition);
                    this._selColumn = -1;
                    this._selRow = -1;
                    for (int k = 0; k <= (this._vborders.Count - 1); k++)
                    {
                        if ((this._vborders[k] >= (point2.X - 1)) & (this._vborders[k] <= (point2.X + 1)))
                        {
                            this._selColumn = k;
                            break;
                        }
                    }
                    for (int n = 0; n <= (this._hborders.Count - 1); n++)
                    {
                        if ((this._hborders[n] >= (point2.Y - 1)) & (this._hborders[n] <= (point2.Y + 1)))
                        {
                            this._selRow = n;
                            break;
                        }
                    }
                }
            }
        }

        // Properties
        public int ColumnCount
        {
            get
            {
                return base.ColumnCount;
            }
            set
            {
                base.ColumnCount = value;
                if (base.Created & !base.DesignMode)
                {
                    this.ResetSizeAndSizeTypes();
                }
            }
        }

        public int RowCount
        {
            get
            {
                return base.RowCount;
            }
            set
            {
                base.RowCount = value;
                if (base.Created & !base.DesignMode)
                {
                    this.ResetSizeAndSizeTypes();
                }
            }
        }

        // Nested Types
       
    }

}
