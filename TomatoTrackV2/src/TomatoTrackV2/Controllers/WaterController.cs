using Microsoft.AspNetCore.Mvc;
using TomatoTrackV2.Models;
using TomatoTrackV2.Services;

namespace TomatoTrackV2.Controllers;

[Route("api/[controller]")]
public class WaterController : Controller
{
    private readonly TomatoLogService _tomatoLogService;
    
    public WaterController(TomatoLogService tomatoLogService)
    {
        _tomatoLogService = tomatoLogService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _tomatoLogService.GetAllLogsAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _tomatoLogService.GetLogAsync(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TomatoLog log)
    {
        await _tomatoLogService.WriteLogAsync(log);
        return Ok(log.LogId);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] TomatoLog waterData)
    {
        await _tomatoLogService.UpdateLogAsync(id, waterData);
        return Ok(waterData);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Delete the water data with the given id
        return Ok($"Water data with id {id} deleted");
    }
}