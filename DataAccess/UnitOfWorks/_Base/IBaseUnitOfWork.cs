using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.UnitOfWorks._Base
{
    public interface IBaseUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
