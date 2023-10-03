using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Tests.Repositories
{
    [TestFixture]
    public class AbstractRepositoryTests
    {
        private ILoggerFactory _loggerFactory;
        private LocomproContext _context;
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _loggerFactory = LoggerFactory.Create(builder => { });

            var options = new DbContextOptionsBuilder<LocomproContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
                .Options;
            _context = new LocomproContext(options);
            _context.Database.EnsureDeleted(); // Make sure the db is clean
            _context.Database.EnsureCreated();

            // Add known entities
            _context.Set<User>().Add(new User { Id = "1", Name = "UserA", Address = "AddressA", Rating = 5.0f, Status = Status.Active });
            _context.Set<User>().Add(new User { Id = "2", Name = "UserB", Address = "AddressB", Rating = 3.0f, Status = Status.Active });
            _context.SaveChanges();

            _repository = new UserRepository(_context, _loggerFactory);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            string id = "1";
            string expectedName = "UserA";
            string expectedAddress = "AddressA";

            // Act
            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(id));
                Assert.That(result.Name, Is.EqualTo(expectedName));
                Assert.That(result.Address, Is.EqualTo(expectedAddress));
            });
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var newUser = new User { Id = "3", Name = "UserC", Address = "AddressC", Rating = 4.0f, Status = Status.Active };

            // Act
            await _repository.AddAsync(newUser);
            var result = await _repository.GetByIdAsync("3");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo("3"));
                Assert.That(result.Name, Is.EqualTo("UserC"));
                Assert.That(result.Address, Is.EqualTo("AddressC"));
            });
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var userToUpdate = await _repository.GetByIdAsync("1");
            userToUpdate.Address = "NewAddressA";

            // Act
            await _repository.UpdateAsync(userToUpdate);
            var updatedUser = await _repository.GetByIdAsync("1");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedUser, Is.Not.Null);
                Assert.That(updatedUser.Id, Is.EqualTo("1"));
                Assert.That(updatedUser.Address, Is.EqualTo("NewAddressA"));
            });
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            string id = "2";

            // Act
            await _repository.DeleteAsync(id);
            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}