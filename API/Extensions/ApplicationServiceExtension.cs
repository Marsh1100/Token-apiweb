using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.UnitOfWork;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions;

public static class ApplicationServiceExtension
{
    public static void ConfigureCors(this IServiceCollection services) =>
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()  //Withorigin (https://domini.com)
        .AllowAnyMethod()          //WithMethods ('GET', 'POST')
        .AllowAnyHeader());
    });

    public static void AddAplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserServices, UserService>();
        services.AddScoped<IAuthorizationHandler, GlobalVerb>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
