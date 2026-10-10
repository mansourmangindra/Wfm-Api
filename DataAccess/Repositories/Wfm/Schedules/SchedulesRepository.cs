using DataAccess.DBContexts.Wfm;
using DataAccess.DBContexts.Wfm.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Wfm.Schedules.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Wfm.Schedules
{
    public class SchedulesRepository : BaseRepository<Schedule>, ISchedulesRepository
    {
        private readonly WfmDbContext _context;
        public SchedulesRepository(WfmDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
