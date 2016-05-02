using DocScanner.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    internal interface IQueryBatchNO
    {
        event EventHandler<TEventArg<string>> OnGetBatchNO;

        string GetBatchNO();
    }
}
