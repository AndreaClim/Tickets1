using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tickets1.Models;

namespace DulceSabor.Controllers
{
    public class AdminController : Controller
    {
        private readonly ticketsDbContext _context;

        public AdminController(ticketsDbContext context)
        {
            _context = context;
        }

        // Vista principal de gestión de tickets
        public async Task<IActionResult> Index()
        {
            // Verifica si el usuario está autenticado y es administrador
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userEmail) || userRole != "Administrador")
            {
                return RedirectToAction("Login", "Home");  // Redirige al login si no está autenticado o no es administrador
            }

            // Recupera los tickets
            var tickets = await _context.tickets
                .Include(t => t.usuarioCliente)
                .Include(t => t.usuarioEmpleado)
                .Include(t => t.categorias)
                .Include(t => t.estados)
                .ToListAsync();

            return View(tickets);  // Asegúrate de que la vista Index.cshtml está en /Views/Admin
        }
    }
}
