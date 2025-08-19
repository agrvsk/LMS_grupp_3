using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;


namespace LMS.Presentation.Controllers
{
    [Route("/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DocumentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _serviceManager.DocumentService.GetAllDocumentsAsync();
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(string id)
        {
            var document = await _serviceManager.DocumentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentCreateDto documentDto)
        {
            if (documentDto == null)
            {
                return BadRequest("Document data is null");
            }
            var createdDocument = await _serviceManager.DocumentService.CreateDocumentAsync(documentDto);
            return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Id }, createdDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(string id, [FromBody] DocumentDto documentDto)
        {
            if (documentDto == null || id != documentDto.Id)
            {
                return BadRequest("Document data is invalid");
            }
            var updatedDocument = await _serviceManager.DocumentService.UpdateDocumentAsync(documentDto);
            if (updatedDocument == null)
            {
                return NotFound();
            }
            return Ok(updatedDocument);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            var result = await _serviceManager.DocumentService.DeleteDocumentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
