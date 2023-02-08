using Entities;
using PanacealogicsSales.Contracts;
using PanacealogicsSales.Entities.Models;

namespace Repository
{
    public class UserProjectRepository : RepositoryBase<UserProject>, IUserProjectRepository
    {
        public UserProjectRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
