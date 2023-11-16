using Locompro.Common.ErrorStore;

namespace Locompro.Tests.Common;

/// <summary>
///     This class contains unit tests for the <see cref="ErrorStoreFactory" /> class.
/// </summary>
[TestFixture]
public class ErrorStoreFactoryTests
{
    /// <summary>
    ///     An instance of <see cref="ErrorStoreFactory" /> to be used for testing.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 - Sprint 2</author>
    private readonly ErrorStoreFactory _factory;

    /// <summary>
    ///     Constructor to initialize the <see cref="ErrorStoreFactory" /> instance.
    /// </summary>
    public ErrorStoreFactoryTests()
    {
        _factory = new ErrorStoreFactory();
    }

    /// <summary>
    ///     Tests that the <see cref="ErrorStoreFactory.Create" /> method returns a non-null <see cref="IErrorStore" />
    ///     instance.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 - Sprint 2</author>
    [Test]
    public void Create_ReturnsNonNullErrorStore()
    {
        // Act
        var errorStore = _factory.Create();

        // Assert
        Assert.That(errorStore, Is.Not.Null);
        Assert.That(errorStore, Is.InstanceOf<ErrorStore>());
    }

    /// <summary>
    ///     Tests that the <see cref="ErrorStoreFactory.Create" /> method returns a new instance of <see cref="IErrorStore" />
    ///     on each call.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 - Sprint 2</author>
    [Test]
    public void Create_ReturnsNewInstanceOnEachCall()
    {
        // Act
        var errorStore1 = _factory.Create();
        var errorStore2 = _factory.Create();

        // Assert
        Assert.That(errorStore2, Is.Not.SameAs(errorStore1));
    }
}