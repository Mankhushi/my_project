using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class NewContactMasterController : ControllerBase
    {
        private readonly INewContactMasterService _contactService;

        public NewContactMasterController(INewContactMasterService contactService)
        {
            _contactService = contactService;
        }

        //-----------------------------------UPDATE--------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateContact([FromBody] NewContactMasteRequest model)
        {
            if (model.ContactID <= 0)
                return BadRequest(new { message = "ContactID is required for update." });

            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = results.Select(e => e.ErrorMessage)
                });
            }

            var (statusCode, message) = await _contactService.UpdateContactAsync(model);

            return StatusCode(statusCode, new { Message = message });
        }



        //-----------------------------------GET BY ID--------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet("get/{contactId}")]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            var result = await _contactService.GetContactByIdAsync(contactId);

            if (result == null)
                return NotFound(new { message = "Contact not found." });

            return Ok(result);
        }
    }
}
