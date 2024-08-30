using DailyDynamo.DAL.Context;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DailyDynamo.DAL.Repository
{
    public class UserProfileRepository : GenericRepository<Profile>, IUserProfileRepository
    {
        public UserProfileRepository(DailyDynamoDatabaseContext context) : base(context) { } 
    }
}
