using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs.Aplicacion;
using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Estado;
using MonitoreoAPI.Models.DTOs.Planta;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;
using System.Runtime.CompilerServices;


[Route("api/[controller]")]
[ApiController]
public class AplicacionesController : ControllerBase
{
    private readonly AppDBContext _context;
    public AplicacionesController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Aplicacion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AplicacionResponseDTO>>> GetAplicacion(
         int? idEstado,
        int? idArea,
        int? idPlanta,
        string? nombre)
    {
        var query = _context.Aplicaciones.AsQueryable();

        if (idEstado.HasValue)
            query = query.Where(e => e.Id_estado == idEstado.Value);

        if (idArea.HasValue)
            query = query.Where(e => e.Id_area == idArea.Value);

        if (idPlanta.HasValue)
            query = query.Where(e => e.Area.Id_planta == idPlanta.Value);

        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(e => e.Nombre.Contains(nombre));

        var aplicaciones = await query
            .Select(a => new AplicacionResponseDTO 
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Id_estado = a.Id_estado,
                Id_area = a.Id_area,
                Id_tipo_recurso = a.Id_tipo_recurso,
                Codigo = a.Codigo,
                Last_update = a.Last_update,
                TipoRecurso = new TipoRecursoDTO
                {
                    Id = a.Id_tipo_recurso,
                    Nombre = a.TipoRecurso.Nombre
                },
                Estado = new EstadoDTO
                {
                    Id = a.Estado.Id,
                    Nombre = a.Estado.Nombre
                },

                Area = new AreaResponseDTO
                {
                    Id = a.Area.Id,
                    Nombre = a.Area.Nombre,
                    Id_planta = a.Area.Id_planta,

                    Planta = new PlantaDTO
                    {
                        Nombre = a.Area.Planta.Nombre
                    }
                }
            }).ToListAsync();

        return Ok(aplicaciones);
    }

    // GET: api/Aplicacion/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AplicacionResponseDTO>> GetAplicacion(int id)
    {
        var aplicacion = await _context.Aplicaciones
            .Where(a => a.Id == id)
            .Select(a => new AplicacionResponseDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Id_estado = a.Id_estado,
                Id_tipo_recurso = a.Id_tipo_recurso,
                Id_area = a.Id_area,
                Codigo = a.Codigo,
                Last_update = a.Last_update,
                Estado = new EstadoDTO
                {
                    Id = a.Estado.Id,
                    Nombre = a.Estado.Nombre
                },
                TipoRecurso = new TipoRecursoDTO
                {
                    Id = a.Id_tipo_recurso,
                    Nombre = a.TipoRecurso.Nombre
                },
                Area = new AreaResponseDTO
                {
                    Id = a.Area.Id,
                    Nombre = a.Area.Nombre,
                    Id_planta = a.Area.Id_planta,

                    Planta = new PlantaDTO
                    {
                        Nombre = a.Area.Planta.Nombre
                    }
                }
            }).FirstOrDefaultAsync();

        if (aplicacion == null)        
            return NotFound();        

        return Ok(aplicacion);
    }

    // PUT: api/Aplicacion/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> PatchAplicacion(int id, AplicacionPatchDTO aplicacionDTO)
    {
        var aplicacion = await _context.Aplicaciones.FindAsync(id);

        if (aplicacion == null)
            return NotFound();

        string Nombre = aplicacionDTO.Nombre ?? aplicacion.Nombre;
        int IdEstado = aplicacionDTO.Id_estado ?? aplicacion.Id_estado;
        int IdTipoRecurso = aplicacionDTO.Id_tipo_recurso ?? aplicacion.Id_tipo_recurso;
        int IdArea = aplicacionDTO.Id_area ?? aplicacion.Id_area;
        string? Codigo = aplicacionDTO.Codigo ?? aplicacion.Codigo;
        DateTime last_update = DateTime.Now;


        if (aplicacionDTO.Id_area != null)
            if (!await AreaExist(IdArea))
                return Conflict("El área especificada no existe.");

        if (aplicacionDTO.Id_estado != null)
            if (!await EstadoExist(IdEstado))
                return Conflict("El estado especificado no existe.");

        if (aplicacionDTO.Id_tipo_recurso != null)
            if (!await TipoRecursoExist(IdTipoRecurso))
                return Conflict("El recurso especificado no existe");
            

        if (await AplicacionExists(Nombre, id))
            return Conflict("El recurso con el mismo nombre ya existe.");

        aplicacion.Nombre = Nombre;
        aplicacion.Id_estado = IdEstado;
        aplicacion.Id_area = IdArea;
        aplicacion.Id_tipo_recurso = IdTipoRecurso;
        aplicacion.Codigo = Codigo;  
        aplicacion.Last_update = last_update;

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

    // POST: api/Aplicacion
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Aplicacion>> PostAplicacion(AplicacionDTO aplicacionDTO)
    {
        if (!await AreaExist(aplicacionDTO.Id_area))
            return Conflict("El área especificada no existe.");

        if (!await EstadoExist(aplicacionDTO.Id_estado))
            return Conflict("El estado especificado no existe.");
        
        if (!await TipoRecursoExist(aplicacionDTO.Id_tipo_recurso))
            return Conflict("El tipo de recurso especificado no existe");

        if (await AplicacionExists(aplicacionDTO.Nombre))
            return Conflict("El recurso con el mismo nombre ya existe.");

        var aplicacion = new Aplicacion
        {
            Nombre = aplicacionDTO.Nombre,
            Id_estado = aplicacionDTO.Id_estado,
            Id_area = aplicacionDTO.Id_area,
            Id_tipo_recurso = aplicacionDTO.Id_tipo_recurso,

            Codigo = aplicacionDTO.Codigo,
            Last_update = aplicacionDTO.Last_update,
        };

        try
        {
            _context.Aplicaciones.Add(aplicacion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAplicacion), new { id = aplicacion.Id }, aplicacion);
        }
        catch (DbUpdateException ex)
        {
            return BadRequest("Error al crear el registro de la aplicación.");
        }
    }

    // DELETE: api/Aplicacion/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAplicacion(int id)
    {
        var aplicacion = await _context.Aplicaciones.FindAsync(id);
        if (aplicacion == null)        
            return NotFound();        

        _context.Aplicaciones.Remove(aplicacion);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AplicacionExists(string nombre, int id = 0)
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
