using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using ParkManagerAPI.Services;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/logs")]
[Authorize]

public class LogController : ControllerBase
{
    private readonly ParkManagerContext _context;
    private readonly CustomLogger _logger;

    public LogController(ParkManagerContext context, CustomLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Récupère la liste de tous les logs (non supprimés).
    /// </summary>
    /// <returns>Liste des logs</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<Log>>> GetAll()
    {
        var logs = await _context.Logs.Where(l => l.DeletedAt == null).ToListAsync();
        return Ok(logs);
    }

    /// <summary>
    /// Récupère un log à partir de son ID.
    /// </summary>
    /// <param name="id">ID du log</param>
    /// <returns>Le log correspondant</returns>
    /// <response code="200">Log trouvé</response>
    /// <response code="404">Aucun log trouvé avec cet ID</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Log>> GetById(int id)
    {
        var log = await _context.Logs.FindAsync(id);
        if (log == null) return NotFound("Log not found !");
        
        return Ok(log);
    }

    /// <summary>
    /// Supprime logiquement un log (soft delete) via son ID.
    /// </summary>
    /// <param name="id">ID du log</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Log>> SofDeleteLog(int id)
    {
        try
        {
            var log = await _context.Logs.FindAsync(id);
            
            log.UpdatedAt = DateTime.Now;
            log.DeletedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Log",
                "Log.SofDeleteLog",
                $"Log ID {log.Id} supprimé logiquement"
            );
            
            return NoContent();
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "Log",
                "LogController.SofDeleteLog",
                $"Erreur suppression logique log ID {id} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
    }
}