using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        // VISTA INICIAL DE LOGIN
        public IActionResult Login()
        {
            return View();
        }

        // M�TODO POST PARA VALIDAR USUARIO Y CONTRASE�A
        [HttpPost]
        public async Task<IActionResult> LoginUsuario(string nombre, string apellido, string contrasenia)
        {
            // Verificar si los campos no est�n vac�os
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(contrasenia))
            {
                ViewBag.Error = "Por favor, complete todos los campos.";
                return View("Login");
            }

            // Buscar el usuario en la base de datos
            var usuario = await _context.usuarios
                .Include(u => u.roles) // Incluir el rol
                .FirstOrDefaultAsync(u =>
                    u.nombre.ToLower() == nombre.ToLower() &&
                    u.apellido.ToLower() == apellido.ToLower() &&
                    u.contrasenia == contrasenia); // Verificar la contrase�a

            // Si el usuario no existe
            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View("Login");
            }

            // Si el usuario existe, redirigir al �ndice (puede ser una vista com�n para todos)
            return RedirectToAction("Index", "Home");  // Redirige al controlador Home (vista Index com�n)
        }

        // Acci�n Index com�n
        public IActionResult Index()
        {
            return View();  // Aqu� puedes tener la vista com�n de index
        }
    }
}
