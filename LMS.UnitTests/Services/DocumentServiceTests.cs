using Domain.Models.Entities;
using LMS.Services;
using LMS.Shared.DTOs.EntityDto;
using LMS.UnitTests.Setups;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class DocumentServiceTests :ServiceTestBase
    {
        private readonly DocumentService _service;

        public DocumentServiceTests()
        {
            MockUow.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
            _service = new DocumentService(MockUow.Object, MockMapper.Object);
        }

        #region [GetDocumentByIdAsync]
        [Fact]
        [Trait("DocumentService", "Get Document")]
        public async Task GetDocumentByIdAsync_DocumentExists_ReturnsDocument()
        {
            var documentId = Guid.NewGuid();
            var document = new Document { Id = documentId, Name = "TestDoc" };
            var documentDto = new DocumentDto { Id = documentId, Name = "TestDoc" };

            MockDocumentRepo
                .Setup(r => r.GetDocumentByIdAsync(documentId))
                .ReturnsAsync(document);

            MockMapper
                .Setup(m => m.Map<DocumentDto>(document))
                .Returns(documentDto);

            var result = await _service.GetDocumentByIdAsync(documentId);

            Assert.NotNull(result);
            Assert.Equal(documentId, result.Id);
            Assert.Equal("TestDoc", result.Name);
        }

        [Fact]
        [Trait("DocumentService", "Get Document")]
        public async Task GetDocumentByIdAsync_DocumentDoesNotExist_ReturnsNull()
        {
            var documentId = Guid.NewGuid();

            MockDocumentRepo
                .Setup(r => r.GetDocumentByIdAsync(documentId))
                .ReturnsAsync((Document?)null);

            MockMapper
                .Setup(m => m.Map<DocumentDto>(It.IsAny<Document>()))
                .Returns((DocumentDto?)null);

            var result = await _service.GetDocumentByIdAsync(documentId);

            Assert.Null(result);
        }
        #endregion

        #region [GetAllDocumentsAsync]
        [Fact]
        [Trait("DocumentService", "Get All Documents")]
        public async Task GetAllDocumentsAsync_DocumentsExist_ReturnsList()
        {
            var documents = new List<Document>
            {
                new Document { Id = Guid.NewGuid(), Name = "Doc1" },
                new Document { Id = Guid.NewGuid(), Name = "Doc2" }
            };

            var documentDtos = new List<DocumentDto>
            {
                new DocumentDto { Id = documents[0].Id, Name = "Doc1" },
                new DocumentDto { Id = documents[1].Id, Name = "Doc2" }
            };

            MockDocumentRepo.Setup(r => r.GetAllDocumentsAsync()).ReturnsAsync(documents);

            MockMapper
                .Setup(m => m.Map<List<DocumentDto>>(documents))
                .Returns(documentDtos);

            var result = await _service.GetAllDocumentsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        [Trait("DocumentService", "Get All Documents")]
        public async Task GetAllDocumentsAsync_NoDocuments_ReturnsEmptyList()
        {
            var emptyDocs = new List<Document>();
            var emptyDtos = new List<DocumentDto>();

            MockDocumentRepo.Setup(r => r.GetAllDocumentsAsync()).ReturnsAsync(emptyDocs);

            MockMapper
                .Setup(m => m.Map<List<DocumentDto>>(emptyDocs))
                .Returns(emptyDtos);

            var result = await _service.GetAllDocumentsAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        #endregion

        #region [GetDocumentsByParentAsync]
        [Fact]
        [Trait("DocumentService", "Get Documents By Parent")]
        public async Task GetDocumentsByParentAsync_DocumentsExist_ReturnsList()
        {
            var parentId = Guid.NewGuid();
            var parentType = "Course";

            var documents = new List<Document>
            {
                new Document { Id = Guid.NewGuid(), Name = "ParentDoc", ParentId = parentId, ParentType = parentType }
            };

            var documentDtos = new List<DocumentDto>
            {
                new DocumentDto { Id = documents[0].Id, Name = "ParentDoc" }
            };

            MockDocumentRepo.Setup(r => r.GetDocumentsByParentAsync(parentId, parentType))
                            .ReturnsAsync(documents);

            MockMapper
                .Setup(m => m.Map<List<DocumentDto>>(documents))
                .Returns(documentDtos);

            var result = await _service.GetDocumentsByParentAsync(parentId, parentType);

            Assert.Single(result);
            Assert.Equal("ParentDoc", result.First().Name);
        }
        #endregion

        #region [CreateDocumentAsync]
        [Fact]
        [Trait("DocumentService", "Create Document")]
        public async Task CreateDocumentAsync_ValidDto_CreatesAndReturnsEntity()
        {
            var createDto = new DocumentCreateDto { Name = "NewDoc" };
            var mappedEntity = new Document { Id = Guid.NewGuid(), Name = "NewDoc" };

            MockMapper
                .Setup(m => m.Map<Document>(createDto))
                .Returns(mappedEntity);

            var result = await _service.CreateDocumentAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal("NewDoc", result.Name);
            MockDocumentRepo.Verify(r => r.Create(mappedEntity), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }
        #endregion

        #region [UpdateDocumentAsync]
        [Fact]
        [Trait("DocumentService", "Update Document")]
        public async Task UpdateDocumentAsync_ValidDto_UpdatesAndReturnsEntity()
        {
            var dto = new DocumentDto { Id = Guid.NewGuid(), Name = "UpdatedDoc" };
            var mappedEntity = new Document { Id = dto.Id, Name = dto.Name };

            MockMapper
                .Setup(m => m.Map<Document>(dto))
                .Returns(mappedEntity);

            var result = await _service.UpdateDocumentAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("UpdatedDoc", result.Name);
            MockDocumentRepo.Verify(r => r.Update(mappedEntity), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }
        #endregion

        #region [DeleteDocumentAsync]
        [Fact]
        [Trait("DocumentService", "Delete Document")]
        public async Task DeleteDocumentAsync_DocumentDoesNotExist_ReturnsFalse()
        {
            var documentId = Guid.NewGuid();

            MockDocumentRepo.Setup(r => r.GetDocumentByIdAsync(documentId))
                            .ReturnsAsync((Document?)null);

            var result = await _service.DeleteDocumentAsync(documentId);

            Assert.False(result);
            MockDocumentRepo.Verify(r => r.Delete(It.IsAny<Document>()), Times.Never);
            MockUow.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        [Trait("DocumentService", "Delete Document")]
        public async Task DeleteDocumentAsync_DocumentExists_DeletesAndReturnsTrue()
        {
            var documentId = Guid.NewGuid();
            var document = new Document { Id = documentId, Name = "ToDelete" };

            MockDocumentRepo.Setup(r => r.GetDocumentByIdAsync(documentId))
                            .ReturnsAsync(document);

            var result = await _service.DeleteDocumentAsync(documentId);

            Assert.True(result);
            MockDocumentRepo.Verify(r => r.Delete(document), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }
        #endregion
    }
}
