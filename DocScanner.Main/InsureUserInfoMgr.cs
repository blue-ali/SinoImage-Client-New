using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    public class InsureUserInfoMgr
    {
        public List<InsureUserInfo> _users;

        private static InsureUserInfoMgr _instance;

        private static string sefname = "_insureinfo.dat";

        public static InsureUserInfoMgr Instance
        {
            get
            {
                bool flag = InsureUserInfoMgr._instance == null;
                if (flag)
                {
                    InsureUserInfoMgr._instance = new InsureUserInfoMgr();
                    InsureUserInfoMgr._instance.Load();
                }
                return InsureUserInfoMgr._instance;
            }
        }

        public List<InsureUserInfo> AllUsers
        {
            get
            {
                return this._users;
            }
        }

        public List<InsureUserInfo> FindUserByName(string Name)
        {
            bool flag = Name == "*";
            List<InsureUserInfo> result;
            if (flag)
            {
                result = this._users;
            }
            else
            {
                List<InsureUserInfo> list = this._users.FindAll((InsureUserInfo o) => o.Name == Name).ToList<InsureUserInfo>();
                result = list;
            }
            return result;
        }

        public List<InsureUserInfo> FindUserByIDNO(string idno)
        {
            return this._users.FindAll((InsureUserInfo o) => o.IDNO == idno).ToList<InsureUserInfo>();
        }

        public bool AddUser(InsureUserInfo info)
        {
            this._users.Add(info);
            this.Save();
            return true;
        }

        public bool Save()
        {
            SerializeHelper.SerializeToXML<List<InsureUserInfo>>(this._users, InsureUserInfoMgr.sefname);
            return true;
        }

        public bool Load()
        {
            bool flag = File.Exists(InsureUserInfoMgr.sefname);
            if (flag)
            {
                this._users = SerializeHelper.DeSerializeFromXML<List<InsureUserInfo>>(InsureUserInfoMgr.sefname);
            }
            else
            {
                this._users = new List<InsureUserInfo>();
            }
            return true;
        }
    }
}
