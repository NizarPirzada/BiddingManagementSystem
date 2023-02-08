using Contracts;
using Entities;
using PanacealogicsSales.Entities.Models;

namespace Repository
{
    public class MessageRepository : RepositoryBase<Thread>, IThreadRepository
    {
        public MessageRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
