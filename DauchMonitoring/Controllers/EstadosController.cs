using DauchMonitoring.Models;
using DauchMonitoring.Models.DTOs.Estado;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EstadosController : ControllerBase
{
    private readonly AppDBContext _context;
    public EstadosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Estado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstadoDTO>>> GetEstado()
    {
        var estados = await _context.Estados
            .Select(e => new EstadoDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion
            })
            .ToListAsync();

        return Ok(estados);
    }

    // GET: api/Estado/5
    [HttpGet("{id}")]

    public async Task<ActionResult<EstadoDTO>> GetEstado(int id)
    {
        var estado = await _context.Estados
           .Select(e => new EstadoDTO
           {
               Id = e.Id,
               Nombre = e.Nombre,
               Descripcion = e.Descripcion
           })
           .FirstOrDefaultAsync(e => e.Id == id);

        if (estado == null)        
            return NotFound();        

        return Ok(estado);
    }
}
