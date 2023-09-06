

using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Persistencia;

namespace API.Services;
//Contiene los metodos para generar el tokennn
public class UserService : IUserService
{
    private readonly TokenWebApiContext _context;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IPasswordHasher<User> _passwordHasher;


}
