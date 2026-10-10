using DataAccess.DBContexts.Wfm;
using DataAccess.DBContexts.Wfm.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Wfm.StaffingRequirements.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Wfm.StaffingRequirements
{
    public class StaffingRequirementsRepository : BaseRepository<StaffingRequirement>, IStaffingRequirementsRepository
    {
        private readonly WfmDbContext _context;
        public StaffingRequirementsRepository(WfmDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
