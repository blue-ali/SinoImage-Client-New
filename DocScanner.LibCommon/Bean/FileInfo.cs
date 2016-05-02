using Google.ProtocolBuffers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DocScaner.Common;
using DocScanner.LibCommon;
using DocScanner.CodeUtils;
using DocScanner.Bean.pb;

namespace DocScanner.Bean
{
    public class NFileInfo : IMyOpe<NFileInfo>
	{
		private List<NNoteInfo> _notes = new List<NNoteInfo>();

		public event EventHandler DataChanged;

		[Category("文件信息"), DisplayName("作者")]
		public string Author
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("版本")]
		public int Version
		{
			get;
			set;
		}

		//[Category("文件信息"), DisplayName("创建日期")]
		//public int CreateDate
		//{
		//	get;
		//	set;
		//}

		[Category("文件信息"), DisplayName("创建时间")]
		public string CreateTime
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("备注")]
		public string Remark
		{
			get;
			set;
		}

		public IList<NNoteInfo> NotesList
		{
			get
			{
				return this._notes;
			}
			set
			{
				this._notes.Clear();
				this._notes.AddRange(value);
			}
		}

		[Browsable(false)]
		public bool Changed
		{
			get;
			set;
		}

		[Browsable(false)]
		public string DisplayName
		{
			get
			{
				bool flag = NodeTitleTypeSetting.FileNodeTitleType == ENFileNodeTitleType.FileName;
				string result;
				if (flag)
				{
					result = FileHelper.GetFileName(this.FileName);
				}
				else
				{
					bool flag2 = NodeTitleTypeSetting.FileNodeTitleType == ENFileNodeTitleType.FullPath;
					if (flag2)
					{
						result = this.LocalPath;
					}
					else
					{
						bool flag3 = NodeTitleTypeSetting.FileNodeTitleType == ENFileNodeTitleType.Serial;
						if (flag3)
						{
							result = this.FileNO;
						}
						else
						{
							bool flag4 = NodeTitleTypeSetting.FileNodeTitleType == ENFileNodeTitleType.SerialFileName;
							if (flag4)
							{
								bool flag5 = this.FileNO != string.Empty;
								if (flag5)
								{
									result = this.FileNO.ToString() + "-" + this.FileName;
								}
								else
								{
									result = this.FileName;
								}
							}
							else
							{
								result = this.FileName;
							}
						}
					}
				}
				return result;
			}
		}

		[Category("文件信息"), DisplayName("本地路径")]
		public string LocalPath
		{
			get;
			set;
		}

		[Browsable(false)]
		public bool Editable
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("操作类型")]
		public EOperType Operation
		{
			get;
			set;
		}

		[Browsable(false)]
		public string FileURL
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("编号")]
		public string FileNO
		{
			get;
			set;
		}

		[Browsable(false), Category("文件信息"), DisplayName("文件内容数据[可选]")]
		public byte[] Data
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("归类")]
		public string Category
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("发票编号")]
		public string ExFaPiaoCode
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("文件名")]
		public string FileName
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("文件MD5")]
		public string FileMD5
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("文件大小")]
		public int FileSize
		{
			get;
			set;
		}

		[Category("文件信息"), DisplayName("所属批次")]
		public string BatchNO
		{
			get;
			set;
		}

		public int ExShenheResult
		{
			get;
			set;
		}

		public string ExShenheRemark
		{
			get;
			set;
		}

		[Browsable(false)]
		public NFileInfo OrigData
		{
			get;
			set;
		}

		public void OnDataChanged()
		{
			bool flag = this.DataChanged != null;
			if (flag)
			{
				this.DataChanged(this, EventArgs.Empty);
			}
		}

		public static NFileInfo FromLocalFile(string fpath, bool IncludeFileContent = true)
		{
			NFileInfo nFileInfo = new NFileInfo();
			nFileInfo.FileName = FileHelper.GetFileName(fpath);
			nFileInfo.FileMD5 = MD5Helper.GetFileMD5(fpath);
			nFileInfo.FileSize = (int)new FileInfo(fpath).Length;
			if (IncludeFileContent)
			{
				nFileInfo.Data = File.ReadAllBytes(fpath);
			}
			return nFileInfo;
		}

		public void AttatchFileData(string fpath)
		{
			bool flag = !File.Exists(fpath);
			if (flag)
			{
				throw new Exception("文件不存在" + fpath);
			}
			this.LocalPath = fpath;
			this.FileName = FileHelper.GetFileName(fpath);
			this.FileMD5 = MD5Helper.GetFileMD5(fpath);
			this.FileSize = (int)new FileInfo(fpath).Length;
			this.Data = File.ReadAllBytes(fpath);
		}

		public string GetFileExt()
		{
			return FileHelper.GetFileExtNoIncDot(this.FileName);
		}

		public void SetDate(DateTime dt)
		{
            //this.CreateDate = tm.Year * 10000 + tm.Month * 100 + tm.Day;
            this.CreateTime = dt.ToString(ConstString.DateFormat);
		}

		public static NFileInfo FromPBMsg(MsgFileInfo input)
		{
			NFileInfo nFileInfo = new NFileInfo();
			nFileInfo.Author = input.Author1;
			nFileInfo.Version = input.Version2;
			//nFileInfo.CreateDate = input.CreateDate3;
			nFileInfo.CreateTime = input.CreateTime4;
			nFileInfo.Remark = input.Remark5;
			nFileInfo.FileName = input.FileName6;
			nFileInfo.FileURL = input.FileURL7;
			nFileInfo.FileNO = input.FileNO8;
			nFileInfo.FileMD5 = input.FileMD59;
			nFileInfo.FileSize = input.FileSize10;
			bool flag = input.Data11 != null && input.Data11 != ByteString.Empty;
			if (flag)
			{
				nFileInfo.Data = input.Data11.ToByteArray();
			}
			nFileInfo.Operation = input.Operation12;
			nFileInfo.BatchNO = input.BatchNO13;
			nFileInfo.Category = input.Category14;
			nFileInfo.NotesList.Clear();
			bool flag2 = input.Notes15List != null;
			if (flag2)
			{
				foreach (MsgNoteInfo current in input.Notes15List)
				{
					nFileInfo.NotesList.Add(NNoteInfo.FromPBMsg(current));
				}
			}
			nFileInfo.ExFaPiaoCode = input.ExFaPiaoCode16;
			nFileInfo.Editable = input.Editable18;
			nFileInfo.ExShenheResult = input.ExShenheResult19;
			nFileInfo.ExShenheRemark = input.ExShenheRemark20;
			nFileInfo.LocalPath = input.LocalPath21;
			return nFileInfo;
		}

		public NFileInfo()
		{
			this.Author = "NoneAuthor";
			this.Version = 1;
            //this.CreateDate = DateTime.Now.ToYMD();
            this.CreateTime = DateTime.Now.ToString(ConstString.DateFormat);
			this.Remark = "";
			this.FileName = "";
			this.FileURL = "";
			this.FileNO = "";
			this.FileMD5 = "";
			this.FileSize = 0;
			this.Operation = EOperType.eADD;
			this.BatchNO = "";
			this.Category = "";
			this.ExFaPiaoCode = "";
			this.Editable = true;
		}

		public MsgFileInfo ToPBMsg(bool includefiledata = true)
		{
			MsgFileInfo.Builder builder = new MsgFileInfo.Builder();
			builder.Author1 = this.Author;
			builder.Version2 = this.Version;
			//builder.CreateDate3 = this.CreateDate;
			builder.CreateTime4 = this.CreateTime;
			builder.Remark5 = this.Remark;
			builder.FileName6 = this.FileName;
			builder.FileURL7 = this.FileURL;
			builder.FileNO8 = this.FileNO;
			builder.FileMD59 = this.FileMD5;
			builder.FileSize10 = this.FileSize;
			bool flag = includefiledata && (this.Operation == EOperType.eADD || this.Operation == EOperType.eUPD);
			if (flag)
			{
				this.AttatchFileData(this.LocalPath);
				bool flag2 = this.Data != null;
				if (flag2)
				{
					builder.Data11 = ByteString.CopyFrom(this.Data);
				}
			}
			else
			{
				this.Data = null;
				builder.Data11 = ByteString.Empty;
			}
			builder.Operation12 = this.Operation;
			builder.BatchNO13 = this.BatchNO;
			builder.Category14 = this.Category;
			builder.Notes15List.Clear();
			foreach (NNoteInfo current in this.NotesList)
			{
				builder.Notes15List.Add(current.ToMsgNoteInfo());
			}
			builder.ExFaPiaoCode16 = this.ExFaPiaoCode;
			builder.Editable18 = this.Editable;
			builder.LocalPath21 = this.LocalPath;
			return builder.BuildParsed();
		}

		public bool HasExShenheInfo()
		{
			return this.ExShenheResult != 0;
		}

		public void ClearShenheInfo()
		{
			this.ExShenheResult = 0;
			this.ExShenheRemark = string.Empty;
		}

		public string ToUITipString()
		{
			return this.FileName + Environment.NewLine + this.Author;
		}

		public string GetShortFileName()
		{
			return FileHelper.GetFileName(this.FileName);
		}

		public bool ExPortDataToFile(string fname = "")
		{
			bool flag = string.IsNullOrEmpty(fname);
			if (flag)
			{
				fname = this.FileName;
			}
			bool flag2 = this.Data != null && this.Data.Length != 0;
			if (flag2)
			{
				File.WriteAllBytes(fname, this.Data);
			}
			return true;
		}

		public void SetCurrentTime()
		{
            this.CreateTime = DateTime.Now.ToString(ConstString.DateFormat);
		}

		public void ToPBFile(string fname, bool includefiledata = true)
		{
			MsgFileInfo msgFileInfo = this.ToPBMsg(includefiledata);
			File.WriteAllBytes(fname, msgFileInfo.ToByteArray());
		}

		public DateTime GetCreateTime()
		{
			//DateTime result = new DateTime(this.CreateDate / 10000, this.CreateDate / 100 % 100, this.CreateDate % 100, this.CreateTime / 10000, this.CreateTime / 100 % 100, this.CreateTime % 100);
			return Convert.ToDateTime(this.CreateTime);
		}

		public NFileInfo MyClone()
		{
			NFileInfo nFileInfo = new NFileInfo();
			nFileInfo.Author = this.Author;
			nFileInfo.Version = this.Version;
			//nFileInfo.CreateDate = this.CreateDate;
			nFileInfo.CreateTime = this.CreateTime;
			nFileInfo.Remark = this.Remark;
			nFileInfo.FileName = this.FileName;
			nFileInfo.FileURL = this.FileURL;
			nFileInfo.FileNO = this.FileNO;
			nFileInfo.FileMD5 = this.FileMD5;
			nFileInfo.LocalPath = this.LocalPath;
			nFileInfo.FileSize = this.FileSize;
			bool flag = this.Data != null;
			if (flag)
			{
				byte[] array = new byte[this.Data.Length];
				Array.Copy(this.Data, array, this.Data.Length);
				nFileInfo.Data = array;
			}
			nFileInfo.Operation = this.Operation;
			nFileInfo.BatchNO = this.BatchNO;
			nFileInfo.Category = this.Category;
			nFileInfo.NotesList.Clear();
			bool flag2 = this.NotesList != null;
			if (flag2)
			{
				foreach (NNoteInfo current in this.NotesList)
				{
					nFileInfo.NotesList.Add(current.MyClone());
				}
			}
			nFileInfo.ExFaPiaoCode = this.ExFaPiaoCode;
			nFileInfo.Editable = this.Editable;
			nFileInfo.ExShenheResult = this.ExShenheResult;
			nFileInfo.ExShenheRemark = this.ExShenheRemark;
			nFileInfo.OrigData = this;
			return nFileInfo;
		}

		public bool MyEqual(NFileInfo right)
		{
			bool flag = right == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = right.Author == this.Author && right.Remark == this.Remark && right.FileName == this.FileName && right.FileMD5 == this.FileMD5 && right.FileSize == this.FileSize && right.Category == this.Category && right.ExFaPiaoCode == this.ExFaPiaoCode;
				bool flag3 = right.NotesList.Count != this.NotesList.Count;
				if (flag3)
				{
					flag2 = false;
				}
				for (int i = 0; i < right.NotesList.Count; i++)
				{
					bool flag4 = !right.NotesList[i].MyEqual(this.NotesList[i]);
					if (flag4)
					{
						result = false;
						return result;
					}
				}
				result = flag2;
			}
			return result;
		}

		public NFileInfo FromPBMsg(object obj)
		{
			return NFileInfo.FromPBMsg(obj as MsgFileInfo);
		}
	}
}
