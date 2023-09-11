using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class AutoTokenResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public bool Result { get; set; }
    public string Msg { get; set; }

}
