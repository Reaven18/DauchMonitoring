using DauchMonitoring.Models;
using DauchMonitoring.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<IEnumerable<AreaResponseDTO>>> GetArea()
    {
        var areas = await _context.Areas
            .Include(p => p.Planta)
            .Select(a => NuevaArea(a)).ToListAsync();

        return Ok(areas);
    }

    // GET: api/Area/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AreaResponseDTO>> GetArea(int id)
    {        
        var area = await _context.Areas
            .Where(a => a.Id == id)
            .Include(p => p.Planta)
            .Include(r => r.Recursos)
            .Select(a => NuevaArea(a))
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
        
        
        if (!await PlantaExist(areaDTO.IdPlanta))
            return NotFound("La planta no existe");
        else
            area.IdPlanta = (int)areaDTO.IdPlanta;        
        if(await AreaExists(areaDTO.Nombre, area.IdPlanta, area.Id))
            return Conflict("Ya existe un área con el mismo nombre en esa planta");     
        else
            area.Nombre = areaDTO.Nombre;       
        
        try
        {
            await _context.SaveChangesAsync();
            return Ok(area);
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

        int idPlanta = areaDTO.IdPlanta ?? area.IdPlanta;
        string nombre = areaDTO.Nombre ?? area.Nombre;

        if (areaDTO.IdPlanta != null)
        {
            if (!await PlantaExist(areaDTO.IdPlanta))
                return NotFound("La planta no existe");            
        }
        
        if (await AreaExists(nombre, idPlanta, area.Id))
                return Conflict("Ya existe un área con el mismo nombre en esa planta");                                

        area.IdPlanta = idPlanta;
        area.Nombre = nombre;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(area);
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
                IdPlanta = area.IdPlanta
            };
            if (!await PlantaExist(area.IdPlanta))
                return NotFound("La planta no existe");
            if (await AreaExists(area.Nombre, area.IdPlanta))            
                return Conflict("Ya existe un área con el mismo nombre en esa planta"); 

            _context.Areas.Add(newArea);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetArea), new { id = newArea.Id },area);

        } catch (DbUpdateException)       
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
        return await _context.Areas.AnyAsync(e => e.Nombre == nombre && e.IdPlanta == idPlanta && e.Id != IdArea);
    }

    private async Task<bool> PlantaExist(int? id)
    {
        return await _context.Plantas.AnyAsync(p => p.Id == id);
    }

    public static AreaResponseDTO NuevaArea(Area a) 
    {
        return new AreaResponseDTO
        {
            Id = a.Id,
            Nombre = a.Nombre,
            IdPlanta = a.IdPlanta,
            Planta = a.Planta == null ?
            null : new PlantaDTO
            {                
                Nombre = a.Planta.Nombre
            },
            Recursos = a.Recursos == null ?
            null : a.Recursos.Select(r => new RecursoDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                IdArea = r.IdArea
            }).ToList()
        };
    }
}
