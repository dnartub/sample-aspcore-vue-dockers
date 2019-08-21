using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blc.Interfaces
{
    public interface IBusinessProcess<TReqest, TResult>
    {
        Task<TResult> RunAsync(TReqest request);
    }
}
