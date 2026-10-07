using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs.Aplicacion;
using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Equipo;
using MonitoreoAPI.Models.DTOs.Estado;
using MonitoreoAPI.Models.DTOs.Planta;
using MonitoreoAPI.Models.DTOs.Recurso;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;

[Route("api/[controller]")]
[ApiController]
public class RecursosController : ControllerBase
{
    private readonly AppDBContext _context;
    public RecursosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Recurso
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecursoResponseDTO>>> GetRecurso(
        int? idEstado,
        int? idArea,
        int? idPlanta,
        int? idTipoRecurso,
        string? nombre)
    {
        var query = _context.Recursos.AsQueryable();

        if (idEstado.HasValue)        
            query = query.Where(r => r.Id_estado == idEstado.Value);

        if (idTipoRecurso.HasValue)
            query = query.Where(r => r.Id_tipo_recurso == idTipoRecurso.Value);

        if (idArea.HasValue)        
            query = query.Where(r => r.Id_area == idArea.Value);        

        if (idPlanta.HasValue)        
            query = query.Where(r => r.Area.Id_planta == idPlanta.Value);        

        if (!string.IsNullOrEmpty(nombre))        
            query = query.Where(r => r.Nombre.Contains(nombre));
        

        var recursos = await query
            .Select(r => new RecursoResponseDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Id_estado = r.Id_estado,
                Id_area = r.Id_area,
                Id_tipo_recurso = r.Id_tipo_recurso,
                TipoRecurso = new TipoRecursoDTO 
                {
                    Id = r.Id_tipo_recurso,
                    Nombre = r.TipoRecurso.Nombre
                },
                Estado = new EstadoDTO
                {
                    Id = r.Estado.Id,
                    Nombre = r.Estado.Nombre,
                    Descripcion = r.Estado.Descripcion                    
                },

                Area = new AreaResponseDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    Id_planta = r.Area.Id_planta,

                    Planta = new PlantaDTO
                    {
                        Nombre = r.Area.Planta.Nombre
                    }
                },
             

               
            })
            .ToListAsync();

        return Ok(recursos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecursoResponseDTO>> GetRecurso(int id)
    {
        var recurso = await _context.Recursos
            .Where(r => r.Id == id)
            .Select(r => new RecursoResponseDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Id_estado = r.Id_estado,
                Id_area = r.Id_area,
                Id_tipo_recurso = r.Id_tipo_recurso,
                TipoRecurso = new TipoRecursoDTO
                {
                    Id = r.Id_tipo_recurso,
                    Nombre = r.TipoRecurso.Nombre
                },
                Estado = new EstadoDTO
                {
                    Id = r.Estado.Id,
                    Nombre = r.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    Id_planta = r.Area.Id_planta,

                    Planta = new PlantaDTO
                    {
                        Nombre = r.Area.Planta.Nombre
                    }
                },

            })
            .FirstOrDefaultAsync();

        if (recurso == null)
            return NotFound();

        return Ok(recurso);
    }
}