using Library.Application.Authors.Commands;
using Library.Application.Authors.Handlers;
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
    public class CreateAuthorHandlerTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly CreateAuthorHandler _handler;

        public CreateAuthorHandlerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _handler = new CreateAuthorHandler(_authorServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncAndReturnsNewGuid()
        {
            // Arrange
            var command = new CreateAuthorCommand("New Author");

            Author? addedAuthor = null;
            _authorServiceMock
                .Setup(s => s.AddAsync(It.IsAny<Author>()))
                .Callback<Author>(author => addedAuthor = author)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _authorServiceMock.Verify(s => s.AddAsync(It.IsAny<Author>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(addedAuthor);
            Assert.Equal(result, addedAuthor!.Id);
            Assert.Equal(command.Name, addedAuthor.Name);
        }
    }
}
