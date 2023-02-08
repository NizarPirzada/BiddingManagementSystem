using Contracts;
using Entities;
using Entities.Models;
using PanacealogicsSales.Entities.Models;

namespace Repository
{
    public class ProjectRepository2 : RepositoryBase<Project2>, IProjectRepository2
    {
        public ProjectRepository2(RepositoryContext2 repositoryContext2)
            : base(repositoryContext2)
        {
        }
    }
}
