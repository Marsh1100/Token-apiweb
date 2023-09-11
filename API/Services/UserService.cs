

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Helpers;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistencia;

namespace API.Services;
//Contiene los metodos para generar el tokennn
public class UserService : IUserService
{
    private readonly JWT _jwt;
   // private readonly TokenWebApiContext _context;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IPasswordHasher<User> _passwordHasher;
    //Registrar de manera asincrona los
    //Generar de manera dinámica los token
    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork =  unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var usuario = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username,
        };

        usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);
        var usuarioExiste = _unitOfWork.Users
                            .Find(u => u.Username.ToLower() == registerDto.Username.ToLower());
        Console.WriteLine(usuario.Password);
        if(usuarioExiste != null)
        {
            try
            {
                _unitOfWork.Users.Add(usuario);
                await _unitOfWork.SaveAsync();

                return $"El usuario {registerDto.Username} ha sido registrado correctamente";
            }
            catch(Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }else
        {
            return $"El usuario con {registerDto.Username} ya se encuentra registrado";
        }
    }

    public async Task<string> AddRoleAsync(AddRoleDto model)
    {
        var usuario = await _unitOfWork.Users
                            .GetByUsernameAsync(model.Username);

        if(usuario == null)
        {
            return $"No existe algun usuario registrado con la cuenta u olvido algún carácter ?{model.Username}";
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

        if(resultado == PasswordVerificationResult.Success)
        {
            var rolExiste = _unitOfWork.Rols
                            .Find(u => u.Nombre.ToLower() == model.Rol.ToLower())
                            .FirstOrDefault();
            
            if(rolExiste != null)
            {
                var usuarioTieneRol = usuario.Rols  
                                        .Any(u => u.Id == rolExiste.Id);    
                
                if(usuarioTieneRol == false)
                {
                    usuario.Rols.Add(rolExiste);
                    _unitOfWork.Users.Update(usuario);
                    await _unitOfWork.SaveAsync();
                    return $"Rol {model.Rol} agregado a la cuenta {model.Username} de forma existosa!";

                }
                
                    return $"El usuario {model.Username} ya tiene ese rol asignado";

                
                
            }

            return $"Rol {model.Rol} no encontrado.";
        }
        return $"Credenciales inconrrectas para el usuario {usuario.Username}";
    }
    private JwtSecurityToken CreateJwtToken(User usuario)
    {
        var roles = usuario.Rols;
        var roleClaims = new List<Claim>();
        foreach(var role in roles)
        {
            roleClaims.Add(new Claim("roles", role.Nombre));
        }

        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("uid", usuario.Id.ToString())
        }
        .Union(roleClaims);


        var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

        var signingCredentials =  new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims : claims,
            expires : DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials : signingCredentials
        );
        return jwtSecurityToken;
    }

    public async Task<DatosUserDto> GetTokenAsync(LoginDto model)
    {
        DatosUserDto datosUserDto = new();
        var usuario = await _unitOfWork.Users   
                            .GetByUsernameAsync(model.Username);
        
        if(usuario == null)
        {
            datosUserDto.EstadoAutenticado = false;
            datosUserDto.Mensaje = $"No existe ningún usuario con el username {model.Username}";
            return datosUserDto;
        }

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);
        if(result == PasswordVerificationResult.Success)
        {
            datosUserDto.Mensaje = "OK";
            datosUserDto.EstadoAutenticado = true;
            if(usuario != null)
            {
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);

                string refreshTokenCreado = GenerarRefreshToken();

                datosUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                datosUserDto.UserName = usuario.Username;
                datosUserDto.Email = usuario.Email;
                datosUserDto.Roles = usuario.Rols
                                    .Select(p=>p.Nombre)
                                    .ToList();
                datosUserDto.RefreshToken = refreshTokenCreado;

                await GuardarHistorialRefreshToken(usuario.Id, datosUserDto.Token, refreshTokenCreado);

                return datosUserDto;
            }else{
                datosUserDto.EstadoAutenticado=false;
                datosUserDto.Mensaje=$"Credenciales incorrectas para el usuario {usuario.Username}";
                return datosUserDto;
            }

        }
            datosUserDto.EstadoAutenticado = false;
            datosUserDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Username}";
            return datosUserDto;

    }

    private static string GenerarRefreshToken()
    {
        var byteArray = new byte[64];
        var refreshToken = "";
        using ( var mg = RandomNumberGenerator.Create())
        {
            mg.GetBytes(byteArray);
            refreshToken = Convert.ToBase64String(byteArray);
        }
        Console.WriteLine(refreshToken);
        return refreshToken;

      /*  var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);*/
    }

    private async Task<AutoTokenResponseDto> GuardarHistorialRefreshToken(int idUsuario,string token, string refreshToken)
    {
        var metHistorial = new HistorialRefreshToken();
        var EsActivo = metHistorial.TokenActivo();
        var historial = new HistorialRefreshToken()
        {
            IdUserFK = idUsuario,
            Token = token,
            RefreshToken = refreshToken,
            FechaCreacion = DateTime.UtcNow,
            //Pueden ser horas o días
            FechaExpiracion = DateTime.UtcNow.AddMinutes(2),
            Activo = EsActivo
        };
        _unitOfWork.RefreshTokens.Add(historial);
                await _unitOfWork.SaveAsync();

        return  new AutoTokenResponseDto { Token = token, RefreshToken = refreshToken, Result = true, Msg = "Ok" };

        
    }

    public async Task<AutoTokenResponseDto> GetTokenRefreshAsync(RefreshTokenUserDto model, int idUsuario)

    {
        var refreshTokenFind = _unitOfWork.RefreshTokens.Find(u => u.Token== model.Token && 
        u.RefreshToken == model.RefreshToken &&
        u.Id == idUsuario);

        if(refreshTokenFind == null)
        {
            return  new AutoTokenResponseDto {Result = false, Msg = "No existe refresh token"};

        }
        var user = await _unitOfWork.Users.GetByUserIdAsync(idUsuario);
        if(user == null )
        {
            return  new AutoTokenResponseDto {Result = false, Msg = "No existe usuario registrado"};

        }
        var refreshToken = GenerarRefreshToken();
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(user); 
        var tokencreado= new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return   await GuardarHistorialRefreshToken(idUsuario,tokencreado, refreshToken);

        
   }

}
