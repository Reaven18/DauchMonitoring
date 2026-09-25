using DauchMonitoring.Models;
using DauchMonitoring.Models.DTOs.Area;
using DauchMonitoring.Models.DTOs.Equipo;
using DauchMonitoring.Models.DTOs.Estado;
using DauchMonitoring.Models.DTOs.Planta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EquiposController : ControllerBase
{
    private readonly AppDBContext _context;
    public EquiposController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Equipo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipo(
        int? idEstado,
        int? idArea,
        int? idPlanta,
        string? nombre,
        string? modelo,
        int? cpu,
        int? memoria,
        int? puerto)
    {
        var query = _context.Equipos.AsQueryable();

        if (idEstado.HasValue)
            query = query.Where(e => e.IdEstado == idEstado.Value);

        if (idArea.HasValue)
            query = query.Where(e => e.IdArea == idArea.Value);

        if (idPlanta.HasValue)
            query = query.Where(e => e.Area.IdPlanta == idPlanta.Value);

        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(e => e.Nombre.Contains(nombre));

        if (!string.IsNullOrEmpty(modelo))
            query = query.Where(e => e.Modelo.Contains(modelo));

        if (cpu.HasValue)
            query = query.Where(e => e.Cpu >= cpu.Value);

        if (memoria.HasValue)
            query = query.Where(e => e.Memoria >= memoria.Value);

        if (puerto.HasValue)
            query = query.Where(e => e.Puerto == puerto.Value);

        var equipos = await query
            .Select(e => new EquipoDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                IdEstado = e.IdEstado,
                IdArea = e.IdArea,
                Codigo = e.Codigo,
                Modelo = e.Modelo,
                Ip = e.Ip,
                Cpu = e.Cpu,
                Memoria = e.Memoria,
                Puerto = e.Puerto,
                Heartbeat = e.Heartbeat,

                Estado = new EstadoDTO
                {
                    Id = e.Estado.Id,
                    Nombre = e.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = e.Area.Id,
                    Nombre = e.Area.Nombre,
                    IdPlanta = e.Area.IdPlanta,

                    Planta = new PlantaDTO
                    {
                        Nombre = e.Area.Planta.Nombre
                    }
                }
            })
            .ToListAsync();

        return Ok(equipos);
    }

    // GET: api/Equipo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EquipoDTO>> GetEquipo(int id)
    {
        

        var equipo = await _context.Equipos.
            Where(e => e.Id == id)
            .Select(e => new EquipoDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                IdEstado = e.IdEstado,
                IdArea = e.IdArea,
                Codigo = e.Codigo,
                Modelo = e.Modelo,
                Ip = e.Ip,
                Cpu = e.Cpu,
                Memoria = e.Memoria,
                Puerto = e.Puerto,
                Heartbeat = e.Heartbeat,

                Estado = new EstadoDTO
                {
                    Id = e.Estado.Id,
                    Nombre = e.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = e.Area.Id,
                    Nombre = e.Area.Nombre,
                    IdPlanta = e.Area.IdPlanta,

                    Planta = new PlantaDTO
                    {
                        Nombre = e.Area.Planta.Nombre
                    }
                }
            })
            .FirstOrDefaultAsync();

        if (equipo == null)
        {
            return NotFound();
        }

        return Ok(equipo);
    }

    // PUT: api/Equipo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutEquipo(int id, EquipoDTO equipoDTO)
    {
        var equipo = await _context.Equipos.FindAsync(id);

        if(equipo == null)        
            return NotFound();

        if (!await AreaExist(equipoDTO.IdArea))
            return Conflict("El área especificada no existe.");

        if (!await EstadoExist(equipoDTO.IdEstado))
            return Conflict("El estado especificado no existe.");

        if (await EquipoExists(equipoDTO.Nombre, id))
            return Conflict("El recurso con el mismo nombre ya existe.");
        
        equipo.Nombre = equipoDTO.Nombre;
        equipo.IdEstado = equipoDTO.IdEstado;
        equipo.IdArea = equipoDTO.IdArea;
        equipo.Codigo = equipoDTO.Codigo;
        equipo.Modelo = equipoDTO.Modelo;
        equipo.Ip = equipoDTO.Ip;
        equipo.Cpu = equipoDTO.Cpu;
        equipo.Memoria = equipoDTO.Memoria;
        equipo.Puerto = equipoDTO.Puerto;
        equipo.Heartbeat = equipoDTO.Heartbeat;        

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error al actualizar el recurso."); 
        }
        
    }

    // POST: api/Equipo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Equipo>> PostEquipo(EquipoDTO equipoDTO)
    {
        if(!await AreaExist(equipoDTO.IdArea))        
            return Conflict("El área especificada no existe.");        

        if(!await EstadoExist(equipoDTO.IdEstado))        
            return Conflict("El estado especificado no existe.");        
            
        if (await EquipoExists(equipoDTO.Nombre))
            return Conflict("El recurso con el mismo nombre ya existe.");

        var equipo = new Equipo
        {
            Nombre = equipoDTO.Nombre,
            IdEstado = equipoDTO.IdEstado,
            IdArea = equipoDTO.IdArea,

            Codigo = equipoDTO.Codigo,
            Modelo = equipoDTO.Modelo,
            Ip = equipoDTO.Ip,
            Cpu = equipoDTO.Cpu,
            Memoria = equipoDTO.Memoria,
            Puerto = equipoDTO.Puerto,
            Heartbeat = equipoDTO.Heartbeat
        };

        try 
        {
            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Id }, equipo);
        }
        catch (DbUpdateException ex)
        {
            return BadRequest("Error al crear el equipo.");
        }
        
    }

    // DELETE: api/Equipo/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteEquipo(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }

        _context.Equipos.Remove(equipo);        ; 
        await _context.SaveChangesAsync();

        return NoContent();
    }
   
    private async Task<bool> EquipoExists(string nombre, int id = 0)
    {
        return await _context.Recursos.AnyAsync(e => e.Nombre == nombre && e.Id != id);
    }
    private async Task<bool> AreaExist(int id) 
    {
        return await _context.Areas.AnyAsync(a => a.Id == id);
    }

    private async Task<bool> EstadoExist(int id)
    {
        return await _context.Estados.AnyAsync(e => e.Id == id);
    }

}
