using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public IUser Users => throw new NotImplementedException();

    public IRol Rols => throw new NotImplementedException();

    public void Dispose()
    {
        _context.Dispose();    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();

    }
}
