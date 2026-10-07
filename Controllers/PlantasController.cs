using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoreoAPI.Models;
using MonitoreoAPI.Models.DTOs.Area;
using MonitoreoAPI.Models.DTOs.Planta;
using System.Runtime.InteropServices;

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
    public async Task<ActionResult<IEnumerable<PlantaResponseDTO>>> GetPlanta(
        string? nombre)
    {
        var query = _context.Plantas.AsQueryable();

        if (!string.IsNullOrEmpty(nombre))
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
                    Id_planta = a.Id_planta,
                }).ToList()
            }).ToListAsync();
        return Ok(plantas);
    }

    // GET: api/Planta/5
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
                    Id_planta = a.Id_planta,
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
    public async Task<IActionResult> PutPlanta(int id, PlantaDTO plantaDTO)
    {
        var planta = await _context.Plantas.FindAsync(id);

        if (planta == null)
            return NotFound("No se encontro la planta");
        if (await PlantaExists(plantaDTO.Nombre, id))
            return Conflict("Ya existe una planta con el mismo nombre");

        planta.Nombre = plantaDTO.Nombre;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(planta);
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error al actualizar la planta");
        }

    }

    // POST: api/Planta
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PlantaDTO>> PostPlanta(PlantaDTO plantaDTO)
    {
        if (await PlantaExists(plantaDTO.Nombre))
            return Conflict("Ya existe una planta con el mismo nombre");
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
            return BadRequest("Ocurrió un error al crear la planta.");
        }

        return CreatedAtAction(nameof(GetPlanta), new { id = planta.Id }, plantaDTO);
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

    private async Task<bool> PlantaExists(string Nombre, int id = 0)
    {
        return await _context.Plantas.AnyAsync(p => p.Nombre == Nombre && p.Id == id);
    }   
}