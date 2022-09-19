﻿using Core.Persistence.Repositories;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using Kodlama.Io.Devs.Persistence.Contexts;

namespace Kodlama.Io.Devs.Persistence.Repositories
{
    public class GitHubRepository : EfRepositoryBase<GitHub, BaseDbContext>, IGitHubRepository
    {
        public GitHubRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
