using DocScanner.LibCommon;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

//
namespace DocScanner.LibCommon
{
    /// <summary>
    /// 对象的状态参数等，以前都是以static方式定义，但在浏览器里，对象的实例个数会多个的（并且对象所在进程、线程的信息也难以判断).
    /// </summary>

    //public class AppContext : IDisposable
    public class AppContext
    {
        [ThreadStatic]
        private static readonly AppContext instanc = new AppContext();
        private ConcurrentDictionary<object, object> _data = new ConcurrentDictionary<object, object>();
        private bool _disposed;

        // Methods
        private AppContext()
        {
            this._data[typeof(IniConfigSetting)] = IniConfigSetting.Cur;
            MessageService service = new MessageService();
            this._data[typeof(MessageService)] = service;
        }

        //public void Dispose()
        //{
        //    this.Dispose(true);
        //}

        //private void Dispose(bool disposing)
        //{
        //    if (!this._disposed)
        //    {
        //        if (disposing)
        //        {
        //            if ((this._data != null) && (this._data.Count > 0))
        //            {
        //                foreach (KeyValuePair<object, object> pair in this._data)
        //                {
        //                    IDisposable disposable = pair.Value as IDisposable;
        //                    if (disposable != null)
        //                    {
        //                        disposable.Dispose();
        //                    }
        //                }
        //            }
        //            this._data.Clear();
        //        }
        //        instance = null;
        //        this._disposed = true;
        //    }
        //}

        //~AppContext()
        //{
        //    this.Dispose(false);
        //}

        public T GetVal<T>(object key) where T : class
        {
            if (!this._data.ContainsKey(key))
            {
                return default(T);
            }
            return (this._data[key] as T);
        }

        public void SetVal(object key, object val)
        {
            this._data[key] = val;
        }

        // Properties
        public IniConfigSetting Config
        {
            get
            {
                return this.GetVal<IniConfigSetting>(typeof(IniConfigSetting));
            }
        }

        public static AppContext GetInstance()
        {
            return instanc;
        }


        public MessageService MS
        {
            get
            {
                return this.GetVal<MessageService>(typeof(MessageService));
            }
        }

    }
}