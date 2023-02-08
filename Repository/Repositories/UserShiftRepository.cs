using Contracts;
using Entities;
using Entities.Models;
using PanacealogicsSales.Contracts;
using PanacealogicsSales.Entities.Models;

namespace Repository
{
    public class UserShiftRepository : RepositoryBase<UserShift>, IUserShiftRepository
    {
        public UserShiftRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
