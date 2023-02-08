using Contracts;
using Entities;
using PanacealogicsSales.Contracts;
using PanacealogicsSales.Repository;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private RepositoryContext2 _repoContext2;
        private IUserRepository _user;
        private IAccountRepository _account;
        private IProposalRepository _proposal;
        private IProductRepository _product;
        private IProjectRepository _project;
        private IThreadRepository _thread;
        private IThreadHistoryRepository _threadhistory;
        private IThreadMessageRepository _threadmessage;
        private IUserProjectRepository _userproject;
        private ISkillRepository _skill;
        private IProjectRepository2 _project2;
        private ISocialUserRepository _socialuser;
        private IUserShiftRepository _usershift;
        public IProjectRepository2 Project2
        {
            get
            {
                if (_project2 == null)
                {
                    _project2 = new ProjectRepository2(_repoContext2);
                }
                return _project2;
            }
        }
        public IUserShiftRepository UserShift
        {
            get
            {
                if (_usershift == null)
                {
                    _usershift = new UserShiftRepository(_repoContext);
                }
                return _usershift;
            }
        }
        public IProposalRepository Proposal
        {
            get
            {
                if (_proposal == null)
                {
                    _proposal = new ProposalRepository(_repoContext);
                }
                return _proposal;
            }
        }
        public IThreadHistoryRepository ThreadHistory
        {
            get
            {
                if (_threadhistory == null)
                {
                    _threadhistory = new ThreadHistoryRepository(_repoContext);
                }
                return _threadhistory;
            }
        }
        public IThreadRepository Thread
        {
            get
            {
                if (_thread == null)
                {
                    _thread = new MessageRepository(_repoContext);
                }
                return _thread;
            }
        }
        public IThreadMessageRepository ThreadMessage
        {
            get
            {
                if (_threadmessage == null)
                {
                    _threadmessage = new ThreadMessageRepository(_repoContext);
                }
                return _threadmessage;
            }
        }
        public ISkillRepository Skill
        {
            get
            {
                if (_skill == null)
                {
                    _skill = new SkillRepository(_repoContext);
                }
                return _skill;
            }
        }
        public ISocialUserRepository SocialUser
        {
            get
            {
                if (_socialuser == null)
                {
                    _socialuser = new SocialUserRepository(_repoContext);
                }
                return _socialuser;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_repoContext);
                }
                return _product;
            }
        }
        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_repoContext);
                }
                return _account;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }
        public IProjectRepository Project
        {
            get
            {
                if (_project == null)
                {
                    _project = new ProjectRepository(_repoContext);
                }
                return _project;
            }
        }
        public IUserProjectRepository UserProject
        {
            get
            {
                if (_userproject == null)
                {
                    _userproject = new UserProjectRepository(_repoContext);
                }
                return _userproject;
            }
        }
        public RepositoryWrapper(RepositoryContext repositoryContext, RepositoryContext2 repositoryContext2)
        {
            _repoContext = repositoryContext;
            _repoContext2 = repositoryContext2;
        }
       

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
