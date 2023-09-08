

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        DatosUserDto datosUserDto = new DatosUserDto();
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
                datosUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                datosUserDto.UserName = usuario.Username;
                datosUserDto.Email = usuario.Email;
                datosUserDto.Roles = usuario.Rols
                                    .Select(p=>p.Nombre)
                                    .ToList();
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

}
