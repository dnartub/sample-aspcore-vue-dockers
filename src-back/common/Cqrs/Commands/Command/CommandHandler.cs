using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Commands.Command
{
    abstract class CommandHandler : ICommandHandler<Command>
    {
        public Task Down(Command command)
        {
            throw new NotImplementedException();
        }

        public Task Execute(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
