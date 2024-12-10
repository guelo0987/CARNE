using System.Security.Claims;
using CARNE.Context;
using Microsoft.AspNetCore.Mvc.Controllers;

public class RolePermissionMiddleware
{
    private readonly RequestDelegate _next;

    public RolePermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;

        // Excluir rutas específicas: Swagger y Login
        if (path != null && (path.StartsWith("/") || path.StartsWith("/api/Auth/login")))
        {
            await _next(context);
            return;
        }

        // Resolver el DbContext
        var db = context.RequestServices.GetRequiredService<MyDbContext>();
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

        // Si no se encuentra un rol, denegar acceso
        if (userRole == null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Acceso denegado: Usuario no autenticado.");
            return;
        }

        // Permitir acceso completo para Admin
        if (userRole == "Admin")
        {
            await _next(context);
            return;
        }

        // Obtener el nombre del controlador desde el endpoint
        var endpoint = context.GetEndpoint();
        var controllerName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName;

        if (controllerName == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Error en la ruta.");
            return;
        }

        // Verificar permisos en la tabla RolePermisos
        var permission = db.RolePermisos
            .FirstOrDefault(p => p.Role == userRole && p.TableName == controllerName);

        if (permission == null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync($"Acceso denegado: No se encontraron permisos para el rol '{userRole}' en la tabla '{controllerName}'.");
            return;
        }

        // Validar los permisos según el método HTTP
        switch (context.Request.Method)
        {
            case "GET":
                if (!permission.CanRead)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync($"Acceso denegado: El rol '{userRole}' no tiene permiso para leer en la tabla '{controllerName}'.");
                    return;
                }
                break;

            case "POST":
                if (!permission.CanCreate)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync($"Acceso denegado: El rol '{userRole}' no tiene permiso para crear en la tabla '{controllerName}'.");
                    return;
                }
                break;

            case "PUT":
                if (!permission.CanUpdate)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync($"Acceso denegado: El rol '{userRole}' no tiene permiso para actualizar en la tabla '{controllerName}'.");
                    return;
                }
                break;

            case "DELETE":
                if (!permission.CanDelete)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync($"Acceso denegado: El rol '{userRole}' no tiene permiso para eliminar en la tabla '{controllerName}'.");
                    return;
                }
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync($"Método HTTP '{context.Request.Method}' no permitido.");
                return;
        }

        // Continuar con la solicitud
        await _next(context);
    }
}
