using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs;
using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Estado;
using MonitoreoAPI.Models.DTOs.Planta;
using MonitoreoAPI.Models.DTOs.Tipo_recurso;

[Route("api/[controller]")]
[ApiController]
public class AreasController : ControllerBase
{
    private readonly AppDBContext _context;
    public AreasController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Area
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AreaResponseDTO>>> GetArea(
        int? idPlanta,
        string? nombre)
    {
        var query = _context.Areas.AsQueryable();

        if (idPlanta.HasValue)
            query = query.Where(a => a.Id_planta == idPlanta.Value);
        if (!string.IsNullOrEmpty(nombre))
            query = query.Where(a => a.Nombre.StartsWith(nombre));

        var areas = await query
            .Select(a => new AreaResponseDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Id_planta = a.Id_planta,
                Planta = a.Planta == null ?
                null : new PlantaDTO
                {
                    Nombre = a.Planta.Nombre
                },
                Recursos = a.Recursos.Select(r => new RecursoPatchDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Id_estado = r.Id_estado,
                    Id_area = r.Id_area,
                    Estado = new EstadoDTO 
                    {
                        Id = r.Id_estado,
                        Nombre = r.Estado.Nombre,
                        Descripcion = r.Estado.Descripcion
                    },
                    Id_tipo_recurso = r.Id_tipo_recurso,
                    TipoRecurso = new TipoRecursoDTO
                    {
                        Id = r.Id_tipo_recurso,
                        Nombre = r.TipoRecurso.Nombre
                    },
                }).ToList()
            })
            .ToListAsync();

        return Ok(areas);
    }

    // GET: api/Area/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AreaResponseDTO>> GetArea(int id)
    {
        var area = await _context.Areas
            .Where(a => a.Id == id)
            .Select(a => new AreaResponseDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Id_planta = a.Id_planta,
                Planta = a.Planta == null ?
                null : new PlantaDTO
                {
                    Nombre = a.Planta.Nombre
                },
                Recursos = a.Recursos.Select(r => new RecursoPatchDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Id_estado = r.Id_estado,
                    Id_area = r.Id_area,
                     Estado = new EstadoDTO
                     {
                         Id = r.Id_estado,
                         Nombre = r.Estado.Nombre,
                         Descripcion = r.Estado.Descripcion
                     },
                      TipoRecurso = new TipoRecursoDTO
                      {
                          Id = r.Id_tipo_recurso,
                          Nombre = r.TipoRecurso.Nombre
                      },
                })
                .ToList()
            })
            .FirstOrDefaultAsync();

        if (area == null)
            return NotFound();

        return Ok(area);
    }

    // PUT: api/Area/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutArea(int id, AreaDTO areaDTO)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null)
            return NotFound();

        if (!await PlantaExist(areaDTO.Id_planta))
            return NotFound("La planta no existe");        
        if (await AreaExists(areaDTO.Nombre, area.Id_planta, area.Id))
            return Conflict("Ya existe un área con el mismo nombre en esa planta");

        area.Id_planta = areaDTO.Id_planta;
        area.Nombre = areaDTO.Nombre;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(NuevaArea(area));
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error al actualizar el área");
        }
    }

    // PATCH: api/Area/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> PatchArea(int id, AreaPatchDTO areaDTO)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null)
            return NotFound();

        int idPlanta = areaDTO.Id_planta ?? area.Id_planta;
        string nombre = areaDTO.Nombre ?? area.Nombre;

        if (areaDTO.Id_planta != null)
        {
            if (!await PlantaExist(areaDTO.Id_planta))
                return NotFound("La planta no existe");
        }

        if (await AreaExists(nombre, idPlanta, area.Id))
            return Conflict("Ya existe un área con el mismo nombre en esa planta");

        area.Id_planta = idPlanta;
        area.Nombre = nombre;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(NuevaArea(area));
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error al actualizar el área");
        }
    }

    // POST: api/Area
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AreaDTO>> PostArea(AreaDTO area)
    {
        try
        {
            var newArea = new Area
            {
                Nombre = area.Nombre,
                Id_planta = area.Id_planta
            };
            if (!await PlantaExist(area.Id_planta))
                return NotFound("La planta no existe");
            if (await AreaExists(area.Nombre, area.Id_planta))
                return Conflict("Ya existe un área con el mismo nombre en esa planta");

            _context.Areas.Add(newArea);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetArea), NuevaArea(newArea));

        }
        catch (DbUpdateException)
        {
            return BadRequest("Error al crear el área");
        }
    }

    // DELETE: api/Area/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArea(int id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null)
        {
            return NotFound();
        }

        _context.Areas.Remove(area);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AreaExists(string nombre, int idPlanta, int? IdArea = null)
    {
        return await _context.Areas.AnyAsync(e => e.Nombre == nombre && e.Id_planta == idPlanta && e.Id != IdArea);
    }

    private async Task<bool> PlantaExist(int? id)
    {
        return await _context.Plantas.AnyAsync(p => p.Id == id);
    }

    public static AreaPatchDTO NuevaArea(Area a)
    {
        return new AreaPatchDTO
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Id_planta = a.Id_planta
        };
    }

}