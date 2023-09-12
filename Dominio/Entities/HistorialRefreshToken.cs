using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities;

public class HistorialRefreshToken : BaseEntity
{
        
    public int IdUserFK {get; set; }
    public User User {get; set; }
    public string Token {get; set; }
    public string RefreshToken  {get; set; }
    public DateTime FechaCreacion {get; set; }
    public DateTime FechaExpiracion {get; set; }
    public bool Activo { get; set; }

    public HistorialRefreshToken()
    {

    }
    public bool TokenActivo()
    {
        return FechaExpiracion<DateTime.UtcNow;
    }




}