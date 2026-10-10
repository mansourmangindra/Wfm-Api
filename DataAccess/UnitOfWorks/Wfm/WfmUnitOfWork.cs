using DataAccess.DBContexts.Wfm;
using DataAccess.DBContexts.Wfm.Models;
using DataAccess.Repositories.Wfm.Schedules;
using DataAccess.Repositories.Wfm.Schedules.Interfaces;
using DataAccess.Repositories.Wfm.StaffingRequirements;
using DataAccess.Repositories.Wfm.StaffingRequirements.Interfaces;
using DataAccess.Repositories.Wfm.Users;
using DataAccess.Repositories.Wfm.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.UnitOfWorks.Wfm
{
    public class WfmUnitOfWork : IWfmUnitOfWork
    {
        private readonly WfmDbContext _context;
        
        public WfmUnitOfWork(WfmDbContext context)
        {
            _context = context;

            UserRepository = new UserRepository(_context);
            SchedulesRepository = new SchedulesRepository(_context);
            StaffingRequirementsRepository = new StaffingRequirementsRepository(_context);
        }

        public IUserRepository UserRepository { get; set; }
        public ISchedulesRepository SchedulesRepository { get; set; }
        public IStaffingRequirementsRepository StaffingRequirementsRepository { get; set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            int result = await _context.SaveChangesAsync();

            return result;
        }
    }
}   
