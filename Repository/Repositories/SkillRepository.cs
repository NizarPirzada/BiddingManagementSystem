using Entities;
using PanacealogicsSales.Contracts;
using PanacealogicsSales.Entities.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanacealogicsSales.Repository
{
    public class SkillRepository : RepositoryBase<Skill>, ISkillRepository
    {
        public SkillRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
