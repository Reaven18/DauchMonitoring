using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DauchMonitoring.Custom;
using DauchMonitoring.Models.DTOs;
using DauchMonitoring.Models;

namespace DauchMonitoring.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;
        private readonly Utilities _utilities;
        public AccesoController(AppDBContext appDBContext, Utilities utilities)
        {
            _appDBContext = appDBContext;
            _utilities = utilities;
            
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO user)
        {
            var modeloUsuario = new Usuario
            {
                Correo = user.Correo,
                PasswordHash = _utilities.encryptSHA256(user.password)
            };
            await _appDBContext.Usuarios.AddAsync(modeloUsuario);
            await _appDBContext.SaveChangesAsync();

            if (modeloUsuario.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false });
            
        }
    }
}
