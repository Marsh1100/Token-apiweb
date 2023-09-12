using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Interfaces;

public interface IUnitOfWork
{
    IUser Users { get; }
    IRol Rols { get; }

    IRefreshToken RefreshTokens { get; }

    Task<int> SaveAsync();
}
