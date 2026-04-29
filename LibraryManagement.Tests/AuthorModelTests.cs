using FluentAssertions;
using LibraryManagement.Domain.Entities;
using Xunit;

namespace LibraryManagement.Tests
{
    public class AuthorModelTests
    {
        [Fact]
        public void Author_Properties_AreSetCorrectly()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Austen",
                Biography = "English novelist known for Pride and Prejudice.",
                DateOfBirth = new DateOnly(1775, 12, 16)
            };

            author.Id.Should().Be(1);
            author.FirstName.Should().Be("Jane");
            author.LastName.Should().Be("Austen");
            author.Biography.Should().Be("English novelist known for Pride and Prejudice.");
            author.DateOfBirth.Should().Be(new DateOnly(1775, 12, 16));
            author.FullName.Should().Be("Jane Austen");
            author.Books.Should().BeEmpty();
        }
        [Fact]
        public void Author_OptionalFields_CanBeNull()
        {
            var author = new Author
            {
                FirstName = "Mark",
                LastName = "Twain"
            };
            author.Biography.Should().BeNull();
            author.DateOfBirth.Should().BeNull();
            author.FullName.Should().Be("Mark Twain");
        }
        [Fact]
        public void Author_FullName_CombinesFirstAndLastName()
        {
            var author = new Author { FirstName = "George", LastName = "Orwell" };
            author.FullName.Should().Be("George Orwell");
        }
        [Fact]
        public void Author_Books_StartsAsEmptyList()
        {
            var author = new Author { FirstName = "Leo", LastName = "Tolstoy" };

            author.Books.Should().NotBeNull();
            author.Books.Should().BeEmpty();
        }
    }
}
