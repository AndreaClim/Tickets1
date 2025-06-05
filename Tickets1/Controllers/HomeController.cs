using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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

        // Vista de inicio de sesión
        public IActionResult Login()
        {
            return View();  // Muestra la vista Login.cshtml
        }

        // Método POST para validar usuario y contraseña
        [HttpPost]
        public async Task<IActionResult> LoginUsuario(string correo, string contrasenia)
        {
            // Verificar si los campos no están vacíos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
            {
                ViewBag.Error = "Por favor, complete todos los campos.";
                return View("Login");
            }

            // Asegúrate de que el tipo de datos sea correcto
            var usuario = await _context.usuarios
                .Include(u => u.roles)  // Incluir el rol
                .FirstOrDefaultAsync(u =>
                    u.correo.ToLower() == correo.ToLower() &&
                    u.contrasenia == contrasenia);  // Verificar la contraseña

            // Si el usuario no existe
            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View("Login");
            }

            // Verificar que el usuario tiene un rol asignado
            if (usuario.roles == null)
            {
                ViewBag.Error = "Usuario sin rol asignado.";
                return View("Login");
            }

            // Si el usuario existe y tiene el rol de Administrador
            if (usuario.roles.nombre.ToLower() == "administrador")
            {
                // Crear una lista de claims con la información del usuario
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.correo),  // Nombre del usuario (correo)
            new Claim(ClaimTypes.Role, "Administrador")  // Rol del usuario
        };

                // Crear el objeto ClaimsIdentity con los claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Crear la cookie de autenticación
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Si no hay ReturnUrl, redirige al AdminController
                return RedirectToAction("Index", "Admin");  // Redirige al AdminController
            }

            // Si el usuario no es administrador, redirige a la vista común de Home
            return RedirectToAction("Index", "Home");  // Redirige a la vista común de Home
        }


        // Acción Index común
        public IActionResult Index()
        {
            return View();  // Página de inicio común
        }

        // Cerrar sesión
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // Limpiar la sesión
            return RedirectToAction("Login");  // Redirigir al login
        }
    }
}
