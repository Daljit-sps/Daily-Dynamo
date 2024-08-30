using DailyDynamo.DAL.Context;
using DailyDynamo.DAL.Repository;
using DailyDynamo.Services.Interfaces;

namespace DailyDynamo.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DailyDynamoDatabaseContext _context;
        private IUserProfileRepository _userProfileRepository;
        private IUserAccountRepository _userAccountRepository;
        private IWorkDiaryRepository _workDiaryRepository;

        public UnitOfWork(DailyDynamoDatabaseContext context)
        {
            _context = context;
        }

        public IUserProfileRepository UserProfile
        {
            get
            {
                _userProfileRepository ??= new UserProfileRepository(_context);
                return _userProfileRepository;
            }
        }

        public IUserAccountRepository UserAccount
        {
            get
            {
                _userAccountRepository ??= new UserAccountRepository(_context);
                return _userAccountRepository;
            }
        }

        public IWorkDiaryRepository WorkDiary => _workDiaryRepository ??= new WorkDiaryRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
