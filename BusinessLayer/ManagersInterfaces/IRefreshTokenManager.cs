using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ManagersInterfaces
{
    public interface IRefreshTokenManager
    {
        public  Task AddRefreshToken(RefreshTokens refreshToken);
        public Task<RefreshTokens> GetRefreshToken(string refreshToken);
        public Task SaveChanges();
    }
}
