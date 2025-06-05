using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tickets1.Models;

namespace DulceSabor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ticketsDbContext _context;

        public HomeController(ticketsDbContext context)
        {
            _context = context;
        }

        // Vista inicial de login
        public IActionResult Login()
        {
            return View();  // Muestra la vista Login.cshtml
        }

        // M�todo POST para validar usuario y contrase�a
        [HttpPost]
        public async Task<IActionResult> LoginUsuario(string correo, string contrasenia, string returnUrl)
        {
            // Verificar si los campos no est�n vac�os
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
            {
                ViewBag.Error = "Por favor, complete todos los campos.";
                return View("Login");
            }

            // Buscar el usuario en la base de datos utilizando correo y contrase�a
            var usuario = await _context.usuarios
                .Include(u => u.roles)  // Incluir el rol del usuario
                .FirstOrDefaultAsync(u =>
                    u.correo.ToLower() == correo.ToLower() &&
                    u.contrasenia == contrasenia);  // Verificar la contrase�a

            // Si el usuario no existe
            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View("Login");
            }

            // Verifica que el usuario tiene un rol asignado
            if (usuario.roles == null)
            {
                ViewBag.Error = "Usuario sin rol asignado.";
                return View("Login");
            }

            // Si el usuario existe y tiene el rol de Administrador
            if (usuario.roles.nombre.ToLower() == "administrador")
            {
                // Crear una lista de claims con la informaci�n del usuario
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.correo),  // Nombre del usuario (correo)
            new Claim(ClaimTypes.Role, "Administrador")  // Rol del usuario
        };

                // Crear el objeto ClaimsIdentity con los claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Crear la cookie de autenticaci�n
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                // Si no hay ReturnUrl, redirige al AdminController
                return RedirectToAction("Index", "Admin");  // Redirige al AdminController
            }

            // Si el usuario no es administrador, redirige a la vista com�n de Home
            return RedirectToAction("Index", "Home");  // Redirige a la vista com�n de Home
        }



        // Acci�n Index com�n
        public IActionResult Index()
        {
            return View();  // P�gina de inicio com�n
        }
    }
}
