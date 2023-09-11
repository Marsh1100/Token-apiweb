using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : ApiBaseController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        var result = await _userService.RegisterAsync(model);

        if(result != null)
        {
            return Ok(result);
        }
        return Unauthorized();
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        return Ok(result);
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenUserDto model)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenExpirado = tokenHandler.ReadJwtToken(model.Token);

        if(tokenExpirado.ValidTo > DateTime.UtcNow)
        {
            return BadRequest(new AutoTokenResponseDto { Result = false, Msg = "El Token no ha expirado" });;
        }
        string idUsuario = tokenExpirado.Claims.First(
            X => X.Type =="uid").Value.ToString();
        var autorizacion = await _userService.GetTokenRefreshAsync(model,int.Parse(idUsuario));
        
        if(autorizacion.Result){
            return Ok(autorizacion);
        }

        return BadRequest(autorizacion);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }
}
