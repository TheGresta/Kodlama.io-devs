﻿using Core.Persistence.Repositories;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Persistence.Contexts;

namespace Kodlama.Io.Devs.Persistence.Repositories
{
    public class GitHubRepository : EfRepositoryBase<User, BaseDbContext>, IGitHubRepository
    {
        public GitHubRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
