using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.CQRS.Commands;

namespace UXI.CQRS
{
    public class CommandDispatcher
    {
        private readonly ICommandHandlerResolver _provider;

        public CommandDispatcher(ICommandHandlerResolver provider)
        {
            _provider = provider;
        }

        public void Dispatch<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            var handler = _provider.Resolve<TCommand>();
            handler.Handle(command);
        }
    }
}
