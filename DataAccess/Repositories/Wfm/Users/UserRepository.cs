using DataAccess.DBContexts.Wfm;
using DataAccess.DBContexts.Wfm.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Wfm.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Wfm.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly WfmDbContext _context;
        public UserRepository(WfmDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
