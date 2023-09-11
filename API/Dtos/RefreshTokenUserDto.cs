using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;

namespace API.Dtos
{
    public class RefreshTokenUserDto
    {
        public int IdUserFK {get; set; }
        public User User {get; set; }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}