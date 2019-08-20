using System;
using System.Collections.Generic;
using System.Text;

namespace Blc.Interfaces
{
    //public abstract class BusinessProcessStep<TResult>
    //{
    //    public abstract TResult Run();

    //    public abstract void Cancel();
    //}

    public interface IBusinessProcessStep<TResult>
    {
        TResult Run();

        void Cancel();
    }
}
