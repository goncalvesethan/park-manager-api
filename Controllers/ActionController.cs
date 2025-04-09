using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using ParkManagerAPI.Services;
using Action = ParkManagerAPI.Models.Action;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/actions")]
[Authorize]

public class ActionController : ControllerBase
{
    private readonly ParkManagerContext _context;
    private readonly CustomLogger _logger;

    public ActionController(ParkManagerContext context, CustomLogger logger)
    {
        _logger = logger;
        _context = context;
    }
    
    /// <summary>
    /// Récupère la liste de toutes les actions (non supprimées).
    /// </summary>
    /// <returns>Une liste d'actions</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<Action>>> GetAll()
    {
        var actions = await _context.Actions.Where(p => p.DeletedAt == null).ToListAsync();
        return Ok(actions);
    }
    
    /// <summary>
    /// Récupère la prochaine action en attente pour un poste à partir de son adresse MAC.
    /// </summary>
    /// <param name="macAddress">Adresse MAC du poste</param>
    /// <returns>L'action en attente, ou null si aucune</returns>
    /// <response code="200">Action trouvée</response>
    /// <response code="404">Aucune action trouvée ou poste introuvable</response>
    [AllowAnonymous]
    [HttpGet("mac/{macAddress}")]
    public async Task<ActionResult<Action>> GetDeviceAction(string macAddress)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstAsync();
            var action = await _context.Actions.Where(a => a.Status == "pending").Where(a => a.DeviceId == device.Id).FirstOrDefaultAsync();
            
            return Ok(action);
        }
        catch (Exception e)
        {
            return NotFound("Action not found !");
        }
        
        
    }
    
    /// <summary>
    /// Récupère une action à partir de son ID.
    /// </summary>
    /// <param name="id">Identifiant de l'action</param>
    /// <returns>L'action correspondante</returns>
    /// <response code="200">Action trouvée</response>
    /// <response code="404">Aucune action trouvée avec cet ID</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Action>> GetById(int id)
    {
        var action = await _context.Actions.FindAsync(id);
        if (action == null) return NotFound("Action not found !");
        
        return Ok(action);
    }
    
    /// <summary>
    /// Crée une nouvelle action.
    /// </summary>
    /// <param name="request">Données de l'action à créer</param>
    /// <returns>La nouvelle action créée</returns>
    /// <response code="201">Action créée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpPost]
    public async Task<ActionResult<Action>> CreateAction([FromBody]Action request)
    {
        try
        {
            _context.Actions.Add(request);
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Action",
                "ActionController.CreateAction",
                $"Nouvelle action '{request.Type}' créée pour le poste {request.DeviceId}"
            );
            
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Action", "ActionController.CreateAction", $"Erreur lors de la création d'une action : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
    }
    
    /// <summary>
    /// Marque comme "faite" la prochaine action en attente pour un poste donné (via son adresse MAC).
    /// </summary>
    /// <param name="macAddress">Adresse MAC du poste</param>
    /// <returns>L'action mise à jour</returns>
    /// <response code="200">Action mise à jour avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [AllowAnonymous]
    [HttpPatch("mac/{macAddress}")]
    public async Task<ActionResult<Action>> SetActionAsDone(string macAddress)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstAsync();
            var action = await _context.Actions.Where(a => a.Status == "pending").Where(a => a.DeviceId == device.Id)
                .FirstAsync();

            action.Status = "done";
            action.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            await _logger.LogAsync(
                "info",
                "Action",
                "ActionController.SetActionAsDone",
                $"Action ID {action.Id} marquée comme 'done' pour le poste MAC {macAddress}"
            );
            
            return Ok(action);
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Action", "ActionController.SetActionAsDone", $"Erreur lors de la mise à jour d'une action : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
    }

    /// <summary>
    /// Supprime logiquement une action (soft delete).
    /// </summary>
    /// <param name="id">Identifiant de l'action</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Action>> SofDeletePark(int id)
    {
        try
        {
            var action = await _context.Actions.FindAsync(id);
            
            action.UpdatedAt = DateTime.Now;
            action.DeletedAt = DateTime.Now;
            
            await _logger.LogAsync(
                "info",
                "Action",
                "ActionController.SofDeletePark",
                $"Action ID {action.Id} supprimée logiquement (soft delete)"
            );
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Action", "ActionController.SofDeletePark", $"Erreur lors de la suppression logique d'une action : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
    }
}