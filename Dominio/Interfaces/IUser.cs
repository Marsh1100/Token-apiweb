using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IUser: IGenericRepository<User>
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByUserIdAsync(int userId);
}
