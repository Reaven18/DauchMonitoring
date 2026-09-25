using DauchMonitoring.Models;
using DauchMonitoring.Models.DTOs.Aplicacion;
using DauchMonitoring.Models.DTOs.Area;
using DauchMonitoring.Models.DTOs.Equipo;
using DauchMonitoring.Models.DTOs.Estado;
using DauchMonitoring.Models.DTOs.Planta;
using DauchMonitoring.Models.DTOs.Recurso;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        string? nombre)
    {
        var query = _context.Recursos.AsQueryable();

        if (idEstado.HasValue)
        {
            query = query.Where(r => r.IdEstado == idEstado.Value);
        }

        if (idArea.HasValue)
        {
            query = query.Where(r => r.IdArea == idArea.Value);
        }

        if (idPlanta.HasValue)
        {
            query = query.Where(r => r.Area.IdPlanta == idPlanta.Value);
        }

        if (!string.IsNullOrEmpty(nombre))
        {
            query = query.Where(r => r.Nombre.Contains(nombre));
        }

        var recursos = await query
            .Select(r => new RecursoResponseDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                IdEstado = r.IdEstado,
                IdArea = r.IdArea,

                Estado = new EstadoDTO
                {
                    Id = r.Estado.Id,
                    Nombre = r.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    IdPlanta = r.Area.IdPlanta,

                    Planta = new PlantaDTO
                    {
                        Nombre = r.Area.Planta.Nombre
                    }
                },

                Equipo = r.Equipo != null
                    ? new EquipoDTO
                    {
                        Codigo = r.Equipo.Codigo,
                        Modelo = r.Equipo.Modelo,
                        Ip = r.Equipo.Ip,
                        Cpu = r.Equipo.Cpu,
                        Memoria = r.Equipo.Memoria,
                        Puerto = r.Equipo.Puerto,
                        Heartbeat = r.Equipo.Heartbeat
                    }
                    : null,

                Aplicacion = r.Aplicacion != null
                    ? new AplicacionDTO
                    {
                        Codigo = r.Aplicacion.Codigo,
                        Version = r.Aplicacion.Version,
                        LastUpdate = r.Aplicacion.LastUpdate
                    }
                    : null
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
                IdEstado = r.IdEstado,
                IdArea = r.IdArea,

                Estado = new EstadoDTO
                {
                    Id = r.Estado.Id,
                    Nombre = r.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    IdPlanta = r.Area.IdPlanta,

                    Planta = new PlantaDTO
                    {
                        Nombre = r.Area.Planta.Nombre
                    }
                },

                Equipo = r.Equipo != null
                    ? new EquipoDTO
                    {
                        Codigo = r.Equipo.Codigo,
                        Modelo = r.Equipo.Modelo,
                        Ip = r.Equipo.Ip,
                        Cpu = r.Equipo.Cpu,
                        Memoria = r.Equipo.Memoria,
                        Puerto = r.Equipo.Puerto,
                        Heartbeat = r.Equipo.Heartbeat
                    }
                    : null,

                Aplicacion = r.Aplicacion != null
                    ? new AplicacionDTO
                    {
                        Codigo = r.Aplicacion.Codigo,
                        Version = r.Aplicacion.Version,
                        LastUpdate = r.Aplicacion.LastUpdate
                    }
                    : null
            })
            .FirstOrDefaultAsync();

        if (recurso == null)
            return NotFound();

        return Ok(recurso);
    }
}