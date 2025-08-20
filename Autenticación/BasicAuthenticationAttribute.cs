using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text;

namespace Proyecto_Final.Autenticación
{
    public class BasicAuthenticationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic "))
            {
                var token = authHeader.Substring("Basic ".Length).Trim();
                var credenciales = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var partes = credenciales.Split(':');

                if (partes.Length == 2)
                {
                    var usuario = partes[0];
                    var contraseña = partes[1];

                    if (usuario == "admin" && contraseña == "1234")
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, usuario) };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        var principal = new ClaimsPrincipal(identity);

                        context.HttpContext.User = principal;
                        return;
                    }
                }
            }

            //context.Result = new UnauthorizedResult();
            context.Result = new JsonResult(new
            {
                codigo = 401,
                mensaje = "Acceso no autorizado. Por favor, verifica tus credenciales."
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

        }
    }
}
