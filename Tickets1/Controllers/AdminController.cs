using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tickets1.Models;

namespace DulceSabor.Controllers
{
    [Authorize(Roles = "Administrador")]  // Asegura que solo los administradores puedan acceder
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
            var tickets = await _context.tickets
                .Include(t => t.id_usuarioC)  // Cargar cliente
                .Include(t => t.id_usuarioE)  // Cargar técnico asignado
                .Include(t => t.id_cat)       // Cargar categoría
                .Include(t => t.id_estado)    // Cargar estado del ticket
                .ToListAsync();

            return View(tickets);  // Asegúrate de que la vista Index.cshtml está en /Views/Admin
        }

        // Vista de detalles de un ticket
        public async Task<IActionResult> Detalles(int id)
        {
            var ticket = await _context.tickets
                .Include(t => t.id_usuarioC)
                .Include(t => t.id_usuarioE)
                .Include(t => t.id_cat)
                .Include(t => t.id_estado)
                .FirstOrDefaultAsync(t => t.id_ticket == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // Acción para actualizar el estado de un ticket
        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, int estadoId, int tecnicoId)
        {
            var ticket = await _context.tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.id_estado = estadoId; // Actualizamos el estado
            ticket.id_usuarioE = tecnicoId; // Asignamos un técnico

            _context.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detalles), new { id = ticket.id_ticket });
        }

        // Vista para gestionar usuarios
        public IActionResult GestionUsuarios()
        {
            var usuarios = _context.usuarios.Include(u => u.roles).ToList();
            return View(usuarios);
        }

        // Acción para crear un nuevo usuario
        [HttpGet]
        public IActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GestionUsuarios));
            }

            return View(usuario);
        }

        // Acción para eliminar usuario
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(GestionUsuarios));
        }
    }
}
