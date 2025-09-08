using LMS.Shared.DTOs.EntityDto;
using LMS.Shared.DTOs.EntityDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> GetDocumentById(Guid id)
        {
            var document = await _serviceManager.DocumentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }
        [Route("{parentType}/{parentId}")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentsByParent(string parentType, Guid parentId)
        {
            var documents = await _serviceManager.DocumentService.GetDocumentsByParentAsync(parentId, parentType);
            return Ok(documents);
        }

        [HttpPost]
      public async Task<IActionResult> CreateDocument([FromForm] string documentDtoJson, IFormFile file)
       // public async Task<IActionResult> CreateDocument([FromForm] DocumentCreateDto documentDto)
        {
            if (documentDtoJson == null)
            {
                return BadRequest("Document data is null");
            }
            var documentDto = JsonConvert.DeserializeObject<DocumentCreateDto>(documentDtoJson);

            if (documentDto == null)
            {
                return BadRequest("Document data is null");
            }

            documentDto.File = file;

            using var stream = documentDto.File.OpenReadStream();
            var createdDocument = await _serviceManager.DocumentService.CreateDocumentAsync(documentDto, stream);
            return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Id }, createdDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] DocumentEditDto documentDto)
        {
            if (documentDto == null)
                return BadRequest("Document data is invalid");

            var updatedDocument = await _serviceManager.DocumentService.UpdateDocumentAsync(documentDto);
            if (updatedDocument == null)
                return NotFound();

            return Ok(updatedDocument);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] DocumentDto documentDto)
        //{
        //    if (documentDto == null || id != documentDto.Id)
        //    {
        //        return BadRequest("Document data is invalid");
        //    }
        //    var updatedDocument = await _serviceManager.DocumentService.UpdateDocumentAsync(documentDto);
        //    if (updatedDocument == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(updatedDocument);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var result = await _serviceManager.DocumentService.DeleteDocumentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUserDocuments(string id)
        {
            var result = await _serviceManager.DocumentService.DeleteUserDocumentsAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
