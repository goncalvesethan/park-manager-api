using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using ParkManagerAPI.Services;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/devices")]
[Authorize]

public class DeviceController : ControllerBase
{
    private readonly ParkManagerContext _context;
    private readonly CustomLogger _logger;

    public DeviceController(ParkManagerContext context, CustomLogger logger)
    {
        _context = context;
        _logger = logger;
    }
    
    /// <summary>
    /// Récupère la liste de tous les postes (non supprimés).
    /// </summary>
    /// <returns>Une liste de postes</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<Device>>> GetAll()
    {
        var devices = await _context.Devices.Where(d => d.DeletedAt == null).ToListAsync();
        return Ok(devices);
    }

    /// <summary>
    /// Récupère un poste à partir de son ID.
    /// </summary>
    /// <param name="id">ID du poste</param>
    /// <returns>Le poste correspondant</returns>
    /// <response code="200">Poste trouvé</response>
    /// <response code="404">Aucun poste trouvé avec cet identifiant</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetById(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return NotFound("Device not found !");
        
        return Ok(device);
    }
    
    /// <summary>
    /// Crée un nouveau poste.
    /// </summary>
    /// <param name="request">Données du poste à créer</param>
    /// <returns>Le poste nouvellement créé</returns>
    /// <response code="201">Poste créé avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpPost]
    public async Task<ActionResult<Device>> CreateDevice([FromBody]Device request)
    {
        try
        {
            _context.Devices.Add(request);
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Device",
                "DeviceController.CreateDevice",
                $"Nouveau poste créé : {request.MacAddress}"
            );
            
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "Device",
                "DeviceController.CreateDevice",
                $"Erreur lors de la création d'un poste : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
    }
    
    /// <summary>
    /// Met à jour un poste existant à partir de son ID.
    /// </summary>
    /// <param name="id">ID du poste à mettre à jour</param>
    /// <param name="request">Nouvelles données du poste</param>
    /// <returns>Le poste mis à jour</returns>
    /// <response code="200">Poste mis à jour avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<Device>> UpdateDevice(int id, [FromBody] Device request)
    {
        try
        {
            var device = await _context.Devices.FindAsync(id);
            device.ParkId = request.ParkId;
            device.RoomId = request.RoomId;
            device.MacAddress = request.MacAddress;
            device.Name = request.Name ?? null;
            device.Brand = request.Brand ?? null;
            device.Processor = request.Processor ?? null;
            device.RAM = request.RAM ?? null;
            device.Storage = request.Storage ?? null;
            device.IpAddress = request.IpAddress ?? null;
            device.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Device",
                "DeviceController.UpdateDevice",
                $"Informations mises à jour pour le poste {device.Id}"
            );
            
            return Ok(device);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "Device",
                "DeviceController.UpdateDevice(mac)",
                $"Erreur lors de la mise à jours des informations pour le poste ID {id} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    /// <summary>
    /// Met hors ligne un poste via son adresse MAC (adresse IP mise à null).
    /// </summary>
    /// <param name="macAddress">Adresse MAC du poste</param>
    /// <returns>Le poste mis à jour</returns>
    /// <response code="200">Poste mis hors ligne avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [AllowAnonymous]
    [HttpPatch("mac/{macAddress}/offline")]
    public async Task<ActionResult<Device>> SetDeviceOffline(String macAddress)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstOrDefaultAsync();

            device.IpAddress = null;
            device.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            await _logger.LogAsync(
                "info",
                "Device",
                "DeviceController.SetDeviceOffline",
                $"Poste ID {device.Id} (MAC {macAddress}) mis hors ligne"
            );
            
            return Ok(device);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "Device",
                "DeviceController.SetDeviceOffline",
                $"Erreur lors de la mise hors ligne du poste MAC {macAddress} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
    }
    
    /// <summary>
    /// Met à jour un poste via son adresse MAC.
    /// </summary>
    /// <param name="macAddress">Adresse MAC du poste</param>
    /// <param name="request">Nouvelles données du poste</param>
    /// <returns>Le poste mis à jour</returns>
    /// <response code="200">Poste mis à jour avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [AllowAnonymous]
    [HttpPut("mac/{macAddress}")]
    public async Task<ActionResult<Device>> UpdateDevice(string macAddress, [FromBody] Device request)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstOrDefaultAsync();
            device.MacAddress = request.MacAddress;
            device.Brand = request.Brand ?? null;
            device.Processor = request.Processor ?? null;
            device.RAM = request.RAM ?? null;
            device.Storage = request.Storage ?? null;
            device.IpAddress = request.IpAddress ?? null;
            device.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Device",
                "DeviceController.UpdateDevice(mac)",
                $"Informations remontées pour le poste {device.Id} (MAC {macAddress})"
            );
            
            return Ok(device);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "Device",
                "DeviceController.UpdateDevice(mac)",
                $"Erreur lors de la remontée d'infos pour le poste MAC {macAddress} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
        
    }
    
    /// <summary>
    /// Supprime logiquement un poste (soft delete) en utilisant son ID.
    /// </summary>
    /// <param name="id">ID du poste</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Device>> SofDeleteDevice(int id)
    {
        try
        {
            var device = await _context.Devices.FindAsync(id);
            
            device.UpdatedAt = DateTime.Now;
            device.DeletedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Device",
                "DeviceController.SofDeleteDevice",
                $"Poste ID {device.Id} supprimé logiquement (soft delete)"
            );
            
            return NoContent();
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "Device",
                "DeviceController.SofDeleteDevice",
                $"Erreur lors de la suppression logique du poste ID {id} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
    }
}