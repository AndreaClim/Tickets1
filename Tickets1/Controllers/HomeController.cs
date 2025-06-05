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

        // MÉTODO POST PARA VALIDAR USUARIO Y CONTRASEÑA
        [HttpPost]
        public async Task<IActionResult> LoginUsuario(string nombre, string apellido, string contrasenia)
        {
            // Verificar si los campos no están vacíos
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
                    u.contrasenia == contrasenia); // Verificar la contraseña

            // Si el usuario no existe
            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View("Login");
            }

            // Si el usuario existe, redirigir al índice (puede ser una vista común para todos)
            return RedirectToAction("Index", "Home");  // Redirige al controlador Home (vista Index común)
        }

        // Acción Index común
        public IActionResult Index()
        {
            return View();  // Aquí puedes tener la vista común de index
        }
    }
}
