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
    public async Task<ActionResult<IEnumerable<AreaDTO>>> GetArea()
    {
        var areas = await _context.Areas
            .Include(p => p.Planta)
            .Select(a => new AreaDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                IdPlanta = a.IdPlanta,
                Planta = new PlantaDTO
                {
                    Id = a.Planta.Id,
                    Nombre = a.Planta.Nombre
                },                
            }).ToListAsync();

        return Ok(areas);
    }

    // GET: api/Area/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AreaDTO>> GetArea(int id)
    {        
        var area = await _context.Areas
            .Where(a => a.Id == id)
            .Include(p => p.Planta)
            .Select(a => new AreaDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                IdPlanta = a.IdPlanta,
                Planta = new PlantaDTO
                {
                    Id = a.Planta.Id,
                    Nombre = a.Planta.Nombre
                },
                Recursos = a.Recursos.Select(r => new RecursoDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    IdArea = r.IdArea
                }).ToList()
            }).FirstOrDefaultAsync();

        return Ok(area);
    }

    // PUT: api/Area/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutArea(int? id, Area area)
    {
        if (id != area.Id)
        {
            return BadRequest();
        }        
        if (_context.Areas.Any(p => p.IdPlanta == area.IdPlanta && p.Nombre == area.Nombre))
            return BadRequest("Ya existe un área con el mismo nombre en esa planta");
        if (!_context.Plantas.Any(p => p.Id == area.IdPlanta))
            return BadRequest("La planta no existe");

        _context.Entry(area).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AreaExists(id))
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

    // POST: api/Area
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Area>> PostArea(Area area)
    {
        try 
        {
            if(AreaExists(area.Id))            
                return BadRequest("El área ya existe");            
            if(_context.Areas.Any(p => p.IdPlanta == area.IdPlanta && p.Nombre == area.Nombre))            
                return BadRequest("Ya existe un área con el mismo nombre en esa planta");            
            if(!_context.Plantas.Any(p => p.Id == area.IdPlanta))
                return BadRequest("La planta no existe");

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetArea", new { id = area.Id }, area);

        } catch (DbUpdateException)       
        {
            return BadRequest("Error al crear el área");
        }       
    }

    // DELETE: api/Area/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArea(int? id)
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

    private bool AreaExists(int? id)
    {
        return _context.Areas.Any(e => e.Id == id);
    }
}
