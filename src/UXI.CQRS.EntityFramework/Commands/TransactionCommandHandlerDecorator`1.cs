using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.CQRS.Commands
{
    public class TransactionCommandHandlerDecorator<TCommand, TDbContext>
        : ICommandHandler<TCommand>
        where TCommand : ICommand
        where TDbContext : IDbContext
    {
        private readonly TDbContext _context;
        private readonly ICommandHandler<TCommand> _decorated;

        public TransactionCommandHandlerDecorator(
           TDbContext context,
           ICommandHandler<TCommand> decorated)
        {
           _context = context;
           _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            _decorated.Handle(command);

            _context.SaveChanges();
        }
    }
}
