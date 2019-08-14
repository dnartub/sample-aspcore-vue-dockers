using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs
{
    public interface IServiceCommand
    {
        T Use<T>(T command) where T: IAnyGetCommand;
        IExecuteCommand Use(IExecuteCommand command);
    }
}
