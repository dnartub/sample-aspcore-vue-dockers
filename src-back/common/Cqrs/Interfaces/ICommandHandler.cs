using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Interfaces
{
    public interface ICommandHandler<T> where T: ICommand
    {
        Task Execute(T command);
        Task Down(T command);
    }
}
