using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blc.Interfaces
{
    public interface IBusinessProcessStep<TResult>
    {
        Task<TResult> RunAsync();
    }

    public interface IBusinessProcessStepCancelable
    {
        Task CancelAsync();
    }
}
