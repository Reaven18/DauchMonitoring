using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;

[Route("api/[controller]")]
[ApiController]
public class Tipo_recursosController : ControllerBase
{
    private readonly AppDBContext _context;
    public Tipo_recursosController(AppDBContext context)
    {
        _context = context;
    }
    // GET: api/Estado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoRecursoDTO>>> GetTipoRecurso()
    {
        var tipoRecursos = await _context.Tipo_Recursos
            .Select(e => NuevoTipoRecurso(e)
            ).ToListAsync();

        return Ok(tipoRecursos);
    }

    // GET: api/Estado/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TipoRecursoDTO>> GetTipoRecurso(int id)
    {
        var tipoRecurso = await _context.Tipo_Recursos
            .Where(e => e.Id == id)
            .Select(e => NuevoTipoRecurso(e))
            .FirstOrDefaultAsync();

        if (tipoRecurso == null)
            return NotFound();

        return Ok(tipoRecurso);
    }


    private static TipoRecursoDTO NuevoTipoRecurso(Tipo_recurso e)
    {
        return new TipoRecursoDTO
        {
            Id = e.Id,
            Nombre = e.Nombre,            
        };
    }
}
