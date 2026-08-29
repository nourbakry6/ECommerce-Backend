using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }
}
