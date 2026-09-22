using DauchMonitoring.Models;
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
    public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipo()
    {
        return await _context.Equipos.ToListAsync();
    }

    // GET: api/Equipo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Equipo>> GetEquipo(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);

        if (equipo == null)
        {
            return NotFound();
        }

        return equipo;
    }

    // PUT: api/Equipo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutEquipo(int? id, Equipo equipo)
    {
        if (id != equipo.Id)
        {
            return BadRequest();
        }

        _context.Entry(equipo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EquipoExists(id))
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

    // POST: api/Equipo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Equipo>> PostEquipo(Equipo equipo)
    {
        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEquipo", new { id = equipo.Id }, equipo);
    }

    // DELETE: api/Equipo/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteEquipo(int? id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }

        _context.Equipos.Remove(equipo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EquipoExists(int? id)
    {
        return _context.Equipos.Any(e => e.Id == id);
    }
}
