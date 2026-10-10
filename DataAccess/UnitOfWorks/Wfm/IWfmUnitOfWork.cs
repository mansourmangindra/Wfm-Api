using DataAccess.Repositories.Wfm.Schedules.Interfaces;
using DataAccess.Repositories.Wfm.StaffingRequirements.Interfaces;
using DataAccess.Repositories.Wfm.Users.Interfaces;
using DataAccess.UnitOfWorks._Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.UnitOfWorks.Wfm
{
    public interface IWfmUnitOfWork : IBaseUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public IStaffingRequirementsRepository StaffingRequirementsRepository { get; set; }
        public ISchedulesRepository SchedulesRepository { get; set; }
    }
}
