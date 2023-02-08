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
    public class ThreadMessageRepository : RepositoryBase<ThreadMessage>, IThreadMessageRepository
    {
        public ThreadMessageRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
