using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Commands.Command
{
    abstract class CommandHandler : ICommandHandler<Command>
    {
        public void Execute(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
