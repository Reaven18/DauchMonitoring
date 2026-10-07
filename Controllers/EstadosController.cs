using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs.Estado;

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
        var estado = await _context.Estados    
            .Select(e => NuevoEstado(e)
            ).ToListAsync();        

        return Ok(estado);
    }

    // GET: api/Estado/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EstadoDTO>> GetEstado(int id)
    {
        var estado = await _context.Estados
            .Where(e => e.Id == id)
            .Select(e => NuevoEstado(e))
            .FirstOrDefaultAsync();            

        if (estado == null)        
            return NotFound();        

        return Ok(estado);
    }


    private static EstadoDTO NuevoEstado(Estado e) 
    {
        return new EstadoDTO
        {
            Id = e.Id,
            Nombre = e.Nombre,
            Descripcion = e.Descripcion            
        };
    }
}