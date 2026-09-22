using DauchMonitoring.Models;
using DauchMonitoring.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

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
    public async Task<ActionResult<IEnumerable<RecursoDTO>>> GetRecurso()
    {
        var recursos = await _context.Recursos
            .Select(r => new RecursoDTO
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

                Area = new AreaDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    IdPlanta = r.Area.IdPlanta,

                    Planta = new PlantaDTO
                    {
                        Id = r.Area.Planta.Id,
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

    // GET: api/recursos/equipos
    [HttpGet("equipos")]
    public async Task<ActionResult<IEnumerable<RecursoDTO>>> GetEqupos()
    {
        var recursos = await _context.Recursos
            .Where(r => r.Equipo != null)
            .Select(r => new RecursoDTO
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
                Area = new AreaDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    IdPlanta = r.Area.IdPlanta,
                    Planta = new PlantaDTO
                    {
                        Id = r.Area.Planta.Id,
                        Nombre = r.Area.Planta.Nombre
                    }
                },
                Equipo = new EquipoDTO
                {
                    Codigo = r.Equipo.Codigo,
                    Modelo = r.Equipo.Modelo,
                    Ip = r.Equipo.Ip,
                    Cpu = r.Equipo.Cpu,
                    Memoria = r.Equipo.Memoria,
                    Puerto = r.Equipo.Puerto,
                    Heartbeat = r.Equipo.Heartbeat
                }
            })
            .ToListAsync();
        return Ok(recursos);
    }

    // GET : api/recursos/aplicaciones
    [HttpGet("aplicaciones")]
    public async Task<ActionResult<IEnumerable<RecursoDTO>>> GetAplicaciones()
    {
        var recursos = await _context.Recursos
            .Where(r => r.Aplicacion != null)
            .Select(r => new RecursoDTO
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
                Area = new AreaDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    IdPlanta = r.Area.IdPlanta,
                    Planta = new PlantaDTO
                    {
                        Id = r.Area.Planta.Id,
                        Nombre = r.Area.Planta.Nombre
                    }
                },
                Aplicacion = new AplicacionDTO
                {
                    Codigo = r.Aplicacion.Codigo,
                    Version = r.Aplicacion.Version,
                    LastUpdate = r.Aplicacion.LastUpdate
                }
            })
            .ToListAsync();
        return Ok(recursos);
    }

    // GET: api/Recurso/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RecursoDTO>> GetRecurso(int id)
    {
        var recurso = await _context.Recursos
            .Include(a => a.Area)
            .ThenInclude(p => p.Planta)
            .Where(r => r.Id == id)
            .Select(r => new RecursoDTO
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
                Area = new AreaDTO
                {
                    Id = r.Area.Id,
                    Nombre = r.Area.Nombre,
                    IdPlanta = r.Area.IdPlanta,
                    Planta = new PlantaDTO
                    {
                        Id = r.Area.Planta.Id,
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
            }).FirstOrDefaultAsync();

        return Ok(recurso);
    }

    // PUT: api/Recurso/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutRecurso(int? id, Recurso recurso)
    {
        if (id != recurso.Id)        
            return BadRequest();        
        if (!_context.Estados.Any(e => e.Id == recurso.IdEstado))
            return BadRequest("El estado no existe.");
        if (!_context.Areas.Any(a => a.Id == recurso.IdArea))
            return BadRequest("El área no existe.");

        _context.Entry(recurso).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RecursoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Recurso
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Recurso>> PostRecurso(Recurso recurso)
    {
        try
        {
            if(RecursoExists(recurso.Id))            
                return BadRequest("El recurso con el mismo Id ya existe.");
            if (!_context.Estados.Any(e => e.Id == recurso.IdEstado))
                return BadRequest("El estado no existe.");
            if (!_context.Areas.Any(a => a.Id == recurso.IdArea))
                return BadRequest("El área no existe.");
            _context.Recursos.Add(recurso);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRecurso", new { id = recurso.Id }, recurso);
        } catch (DbUpdateException) 
        { 
            return BadRequest("Error al crear el recurso. Verifique los datos ingresados.");
        }
    }

    // DELETE: api/Recurso/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteRecurso(int? id)
    {
        var recurso = await _context.Recursos.FindAsync(id);
        if (recurso == null)
        {
            return NotFound();
        }

        _context.Recursos.Remove(recurso);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RecursoExists(int? id)
    {
        return _context.Recursos.Any(e => e.Id == id);
    }
}
