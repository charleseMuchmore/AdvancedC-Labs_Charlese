using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppConfigController : Controller
    {
        private readonly BitsContext _context;
        public AppConfigController(BitsContext context)
        {
            _context = context;
        }
        // GET: api/AppConfigs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppConfig>>> GetAppConfigs()
        {
            if (_context.AppConfigs == null)
            {
                return NotFound();
            }
            return await _context.AppConfigs.ToListAsync();
        }

        // GET: api/AppConfigs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppConfig>> GetAppConfig(int id)
        {
            if (_context.AppConfigs == null)
            {
                return NotFound();
            }
            var appConfig = await _context.AppConfigs.FindAsync(id);

            if (appConfig == null)
            {
                return NotFound();
            }

            return appConfig;
        }

        // PUT: api/AppConfigs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppConfig(int id, AppConfig appConfig)
        {
            if (id != appConfig.BreweryId)
            {
                return BadRequest();
            }

            _context.Entry(appConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryExists(id))
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

        // POST: api/AppConfigs
        [HttpPost]
        public async Task<ActionResult<AppConfig>> PostCustomer(AppConfig appConfig)
        {
            if (_context.AppConfigs == null)
            {
                return Problem("Entity set 'bits.AppConfig'  is null.");
            }
            _context.AppConfigs.Add(appConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppConfig", new { id = appConfig.BreweryId }, appConfig);
        }
        // DELETE: api/AppConfigs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppConfig(int id)
        {
            if (_context.AppConfigs == null)
            {
                return NotFound();
            }
            var appConfig = await _context.AppConfigs.FindAsync(id);
            if (appConfig == null)
            {
                return NotFound();
            }

            _context.AppConfigs.Remove(appConfig);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BreweryExists(int id)
        {
            return (_context.AppConfigs?.Any(e => e.BreweryId == id)).GetValueOrDefault();
        }
    }
}


