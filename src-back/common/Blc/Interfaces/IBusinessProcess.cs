using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blc.Interfaces
{
    //public abstract class BusinessProcess<TReqest, TResult>
    //{
    //    public abstract TResult Run(TReqest request);
    //}

    public interface IBusinessProcess<TReqest, TResult>
    {
        TResult Run(TReqest request);
    }
}
