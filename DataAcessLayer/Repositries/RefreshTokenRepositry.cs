using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositries
{
    public class RefreshTokenRepositry
    {
        private Context _context;

        public RefreshTokenRepositry(Context context)
        {
            _context = context;
        }
        public async Task AddRefreshToken(RefreshTokens refreshToken)
        {
            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
        public async Task<RefreshTokens> getRegreshToken(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(e => e.token == refreshToken);
        }
        public async Task SaveChanges()
       => await _context.SaveChangesAsync();

    }
}
