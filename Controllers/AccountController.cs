using AgendaConsultas.Data;
using AgendaConsultas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgendaConsultas.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AgendaConsultas.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            bool emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == usuario.Email);

            if (emailExiste)
            {
                ModelState.AddModelError(
                    "Email",
                    "Já existe um usuário cadastrado com este e-mail."
                );

                return View(usuario);
            }

            usuario.Senha = _passwordHasher.HashPassword(
                usuario,
                usuario.Senha
            );

            usuario.DataCadastro = DateTime.Now;

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Cadastro realizado com sucesso!";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-mail ou senha inválidos."
                );

                return View(model);
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                model.Senha
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "E-mail ou senha inválidos."
                );

                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            usuario.Id.ToString()
        ),

        new Claim(
            ClaimTypes.Name,
            usuario.Nome
        ),

        new Claim(
            ClaimTypes.Email,
            usuario.Email
        )
    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login", "Account");
        }

    }
}