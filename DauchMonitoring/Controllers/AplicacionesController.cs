using DauchMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<IEnumerable<Aplicacion>>> GetAplicacion()
    {
        return await _context.Aplicaciones.ToListAsync();
    }

    // GET: api/Aplicacion/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Aplicacion>> GetAplicacion(int id)
    {
        var aplicacion = await _context.Aplicaciones.FindAsync(id);

        if (aplicacion == null)
        {
            return NotFound();
        }

        return aplicacion;
    }

    // PUT: api/Aplicacion/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutAplicacion(int? id, Aplicacion aplicacion)
    {
        if (id != aplicacion.Id)
        {
            return BadRequest();
        }

        _context.Entry(aplicacion).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AplicacionExists(id))
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

    // POST: api/Aplicacion
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Aplicacion>> PostAplicacion(Aplicacion aplicacion)
    {
        _context.Aplicaciones.Add(aplicacion);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAplicacion", new { id = aplicacion.Id }, aplicacion);
    }

    // DELETE: api/Aplicacion/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAplicacion(int? id)
    {
        var aplicacion = await _context.Aplicaciones.FindAsync(id);
        if (aplicacion == null)
        {
            return NotFound();
        }

        _context.Aplicaciones.Remove(aplicacion);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AplicacionExists(int? id)
    {
        return _context.Aplicaciones.Any(e => e.Id == id);
    }
}
