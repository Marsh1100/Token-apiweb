using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.Repository;

public class RolRepository : GenericRepository<Rol>, IRol
{
   private readonly TokenWebApiContext _context;

   public RolRepository(TokenWebApiContext context) : base(context)
   {
        this._context = context;    
   } 

   
}
