using Contracts;
using Entities;
using Entities.Models;
using PanacealogicsSales.Entities.Models;

namespace Repository
{
    public class ProposalRepository : RepositoryBase<Proposal>, IProposalRepository
    {
        public ProposalRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
