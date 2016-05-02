using DocScanner.Bean.pb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DocScanner.Bean
{
	public class NCategoryInfo
	{

        [Category("分类信息"), DisplayName("名称")]
		public string CategoryName
		{
            get;
            set;
		}

        [Category("分类信息"), DisplayName("批次号")]
        public string BatchNo
        {
            get;
            set;
        }

        [Category("分类信息"), DisplayName("批次号")]
        public int CategoryLevel
        {
            get;
            set;
        }

        public NCategoryInfo(string CategoryName)
		{
            this.CategoryName = CategoryName;

        }

        public NCategoryInfo()
        {
        }
      
    }
}
