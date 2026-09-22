using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DauchMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
using DauchMonitoring.Models.DTOs;

[Route("api/[controller]")]
[ApiController]
public class PlantasController : ControllerBase
{
    private readonly AppDBContext _context;
    public PlantasController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Planta
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlantaDTO>>> GetPlanta()
    {        
        var plantas = await _context.Plantas            
            .Select(p => new PlantaDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Areas = p.Areas.Select(a => new AreaDTO
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    IdPlanta = a.IdPlanta,
                }).ToList()
            }).ToListAsync();
        return Ok(plantas);
    }

    // GET: api/Planta/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PlantaDTO>> GetPlanta(int id)
    {
        var planta = await _context.Plantas
            .Where(p => p.Id == id)
            .Select(p => new PlantaDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Areas = p.Areas.Select(a => new AreaDTO
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    IdPlanta = a.IdPlanta,
                }).ToList()
            }).FirstOrDefaultAsync();

        if (planta == null)       
            return NotFound();        

        return Ok(planta);
    }

    // PUT: api/Planta/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutPlanta(int? id, Planta planta)
    {
        if (id != planta.Id)
        {
            return BadRequest();
        }

        _context.Entry(planta).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlantaExists(id))
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

    // POST: api/Planta
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Planta>> PostPlanta(Planta planta)
    {
        try
        {
            if(PlantaExists(planta.Id))
            {
                return BadRequest("Ya existe una planta con ese ID.");
            }
            _context.Plantas.Add(planta);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException) 
        {
            return BadRequest("Ocurrió un error al guardar la planta.");
        }

        return CreatedAtAction("GetPlanta", new { id = planta.Id }, planta);
    }

    // DELETE: api/Planta/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePlanta(int? id)
    {
        var planta = await _context.Plantas.FindAsync(id);
        if (planta == null)
        {
            return NotFound();
        }

        _context.Plantas.Remove(planta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlantaExists(int? id)
    {
        return _context.Plantas.Any(e => e.Id == id);
    }
}
