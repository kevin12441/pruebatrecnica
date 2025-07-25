using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private readonly PruebatecnicaContext _context;

        public AccountController(PruebatecnicaContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "El email y la contraseña son requeridos");
                    return View();
                }

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                    return View();
                }

                try
                {
                    // Verifica si la contraseña almacenada es un hash BCrypt válido
                    if (!user.Password.StartsWith("$2a$") && !user.Password.StartsWith("$2b$") && !user.Password.StartsWith("$2y$"))
                    {
                        // Si la contraseña no está hasheada, la hasheamos
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        await _context.SaveChangesAsync();
                    }

                    bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

                    if (!isValidPassword)
                    {
                        ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                        return View();
                    }
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    // Si hay un error con el formato del salt, rehasheamos la contraseña
                    user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Si hay un error al verificar la contraseña, probablemente es porque no está hasheada correctamente
                    ModelState.AddModelError("", "Error al verificar credenciales. Por favor, contacte al administrador.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim("FullName", user.Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(1)
                    });

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error en el proceso de inicio de sesión. Por favor, intente nuevamente.");
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
