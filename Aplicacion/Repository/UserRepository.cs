using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class UserRepository : GenericRepository<User>, IUser
{
    private readonly TokenWebApiContext _context;

    public UserRepository(TokenWebApiContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
       return await _context.Users 
                .Include(u => u.Rols)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower()); 
    }

    public async Task<User> GetByUserIdAsync(int id)
    {
        return await _context.Users
                .Include(u => u.Rols)
                .FirstOrDefaultAsync(u => u.Id == id);
    }
   
}
