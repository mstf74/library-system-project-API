using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.GenericRepo;
using DataAcessLayer.Models;
using DataAcessLayer.Repositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class RefreshTokenManager:IRefreshTokenManager
    {
        private readonly RefreshTokenRepositry _repositry;

        public RefreshTokenManager(RefreshTokenRepositry repo)
        {
            _repositry = repo;
        }
        public async Task AddRefreshToken(RefreshTokens refreshToken)
        {
             await _repositry.AddRefreshToken(refreshToken);
        }
        public async Task<RefreshTokens> GetRefreshToken(string refreshToken)
        {
            return await _repositry.getRegreshToken(refreshToken);
        }
        public async Task SaveChanges()
        => await _repositry.SaveChanges();


    }
}
