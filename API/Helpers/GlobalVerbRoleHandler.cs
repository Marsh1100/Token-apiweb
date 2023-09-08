using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Helpers;

public class GlobalVerbRoleHandler : AuthorizationHandler<GlobalVerbRoleRequeriment>
{
    private readonly IHttpContextAccessor _httoContextAccessor;

    public GlobalVerbRoleHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httoContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GlobalVerbRoleRequeriment requeriment)
    {
        // Obtener todos los roles del usuario
        var roles = context.User.FindAll(c => string.Equals(c.Type, ClaimTypes.Role)).Select(c=> c.Value);
        // Obtener el método HTTP de la solicitud actual
        var verb = _httoContextAccessor.HttpContext?.Request.Method;
        
        // Verificar si el método HTTP es nulo o vacío
        if(string.IsNullOrEmpty(verb)) 
        {
            throw new Exception($"request cann't be null");
        }

        foreach(var role in roles)
        {
            if(requeriment.IsAllowed(role, verb)) 
            {
                context.Succeed(requeriment);
                return Task.CompletedTask;
            }
        }
        context.Fail();
        return Task.CompletedTask;
    }

    
}
