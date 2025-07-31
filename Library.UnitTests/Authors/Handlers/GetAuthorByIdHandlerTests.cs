using Library.Application.Authors.Handlers;
using Library.Application.Authors.Queries;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Authors.Handlers
{
    public class GetAuthorByIdHandlerTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly GetAuthorByIdHandler _handler;

        public GetAuthorByIdHandlerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _handler = new GetAuthorByIdHandler(_authorServiceMock.Object);
        }

        [Fact]
        public async Task Handle_AuthorExists_ReturnsAuthorDto()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author
            {
                Id = authorId,
                Name = "Test Author"
            };

            _authorServiceMock.Setup(s => s.GetByIdAsync(authorId)).ReturnsAsync(author);

            var query = new GetAuthorByIdQuery(authorId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorId, result.Id);
            Assert.Equal(author.Name, result.Name);
            _authorServiceMock.Verify(s => s.GetByIdAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task Handle_AuthorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorServiceMock.Setup(s => s.GetByIdAsync(authorId)).ReturnsAsync((Author?)null);

            var query = new GetAuthorByIdQuery(authorId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _authorServiceMock.Verify(s => s.GetByIdAsync(authorId), Times.Once);
        }
    }
}
