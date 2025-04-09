using Kalakobana.AdminPanel.Application.Services;
using Kalakobana.AdminPanel.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kalakobana.AdminPanel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly IAdminPanelService _panelService;

        public AdminPanelController(IAdminPanelService panelService)
        {
            _panelService = panelService;
        }

        [HttpPost("insert-into-db")]
        public async Task<IActionResult> InsertAsync(DataRequest dataRequest)
        {
            if (dataRequest == null || string.IsNullOrEmpty(dataRequest.TableName) || string.IsNullOrEmpty(dataRequest.Value))
            {
                return BadRequest("Invalid request. TableName and Value are required.");
            }

            var result = await _panelService.InsertAsync(dataRequest);
            if (result)
            {
                return Ok("Data inserted successfully.");
            }
            return BadRequest("Error inserting data.");
        }

        [HttpPost("delete-from-db")]
        public async Task<IActionResult> DeleteAsync(DataRequest dataRequest)
        {
            if (dataRequest == null || string.IsNullOrEmpty(dataRequest.TableName) || string.IsNullOrEmpty(dataRequest.Value))
            {
                return BadRequest("Invalid request. TableName and Value are required.");
            }

            var result = await _panelService.DeleteAsync(dataRequest);
            if (result)
            {
                return Ok("Data deleted successfully.");
            }
            return BadRequest("Error deleting data.");
        }

        [HttpGet("get-pending")]
        public async Task<IActionResult> GetPendingData()
        {
            try
            {
                var pendingData = await _panelService.GetPendingDataAsync();
                return Ok(pendingData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching pending data: {ex.Message}");
            }
        }

        [HttpPost("process-pending-data")]
        public async Task<IActionResult> ProcessingPendingData(ProcessPendingData pendingData)
        {
            try
            {
                var result = await _panelService.ProcessPendingDataAsync(pendingData);
                if (result)
                {
                    return Ok(pendingData.ApproveStatus ? "Data approved and moved to the real table." : "Data rejected and removed from pending table.");
                }
                return BadRequest("Error processing pending data.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing pending data: {ex.Message}");
            }
        }
    }
}
