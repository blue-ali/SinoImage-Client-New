using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DocScanner.LibCommon;
using DocScaner.Common;
using DocScanner.Bean.pb;

namespace DocScanner.Bean
{
    public class NBatchInfo : IMyOpe<NBatchInfo>
	{
		
		private List<NFileInfo> _fileinfos = new List<NFileInfo>();

		public const string PBFileExt = ".pbope";

		public const string PBDataExt = ".pbdata";

		[method: CompilerGenerated]
		//[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
		public event EventHandler DataChanged;

		[Category("批次信息"), DisplayName("作者")]
		public string Author
		{
			get;
			set;
		}

		[Browsable(false)]
		public bool IsFromWeb
		{
			get;
			set;
		}

		[Browsable(false)]
		public string DisplayName
		{
			get
			{
				return this.BatchNO;
			}
		}

		[Browsable(false)]
		public bool Changed
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("版本")]
		public int Version
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("创建日期")]
		public int CreateDate
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("创建时间")]
		public int CreateTime
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("备注")]
		public string Remark
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("标题")]
		public string Title
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("批次编号")]
		public string BatchNO
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("文件数目")]
		public int FileCount
		{
			get
			{
				return (this._fileinfos != null) ? this._fileinfos.Count : 0;
			}
		}

		[Category("批次信息"), DisplayName("操作")]
		public EOperType Operation
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("文件列表")]
		public List<NFileInfo> FileinfosList
		{
			get
			{
				return this._fileinfos;
			}
			set
			{
				this._fileinfos = value;
			}
		}

		[Category("批次信息"), DisplayName("机构号")]
		public string OrgID
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("业务编号")]
		public string BusiSysId
		{
			get;
			set;
		}

		[Browsable(false), Category("批次信息"), DisplayName("登录帐号密码")]
		public string Password
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("业务类型")]
		public string BusiTypeId
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("操作员")]
		public string TellerNO
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("机器信息")]
		public string MachineID
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("条形码")]
		public string BarCode
		{
			get;
			set;
		}

		[Category("批次信息"), DisplayName("来源IP")]
		public string SourceIP
		{
			get;
			set;
		}

		[Browsable(false)]
		public NResultInfo ResultInfo
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
		public NBatchInfo OrigData
		{
			get;
			set;
		}

		public void SetCurrentTime()
		{
			int createDate;
			int createTime;
			TimeHelper.SetCurrentTime(out createDate, out createTime);
			this.CreateDate = createDate;
			this.CreateTime = createTime;
		}

		public void ExtractFileData(string Rootdir)
		{
			foreach (NFileInfo current in this.FileinfosList)
			{
				string fileName = FileHelper.GetFileName(current.FileName);
				current.LocalPath = Path.Combine(Rootdir, fileName);
				current.ExPortDataToFile(current.LocalPath);
			}
		}

		public bool HasError()
		{
			return this.ResultInfo != null && this.ResultInfo.Status > EResultStatus.eSuccess;
		}

		public bool HasExShenheInfo()
		{
			bool flag = this.ExShenheResult != 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				foreach (NFileInfo current in this.FileinfosList)
				{
					bool flag2 = current.ExShenheResult != 0;
					if (flag2)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		[Category("文件信息"), DisplayName("文件大小")]
		public void UpadateAllDate(DateTime tm)
		{
			this.CreateDate = tm.Year * 10000 + tm.Month * 100 + tm.Day;
			this.CreateTime = tm.Hour * 10000 + tm.Minute * 100 + tm.Second;
			foreach (NFileInfo current in this.FileinfosList)
			{
				current.SetDate(tm);
				foreach (NNoteInfo current2 in current.NotesList)
				{
					current2.SetDateTime(tm);
				}
			}
		}

		public MsgBatchInfo ToPBMsg(bool includefiledata = true)
		{
			MsgBatchInfo.Builder builder = new MsgBatchInfo.Builder();
			builder.Author1 = this.Author;
			builder.Version2 = this.Version;
			builder.CreateDate3 = this.CreateDate;
			builder.CreateTime4 = this.CreateTime;
			builder.Remark5 = this.Remark;
			builder.BatchNO6 = this.BatchNO;
			builder.Title7 = this.Title;
			builder.Operation8 = this.Operation;
			builder.Fileinfos9List.Clear();
			foreach (NFileInfo current in this.FileinfosList)
			{
				builder.Fileinfos9List.Add(current.ToPBMsg(includefiledata));
			}
			builder.OrgID10 = this.OrgID;
			builder.BusiSysId11 = this.BusiSysId;
			builder.BusiTypeId12 = this.BusiTypeId;
			builder.BarCode13 = this.BarCode;
			builder.SourceIP14 = this.SourceIP;
			builder.MachineID15 = this.MachineID;
			builder.Password16 = this.Password;
			bool flag = this.ResultInfo != null;
			if (flag)
			{
				builder.ResultInfo17 = this.ResultInfo.ToPBMsg();
			}
			builder.Editable18 = this.Editable;
			return builder.BuildParsed();
		}

		public NBatchInfo()
		{
			this.Author = "NoneAuthor";
			this.Version = 1;
			this.CreateDate = DateTime.Now.ToYMD();
			this.CreateTime = DateTime.Now.ToHMS();
			this.Remark = "";
			this.Title = "";
			this.BatchNO = "";
			this.Operation = EOperType.eADD;
			this.OrgID = "NoneOrgID";
			this.BusiSysId = "";
			this.BusiTypeId = "";
			this.TellerNO = "";
			this.BarCode = "";
			this.SourceIP = "";
			this.MachineID = "";
			this.Password = "";
			this.Editable = true;
		}

		public static NBatchInfo FromPBMsg(MsgBatchInfo input)
		{
			NBatchInfo nBatchInfo = new NBatchInfo();
			nBatchInfo.Author = input.Author1;
			nBatchInfo.Version = input.Version2;
			nBatchInfo.CreateDate = input.CreateDate3;
			nBatchInfo.CreateTime = input.CreateTime4;
			nBatchInfo.Remark = input.Remark5;
			nBatchInfo.BatchNO = input.BatchNO6;
			nBatchInfo.Title = input.Title7;
			nBatchInfo.Operation = input.Operation8;
            //FileinfosList = input.Fileinfos9List.Select<MsgFileInfo, NFileInfo>((<> c.<> 9__106_0 ?? (<> c.<> 9__106_0 = new Func<MsgFileInfo, NFileInfo>(<> c.<> 9.< FromPBMsg > b__106_0)))).ToList<NFileInfo>(),
            nBatchInfo.FileinfosList = input.Fileinfos9List.Select<MsgFileInfo, NFileInfo>(o => NFileInfo.FromPBMsg(o)).ToList<NFileInfo>();
            nBatchInfo.OrgID = input.OrgID10;
			nBatchInfo.BusiSysId = input.BusiSysId11;
			nBatchInfo.BusiTypeId = input.BusiTypeId12;
			nBatchInfo.BarCode = input.BarCode13;
			nBatchInfo.SourceIP = input.SourceIP14;
			nBatchInfo.MachineID = input.MachineID15;
			nBatchInfo.Password = input.Password16;
			bool flag = input.ResultInfo17 != null;
			if (flag)
			{
				nBatchInfo.ResultInfo = NResultInfo.FromNetMsg(input.ResultInfo17);
			}
			nBatchInfo.Editable = input.Editable18;
			nBatchInfo.ExShenheResult = input.ExShenheResult19;
			nBatchInfo.ExShenheRemark = input.ExShenheRemark20;
			return nBatchInfo;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.BatchNO);
			return stringBuilder.ToString();
		}

		public static NBatchInfo FromPBFile(string fname)
		{
			MsgBatchInfo input = MsgBatchInfo.ParseFrom(File.ReadAllBytes(fname));
			return NBatchInfo.FromPBMsg(input);
		}

		public void ToPBFile(string fname, bool includefiledata = true)
		{
			MsgBatchInfo msgBatchInfo = this.ToPBMsg(includefiledata);
			File.WriteAllBytes(fname, msgBatchInfo.ToByteArray());
		}

		public void ClearShenHeInfos()
		{
			this.ExShenheResult = 0;
			this.ExShenheRemark = string.Empty;
			foreach (NFileInfo current in this.FileinfosList)
			{
				current.ExShenheResult = 0;
				current.ExShenheRemark = string.Empty;
			}
		}

		public NBatchInfo MyClone()
		{
			NBatchInfo nBatchInfo = new NBatchInfo();
			nBatchInfo.Author = this.Author;
			nBatchInfo.Version = this.Version;
			nBatchInfo.CreateDate = this.CreateDate;
			nBatchInfo.CreateTime = this.CreateTime;
			nBatchInfo.Remark = this.Remark;
			nBatchInfo.BatchNO = this.BatchNO;
			nBatchInfo.Title = this.Title;
			nBatchInfo.Operation = this.Operation;
			nBatchInfo.FileinfosList.Clear();
			foreach (NFileInfo current in this.FileinfosList)
			{
				nBatchInfo.FileinfosList.Add(current.MyClone());
			}
			nBatchInfo.OrgID = this.OrgID;
			nBatchInfo.BusiSysId = this.BusiSysId;
			nBatchInfo.BusiTypeId = this.BusiTypeId;
			nBatchInfo.BarCode = this.BarCode;
			nBatchInfo.SourceIP = this.SourceIP;
			nBatchInfo.MachineID = this.MachineID;
			nBatchInfo.Password = this.Password;
			nBatchInfo.Editable = this.Editable;
			nBatchInfo.OrigData = this;
			return nBatchInfo;
		}

		public bool MyEqual(NBatchInfo input)
		{
			bool flag = input == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.Author == input.Author && this.OrgID == input.OrgID && this.BarCode == input.BarCode;
				bool flag3 = !flag2;
				if (flag3)
				{
					result = flag2;
				}
				else
				{
					bool flag4 = this.FileinfosList.Count != input.FileinfosList.Count;
					if (flag4)
					{
						result = false;
					}
					else
					{
						for (int i = 0; i < this.FileinfosList.Count; i++)
						{
							bool flag5 = !this.FileinfosList[i].MyEqual(input.FileinfosList[i]);
							if (flag5)
							{
								result = false;
								return result;
							}
						}
						result = true;
					}
				}
			}
			return result;
		}

		public void ClearShenheInfo()
		{
			this.ExShenheResult = 0;
			this.ExShenheRemark = string.Empty;
			foreach (NFileInfo current in this.FileinfosList)
			{
				current.ExShenheResult = 0;
				current.ExShenheRemark = string.Empty;
			}
		}

		public NBatchInfo FromPBMsg(object obj)
		{
			return NBatchInfo.FromPBMsg(obj as MsgBatchInfo);
		}
	}
}

