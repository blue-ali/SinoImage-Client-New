using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using System;
using System.ComponentModel;
using System.Drawing;

namespace DocScanner.Bean
{
    public class NNoteInfo : IMyOpe<NNoteInfo>
	{
		public event EventHandler DataChanged;

		[Category("批注信息"), DisplayName("作者")]
		public string Author
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("版本")]
		public int Version
		{
			get;
			set;
		}

		//[Category("批注信息"), DisplayName("创建日期")]
		//public int CreateDate
		//{
		//	get;
		//	set;
		//}

		[Category("批注信息"), DisplayName("创建时间")]
		public string CreateTime
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("备注")]
		public string Remark
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("关联文件的MD5码")]
		public string FileMD5Link
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("批注内容")]
		public string NoteMsg
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("名称")]
		public string NoteName
		{
			get;
			set;
		}

		public string FileLink
		{
			get;
			set;
		}

		public EOperType Operation
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("位置X")]
		public int RegionX
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("位置Y")]
		public int RegionY
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("宽度")]
		public int RegionWidth
		{
			get;
			set;
		}

		[Category("批注信息"), DisplayName("高度")]
		public int RegionHeight
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

		[Browsable(false)]
		public NNoteInfo OrigData
		{
			get;
			set;
		}

		public Rectangle GetRegion()
		{
			return new Rectangle
			{
				X = this.RegionY,
				Y = this.RegionY,
				Width = this.RegionWidth,
				Height = this.RegionHeight
			};
		}

		public void SetRegion(Rectangle rect)
		{
			this.RegionX = rect.X;
			this.RegionY = rect.Y;
			this.RegionWidth = rect.Width;
			this.RegionHeight = rect.Height;
		}

		public NNoteInfo()
		{
			this.Author = "";
			this.Version = 1;
            //this.CreateDate = DateTime.Now.ToYMD();
            this.CreateTime = DateTime.Now.ToString(ConstString.DateFormat);
			this.Remark = "";
			this.NoteName = "";
			this.NoteMsg = "";
			this.FileLink = "";
			this.FileMD5Link = "";
			this.Operation = EOperType.eADD;
			this.RegionX = 0;
			this.RegionY = 0;
			this.RegionWidth = 0;
			this.RegionHeight = 0;
			this.Editable = true;
		}

		public override string ToString()
		{
			return this.NoteMsg;
		}

		public DateTime GetCreateTime()
		{
			//DateTime result = new DateTime(this.CreateDate / 10000, this.CreateDate / 100 % 100, this.CreateDate % 100, this.CreateTime / 10000, this.CreateTime / 100 % 100, this.CreateTime % 100);
			return Convert.ToDateTime(this.CreateTime);
		}

		public void SetDateTime(DateTime dt)
		{
            this.CreateTime = dt.ToString(ConstString.DateFormat);
		}

		public NNoteInfo MyClone()
		{
			return new NNoteInfo
			{
				Author = this.Author,
				Version = this.Version,
				//CreateDate = this.CreateDate,
				CreateTime = this.CreateTime,
				Remark = this.Remark,
				NoteName = this.NoteName,
				NoteMsg = this.NoteMsg,
				FileLink = this.FileLink,
				FileMD5Link = this.FileMD5Link,
				Operation = this.Operation,
				RegionX = this.RegionX,
				RegionY = this.RegionY,
				RegionWidth = this.RegionWidth,
				RegionHeight = this.RegionHeight,
				Editable = this.Editable,
				OrigData = this
			};
		}

		public bool MyEqual(NNoteInfo right)
		{
			bool flag = right == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.Remark == right.Remark && this.NoteName == right.NoteName && this.NoteMsg == right.NoteMsg && this.RegionX == right.RegionX && this.RegionY == right.RegionY && this.RegionWidth == right.RegionWidth && this.RegionHeight == right.RegionHeight;
				result = flag2;
			}
			return result;
		}

		public NNoteInfo FromPBMsg(object obj)
		{
			return this.FromPBMsg(obj as NNoteInfo);
		}

		public static NNoteInfo FromPBMsg(MsgNoteInfo input)
		{
			return new NNoteInfo
			{
				Author = input.Author1,
				Version = input.Version2,
				//CreateDate = input.CreateDate3,
				CreateTime = input.CreateTime4,
				Remark = input.Remark5,
				NoteName = input.NoteName6,
				NoteMsg = input.NoteMsg7,
				FileLink = input.FileLink8,
				FileMD5Link = input.FileMD5Link9,
				Operation = input.Operation10,
				RegionX = input.RegionX11,
				RegionY = input.RegionY12,
				RegionWidth = input.RegionWidth13,
				RegionHeight = input.RegionHeight14,
				Editable = input.Editable15
			};
		}

		public MsgNoteInfo ToMsgNoteInfo()
		{
			return new MsgNoteInfo.Builder
			{
				Author1 = this.Author,
				Version2 = this.Version,
				//CreateDate3 = this.CreateDate,
				CreateTime4 = this.CreateTime,
				Remark5 = this.Remark,
				NoteName6 = this.NoteName,
				NoteMsg7 = this.NoteMsg,
				FileLink8 = this.FileLink,
				FileMD5Link9 = this.FileMD5Link,
				Operation10 = this.Operation,
				RegionX11 = this.RegionX,
				RegionY12 = this.RegionY,
				RegionWidth13 = this.RegionWidth,
				RegionHeight14 = this.RegionHeight
			}.BuildParsed();
		}

		public MsgNoteInfo ToPBMsg()
		{
			return new MsgNoteInfo.Builder
			{
				Author1 = this.Author,
				Version2 = this.Version,
				//CreateDate3 = this.CreateDate,
				CreateTime4 = this.CreateTime,
				Remark5 = this.Remark,
				NoteName6 = this.NoteName,
				NoteMsg7 = this.NoteMsg,
				FileLink8 = this.FileLink,
				FileMD5Link9 = this.FileMD5Link,
				Operation10 = this.Operation,
				RegionX11 = this.RegionX,
				RegionY12 = this.RegionY,
				RegionWidth13 = this.RegionWidth,
				RegionHeight14 = this.RegionHeight,
				Editable15 = this.Editable
			}.BuildParsed();
		}

		public void SetCurrentTime()
		{
            this.CreateTime = DateTime.Now.ToString(ConstString.DateFormat);
		}
	}
}
