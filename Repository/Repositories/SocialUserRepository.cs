using Contracts;
using Entities;
using PanacealogicsSales.Contracts;
using PanacealogicsSales.Entities.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanacealogicsSales.Repository
{
    public class SocialUserRepository : RepositoryBase<SocialUser>, ISocialUserRepository
    {
        public SocialUserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
