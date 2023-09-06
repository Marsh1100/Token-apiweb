using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class DatosUserDto
{
    [Required]
    public string Mensaje { get; set; }
    [Required]
    public string EstadoAutenticado { get; set; }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }

     [Required]
    public List<string> Roles { get; set; }
    [Required]
    public string  Token{ get; set; }

}
