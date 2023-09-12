using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Repository;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TokenWebApiContext _context;
    public UnitOfWork( TokenWebApiContext context)
    {
        _context = context;
    }

    private RolRepository _rol;

    public IRol Rols
    {
        get{
            if(_rol == null){
                _rol = new RolRepository(_context);
            }
            return _rol;
        }
    }

    private UserRepository _user;

    public IUser Users
    {
        get{
            if(_user == null){
                _user = new UserRepository(_context);
            }
            return _user;
        }
    }

    private RefreshTokenRepository _refresh;

    public IRefreshToken RefreshTokens
    {
        get{
            if(_refresh == null){
                _refresh = new RefreshTokenRepository(_context);
            }
            return _refresh;
        }
    }


    public void Dispose()
    {
        _context.Dispose();    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();

    }
}
