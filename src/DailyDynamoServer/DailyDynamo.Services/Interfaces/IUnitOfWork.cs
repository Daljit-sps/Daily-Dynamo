using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IUserProfileRepository UserProfile { get; }

        IUserAccountRepository UserAccount { get; }

        IWorkDiaryRepository WorkDiary { get; }

        Task<int> SaveChangesAsync();
    }
}
