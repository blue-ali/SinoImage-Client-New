using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DocScanner.LibCommon
{
    public interface IStructuralEquatable
    {
        bool Equals(object other, IEqualityComparer comparer);

        int GetHashCode(IEqualityComparer comparer);
    }
}