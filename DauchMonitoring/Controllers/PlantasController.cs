using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DauchMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
using DauchMonitoring.Models.DTOs.Planta;
using DauchMonitoring.Models.DTOs.Area;

[Route("api/[controller]")]
[ApiController]
public class PlantasController : ControllerBase
{
    private readonly AppDBContext _context;

    public PlantasController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Plantas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlantaResponseDTO>>> GetPlanta(
        string? nombre)
    {
        var query = _context.Plantas.AsQueryable();
        if(!string.IsNullOrEmpty(nombre))        
            query = query.Where(p => p.Nombre.Contains(nombre));

        var plantas = await query
            .Select(p => new PlantaResponseDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Areas = p.Areas.Select(a => new AreaPatchDTO
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    IdPlanta = a.IdPlanta
                }).ToList()
            })
            .ToListAsync();

        return Ok(plantas);
    }

    // GET: api/Plantas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PlantaResponseDTO>> GetPlanta(int id)
    {
        var planta = await _context.Plantas
            .Where(p => p.Id == id)
            .Select(p => new PlantaResponseDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Areas = p.Areas.Select(a => new AreaPatchDTO
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    IdPlanta = a.IdPlanta
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (planta == null)
            return NotFound();

        return Ok(planta);
    }

    // PUT: api/Plantas/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutPlanta(int id, PlantaDTO plantaDTO)
    {
        var planta = await _context.Plantas.FindAsync(id);

        if (planta == null)
            return NotFound();

        planta.Nombre = plantaDTO.Nombre;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Ocurrió un error al actualizar la planta.");
        }

        return Ok(planta);
    }

    // POST: api/Plantas
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Planta>> PostPlanta(PlantaDTO plantaDTO)
    {
        var planta = new Planta
        {
            Nombre = plantaDTO.Nombre
        };

        try
        {
            _context.Plantas.Add(planta);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Ocurrió un error al guardar la planta.");
        }

        return CreatedAtAction(
            nameof(GetPlanta),
            new { id = planta.Id },
            planta
        );
    }

    // DELETE: api/Plantas/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePlanta(int id)
    {
        var planta = await _context.Plantas.FindAsync(id);

        if (planta == null)
            return NotFound();

        _context.Plantas.Remove(planta);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}