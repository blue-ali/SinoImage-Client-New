using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public abstract class AbstractSetting<T> : IEquatable<T> where T : new()
    {
        // Fields
        private static T _cur;

        // Methods

        private bool Equal(object right)
        {
            return false;
        }

        public abstract bool Equals(T other);
        private object MyClone()
        {
            return base.MemberwiseClone();
        }

        // Properties
        [Browsable(false)]
        public static T CurSetting
        {
            get
            {
                if (AbstractSetting<T>._cur == null)
                {
                    AbstractSetting<T>._cur = Activator.CreateInstance<T>();
                }
                return AbstractSetting<T>._cur;
            }
        }

        [Browsable(false)]
        public abstract string Name { get; }
    }



}
