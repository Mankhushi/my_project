using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;
using ClosedXML.Excel;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class NewEmailMasterController : ControllerBase
    {
        private readonly INewEmailMasterService _emailService;
        private object _service;

        public NewEmailMasterController(INewEmailMasterService emailService)
        {
            _emailService = emailService;
        }

        // ------------------------ ADD EMAIL -----------------------------

        

        // ------------------------ UPDATE EMAIL -----------------------------

        /// <summary>
        /// Updates an existing email record in the Email Master.
        /// </summary>
        /// <param name="id">The Email ID to be updated.</param>
        /// <param name="request">The updated email details.</param>
        /// <remarks>
        /// <para><b>Notes:</b></para>
        /// <para>- <c>Email</c> must remain unique across all records.</para>
        /// <para>- <c>IsActive</c> can be used to activate/deactivate the email.</para>
        /// <para>- <c>AdminId</c> identifies the user performing the update.</para>
        /// <para>- If the email does not exist, a 404 response will be returned.</para>
        /// </remarks>
        /// <returns>Returns an HTTP status code and message indicating update success or failure.</returns>
        /// <response code="200">Email updated successfully.</response>
        /// <response code="400">Invalid ID or invalid input data.</response>
        /// <response code="404">Email record not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("update-email/{id}")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] NewEmailMasterRequest request)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid Email ID." });

            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(request, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(e => e.ErrorMessage)
                });
            }

            var (statusCode, message) = await _emailService.UpdateEmailAsync(id, request);

            return StatusCode(statusCode, new { Message = message });
        }

        // ------------------------ GET EMAIL BY ID -----------------------------

        /// <summary>
        /// Retrieves a specific email record by its unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the email.</param>
        /// <returns>Email details if found.</returns>
        /// <response code="200">Email record found successfully.</response>
        /// <response code="400">Invalid ID supplied.</response>
        /// <response code="404">Email record not found.</response>
        [HttpGet("get-email/{id}")]
        public async Task<IActionResult> GetEmailById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid Email ID." });

            var data = await _emailService.GetEmailByIdAsync(id);

            if (data == null)
                return NotFound(new { Message = "Email record not found." });

            return Ok(new
            {
                Message = "Email record retrieved successfully.",
                Data = data
            });
        }

        // ------------------------ GET ALL EMAILS (Paginated) -----------------------------

        /// <summary>
        /// Retrieves a paginated list of all email records.
        /// </summary>
        /// <param name="pageNumber">The page number to fetch (default: 1).</param>
        /// <param name="pageSize">The number of records per page (default: 10).</param>
        /// <param name="searchTerm">Optional search keyword to filter email records.</param>
        /// <returns>A paginated list of email records with total count.</returns>
        /// <response code="200">Emails retrieved successfully.</response>
        /// <response code="404">No email records found.</response>
        /// <response code="500">Unexpected server error.</response>
        // -------------------------------------------------------
        // GET ALL EMAILS (Paginated + Search)
        // -------------------------------------------------------
        /// <summary>Retrieves paginated email list.</summary>
        [HttpGet("get-emails")]
        public async Task<IActionResult> GetAllEmails(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] bool? isActive = null)
        {
            var result = await _emailService.GetAllEmailsAsync(
                pageNumber,
                pageSize,
                searchTerm,
                isActive
            );

            if (result.Data.Count == 0)
                return Ok(new { Message = "No email records found.", Data = new List<object>() });

            return Ok(result);
        }


        // ------------------------ EXPORT EMAILS TO EXCEL -----------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportEmailsExcel([FromQuery] bool? isActive)
        {
            var data = await _emailService.ExportEmailsAsync(isActive);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("EmailMaster");

            // Header
            worksheet.Cell(1, 1).Value = "Email";
            worksheet.Cell(1, 2).Value = "IsActive";

            int row = 2;

            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.Email;
                worksheet.Cell(row, 2).Value = item.IsActive ? "Active" : "Inactive";
                row++;
            }

            // Auto adjust column width
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EmailMasterExport.xlsx"
            );
        }

    }
}
