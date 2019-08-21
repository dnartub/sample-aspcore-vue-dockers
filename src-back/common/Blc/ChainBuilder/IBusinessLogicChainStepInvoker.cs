using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blc.ChainBuilder
{
    internal interface IBusinessLogicChainStepInvoker
    {
        IBusinessLogicChainStepInvoker PreviosBusinessLogicChainStep { get; set; }

        object RunBusinessProcessStep(IServiceProvider provider, object previosResult);
        void CancelBusinessProcessStep();
        Task<object> RunBusinessLogicChainOnException(Exception cathcedException, object previosResult, IServiceProvider provider);
        Task<object> RunBusinessLogicChainOnResult(object previosResult, IServiceProvider provider);
        Task<object> RunBusinessLogicChain(IServiceProvider provider);
    }
}
