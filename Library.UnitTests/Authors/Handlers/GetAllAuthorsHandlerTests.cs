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
    public class GetAllAuthorsHandlerTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly GetAllAuthorsHandler _handler;

        public GetAllAuthorsHandlerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _handler = new GetAllAuthorsHandler(_authorServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllAuthors_AsAuthorDtoList()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Author 1" },
                new Author { Id = Guid.NewGuid(), Name = "Author 2" }
            };

            _authorServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(authors);

            var query = new GetAllAuthorsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authors.Count, result.Count());

            foreach (var authorDto in result)
            {
                var matchingAuthor = authors.First(a => a.Id == authorDto.Id);
                Assert.Equal(matchingAuthor.Name, authorDto.Name);
            }

            _authorServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }
    }
}
