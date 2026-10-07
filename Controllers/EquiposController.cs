using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Equipo;
using MonitoreoAPI.Models.DTOs.Estado;
using MonitoreoAPI.Models.DTOs.Planta;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;

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
    public async Task<ActionResult<IEnumerable<EquipoResponseDTO>>> GetEquipo(
        int? idEstado,
        int? idTipoRecurso,
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
            query = query.Where(e => e.Id_estado == idEstado.Value);
        
        if(idTipoRecurso.HasValue)
            query = query.Where(e => e.Id_tipo_recurso == idTipoRecurso.Value);

        if (idArea.HasValue)
            query = query.Where(e => e.Id_area == idArea.Value);

        if (idPlanta.HasValue)
            query = query.Where(e => e.Area.Id_planta == idPlanta.Value);

        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(e => e.Nombre.Contains(nombre));

        if (!string.IsNullOrEmpty(modelo))
            query = query.Where(e => e.Modelo.Contains(modelo));

        if (cpu.HasValue)
            query = query.Where(e => e.CPU >= cpu.Value);

        if (memoria.HasValue)
            query = query.Where(e => e.Memoria >= memoria.Value);

        if (puerto.HasValue)
            query = query.Where(e => e.Puerto == puerto.Value);

        var equipos = await query
            .Select(e => new EquipoResponseDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Id_estado = e.Id_estado,
                Id_area = e.Id_area,
                Id_tipo_recurso = e.Id_tipo_recurso,
                Codigo = e.Codigo,
                Modelo = e.Modelo,
                Direccion_IP = e.Direccion_IP,
                CPU = e.CPU,
                Memoria = e.Memoria,
                Puerto = e.Puerto,
                Heartbeat = e.Heartbeat,
                TipoRecurso = new TipoRecursoDTO
                {
                    Id = e.Id_tipo_recurso,
                    Nombre = e.TipoRecurso.Nombre
                },
                Estado = new EstadoDTO
                {
                    Id = e.Estado.Id,
                    Nombre = e.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = e.Area.Id,
                    Nombre = e.Area.Nombre,
                    Id_planta = e.Area.Id_planta,

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
            .Select(e => new EquipoResponseDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Id_estado = e.Id_estado,
                Id_tipo_recurso = e.Id_tipo_recurso,
                Id_area = e.Id_area,
                Codigo = e.Codigo,
                Modelo = e.Modelo,
                Direccion_IP = e.Direccion_IP,
                CPU = e.CPU,
                Memoria = e.Memoria,
                Puerto = e.Puerto,
                Heartbeat = e.Heartbeat,
                TipoRecurso = new TipoRecursoDTO
                {
                    Id = e.Id_tipo_recurso,
                    Nombre = e.TipoRecurso.Nombre
                },
                Estado = new EstadoDTO
                {
                    Id = e.Estado.Id,
                    Nombre = e.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = e.Area.Id,
                    Nombre = e.Area.Nombre,
                    Id_planta = e.Area.Id_planta,

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
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> PatchEquipo(int id, EquipoPatchDTO equipoDTO)
    {
        var equipo = await _context.Equipos.FindAsync(id);

        if (equipo == null)
            return NotFound();

        string Nombre = equipoDTO.Nombre ?? equipo.Nombre;
        int IdEstado = equipoDTO.Id_estado ?? equipo.Id_estado;
        int IdTipoRecurso = equipoDTO.Id_tipo_recurso ?? equipo.Id_tipo_recurso;
        int IdArea = equipoDTO.Id_area ?? equipo.Id_area;
        string? Codigo = equipoDTO.Codigo ?? equipo.Codigo;
        string? Modelo = equipoDTO.Modelo ?? equipo.Modelo;
        string? Direccion_IP = equipoDTO.Direccion_IP ?? equipo.Direccion_IP;
        int? CPU = equipoDTO.CPU ?? equipo.CPU;
        int? Memoria = equipoDTO.Memoria ?? equipo.Memoria;
        int? Puerto = equipoDTO.Puerto ?? equipo.Puerto;
        DateTime Heartbeat = equipoDTO.Heartbeat ?? equipo.Heartbeat;


        if (equipoDTO.Id_area != null)         
            if (!await AreaExist(IdArea))
                return Conflict("El área especificada no existe.");        

        if(equipoDTO.Id_estado != null)
            if (!await EstadoExist(IdEstado))
                return Conflict("El estado especificado no existe.");

        if (equipoDTO.Id_tipo_recurso != null)
            if (!await TipoRecursoExist(IdTipoRecurso))
                return Conflict("El tipo de recurso especificado no esxiste.");

        if (await EquipoExists(Nombre, id))
            return Conflict("El recurso con el mismo nombre ya existe.");

        equipo.Nombre = Nombre;
        equipo.Id_estado = IdEstado;
        equipo.Id_area = IdArea;
        equipo.Id_tipo_recurso = IdTipoRecurso;
        equipo.Codigo = Codigo;
        equipo.Modelo = Modelo;
        equipo.Direccion_IP = Direccion_IP;
        equipo.CPU = CPU;
        equipo.Memoria = Memoria;
        equipo.Puerto = Puerto;
        equipo.Heartbeat = Heartbeat;

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
        if (!await AreaExist(equipoDTO.Id_area))
            return Conflict("El área especificada no existe.");

        if (!await EstadoExist(equipoDTO.Id_estado))
            return Conflict("El estado especificado no existe.");

        if (!await TipoRecursoExist(equipoDTO.Id_tipo_recurso))
            return Conflict("El tipo de recurso especificado no existe.");

        if (await EquipoExists(equipoDTO.Nombre))
            return Conflict("El recurso con el mismo nombre ya existe.");

        var equipo = new Equipo
        {
            Nombre = equipoDTO.Nombre,
            Id_estado = equipoDTO.Id_estado,
            Id_tipo_recurso = equipoDTO.Id_tipo_recurso,
            Id_area = equipoDTO.Id_area,
            Codigo = equipoDTO.Codigo,
            Modelo = equipoDTO.Modelo,
            Direccion_IP = equipoDTO.Direccion_IP,
            CPU = equipoDTO.CPU,
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
            return NotFound();
        
        _context.Equipos.Remove(equipo); ;
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
    private async Task<bool> TipoRecursoExist(int id)
    {
        return await _context.Tipo_Recursos.AnyAsync(t => t.Id == id);
    }

}