using AgendaConsultas.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgendaConsultas.Models;

namespace AgendaConsultas.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == usuarioId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consulta consulta)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            consulta.UsuarioId = usuarioId;

            ModelState.Remove("UsuarioId");
            ModelState.Remove("Usuario");

            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            _context.Consultas.Add(consulta);

            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Consulta cadastrada com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId
                );

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Consulta consulta)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            if (id != consulta.Id)
            {
                return NotFound();
            }

            var consultaBanco = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId
                );

            if (consultaBanco == null)
            {
                return NotFound();
            }

            ModelState.Remove("UsuarioId");
            ModelState.Remove("Usuario");

            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            consultaBanco.Especialidade = consulta.Especialidade;
            consultaBanco.DataHora = consulta.DataHora;
            consultaBanco.Descricao = consulta.Descricao;

            await _context.SaveChangesAsync();

            TempData["Sucesso"] =
                "Consulta atualizada com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId
                );

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId
                );

            if (consulta == null)
            {
                return NotFound();
            }

            _context.Consultas.Remove(consulta);

            await _context.SaveChangesAsync();

            TempData["Sucesso"] =
                "Consulta excluída com sucesso!";

            return RedirectToAction(nameof(Index));
        }

    }
}