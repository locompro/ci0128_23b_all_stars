using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Tests.Data.Repositories;

[TestFixture]
public class CrudRepositoryTests
{
    [SetUp]
    public void SetUp()
    {
        _loggerFactory = LoggerFactory.Create(builder => { });

        var options = new DbContextOptionsBuilder<LocomproContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .Options;
        _context = new LocomproContext(options);
        _context.Database.EnsureDeleted(); // Make sure the db is clean
        _context.Database.EnsureCreated();

        // Add known entities
        _context.Set<User>().Add(new User
            { Id = "1", Name = "UserA", Address = "AddressA", Rating = 5.0f, Status = Status.Active });
        _context.Set<User>().Add(new User
            { Id = "2", Name = "UserB", Address = "AddressB", Rating = 3.0f, Status = Status.Active });
        _context.SaveChanges();

        _userRepository = new CrudRepository<User, string>(_context, _loggerFactory);
    }

    private ILoggerFactory _loggerFactory;
    private LocomproContext _context;
    private ICrudRepository<User, string> _userRepository;

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        var id = "1";
        var expectedName = "UserA";
        var expectedAddress = "AddressA";

        // Act
        var result = await _userRepository.GetByIdAsync(id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(expectedName));
            Assert.That(result.Address, Is.EqualTo(expectedAddress));
        });
    }

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Act
        var result = await _userRepository.GetAllAsync();

        // Assert
        Assert.Multiple(() =>
        {
            var enumerable = result as User[] ?? result.ToArray();
            Assert.That(enumerable, Is.Not.Null);
            Assert.That(enumerable, Has.Length.EqualTo(2));
        });
    }

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var newUser = new User
            { Id = "3", Name = "UserC", Address = "AddressC", Rating = 4.0f, Status = Status.Active };

        // Act
        await _userRepository.AddAsync(newUser);
        var result = await _userRepository.GetByIdAsync("3");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("3"));
            Assert.That(result.Name, Is.EqualTo("UserC"));
            Assert.That(result.Address, Is.EqualTo("AddressC"));
        });
    }

    /// <author>Ariel Arevalo Alvarado B50562</author>
    [Test]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var userToUpdate = await _userRepository.GetByIdAsync("1");
        userToUpdate.Address = "NewAddressA";

        // Act
        _userRepository.UpdateAsync(userToUpdate.Id, userToUpdate);
        var updatedUser = await _userRepository.GetByIdAsync("1");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(updatedUser, Is.Not.Null);
            Assert.That(updatedUser.Id, Is.EqualTo("1"));
            Assert.That(updatedUser.Address, Is.EqualTo("NewAddressA"));
        });
    }

    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        var id = "2";
        // Act
        await _userRepository.DeleteAsync(id);
        var result = await _userRepository.GetByIdAsync(id);
        // Assert
        Assert.That(result, Is.Null);
    }
}