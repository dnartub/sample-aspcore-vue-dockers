using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blc.Interfaces
{
    //public abstract class BusinessProcessStep<TResult>
    //{
    //    public abstract TResult Run();

    //    public abstract void Cancel();
    //}

    public interface IBusinessProcessStep<TResult>
    {
        Task<TResult> RunAsync();

        Task CancelAsync();
    }
}
