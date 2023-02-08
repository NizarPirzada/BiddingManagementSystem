using PanacealogicsSales.Contracts;

namespace Contracts
{
    public interface IRepositoryWrapper
    {

        IAccountRepository Account { get; }
        IUserRepository User { get; }
        IThreadRepository Thread { get; }
        IThreadMessageRepository ThreadMessage { get; }
        IThreadHistoryRepository ThreadHistory { get; }
        IProductRepository Product { get; }
        IProjectRepository Project { get; }
        IUserProjectRepository UserProject { get; }
        ISkillRepository Skill { get; }
        IProposalRepository Proposal { get; }
        ISocialUserRepository SocialUser { get; }
        IUserShiftRepository UserShift { get; }
        IProjectRepository2 Project2 { get; }
        void Save();
    }
}
