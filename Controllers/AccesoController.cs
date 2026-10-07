using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Custom;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs;

namespace MonitoreoAPI.Controllers
{
   [Route("api/")]   
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
        [AllowAnonymous]
        public async Task<IActionResult> Registrarse(UsuarioDTO user)
        {
            var modeloUsuario = new Usuario
            {
                Correo = user.Correo,
                PasswordHash = _utilities.encryptSHA256(user.Password)
            };
            await _appDBContext.Usuarios.AddAsync(modeloUsuario);
            await _appDBContext.SaveChangesAsync();

            if (modeloUsuario.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            var usuarioEncontrado = await _appDBContext.Usuarios
                                   .Where(U => 
                                     U.Correo == user.Correo && 
                                     U.PasswordHash == _utilities.encryptSHA256(user.Password)
                                   ).FirstOrDefaultAsync();
            if(usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilities.generarJWT(usuarioEncontrado) });
          
        }

        }
}
