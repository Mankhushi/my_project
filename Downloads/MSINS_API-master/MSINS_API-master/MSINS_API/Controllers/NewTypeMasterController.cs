using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class NewTypeMasterController : ControllerBase
    {
        private readonly INewTypeMasterService _newTypeMasterService;

        public NewTypeMasterController(INewTypeMasterService newTypeMasterService)
        {
            _newTypeMasterService = newTypeMasterService;
        }

        //  ADD TYPE MASTER
        [HttpPost("add")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddType([FromForm] NewTypeMasterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newTypeMasterService.AddTypeAsync(request);

            if (result.IsSuccess)
                return Ok(new { Message = result.Message, TypeId = result.TypeId });
            else
                return BadRequest(new { Message = result.Message });
        }

        // UPDATE TYPE MASTER
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateType(int id, [FromBody] NewTypeMasterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.TypeId = id; // ensure ID from route is used

            var result = await _newTypeMasterService.UpdateTypeAsync(request);

            if (result.IsSuccess)
                return Ok(new { Message = result.Message });
            else
                return BadRequest(new { Message = result.Message });
        }

        //  GET TYPE MASTER BY ID
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTypeById(int id)
        {
            var typeData = await _newTypeMasterService.GetTypeByIdAsync(id);

            if (typeData == null)
                return NotFound(new { Message = $"Type Master with ID {id} not found." });

            return Ok(typeData);
        }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="pageNumber"></param>
         /// <param name="pageSize"></param>
         /// <param name="searchTerm"></param>
         /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetAllTypes(
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? searchTerm = null)
        {
            var result = await _newTypeMasterService.GetAllTypesAsync(pageNumber, pageSize, searchTerm);
            return Ok(result);
        }


    }
}
