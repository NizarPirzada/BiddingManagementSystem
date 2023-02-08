using Contracts;
using Entities;
using Entities.Models;
using PanacealogicsSales.Entities.Models;

namespace Repository
{
    public class ThreadHistoryRepository : RepositoryBase<ThreadHistory>, IThreadHistoryRepository
    {
        public ThreadHistoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
