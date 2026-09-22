using DauchMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EstadosController : ControllerBase
{
    private readonly AppDBContext _context;
    public EstadosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: api/Estado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estado>>> GetEstado()
    {
        return await _context.Estados.ToListAsync();
    }

    // GET: api/Estado/5
    [HttpGet("{id}")]

    public async Task<ActionResult<Estado>> GetEstado(int id)
    {
        var estado = await _context.Estados.FindAsync(id);

        if (estado == null)
        {
            return NotFound();
        }

        return estado;
    }

    // PUT: api/Estado/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutEstado(int? id, Estado estado)
    {
        if (id != estado.Id)
        {
            return BadRequest();
        }

        _context.Entry(estado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EstadoExists(id))
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

    // POST: api/Estado
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Estado>> PostEstado(Estado estado)
    {
        _context.Estados.Add(estado);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEstado", new { id = estado.Id }, estado);
    }

    // DELETE: api/Estado/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteEstado(int? id)
    {
        var estado = await _context.Estados.FindAsync(id);
        if (estado == null)
        {
            return NotFound();
        }

        _context.Estados.Remove(estado);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EstadoExists(int? id)
    {
        return _context.Estados.Any(e => e.Id == id);
    }
}
