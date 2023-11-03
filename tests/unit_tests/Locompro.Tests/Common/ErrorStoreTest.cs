using Locompro.Common;

namespace Locompro.Tests.Common;

/// <summary>
/// This class contains unit tests for the ErrorStore class.
/// </summary>
/// <author>Your Name</author>
[TestFixture]
public class ErrorStoreTest
{
    private ErrorStore _errorStore = null!;
    

    /// <summary>
    /// Initializes a new instance of the ErrorStore class for each test.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 </author>
    [SetUp]
    public void SetUp()
    {
        _errorStore = new ErrorStore();
    }

    /// <summary>
    /// Tests the StoreError method by adding a single error to the store and verifying the error count and HasErrors property.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 </author>
    [Test]
    public void StoreError_StoresSingleError()
    {
        // Arrange
        var errorMessage = "An error occurred.";

        // Act
        _errorStore.StoreError(errorMessage);

        // Assert
        Assert.That(_errorStore.Count, Is.EqualTo(1));
        Assert.IsTrue(_errorStore.HasErrors);
    }

    /// <summary>
    /// Tests the StoreErrors method by adding multiple errors to the store and verifying the error count and HasErrors property.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 </author>
    [Test]
    public void StoreErrors_StoresMultipleErrors()
    {
        // Arrange
        var errorMessages = new List<string> { "Error 1", "Error 2", "Error 3" };

        // Act
        _errorStore.StoreErrors(errorMessages);

        // Assert
        Assert.That(_errorStore.Count, Is.EqualTo(errorMessages.Count));
        Assert.IsTrue(_errorStore.HasErrors);
    }

    /// <summary>
    /// Tests the ClearStore method by adding an error to the store, clearing the store, and verifying the error count and HasErrors property.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 </author>
    [Test]
    public void ClearStore_ClearsAllStoredErrors()
    {
        // Arrange
        _errorStore.StoreError("An error occurred.");

        // Act
        _errorStore.ClearStore();

        // Assert
        Assert.That(_errorStore.Count, Is.EqualTo(0));
        Assert.IsFalse(_errorStore.HasErrors);
    }

    /// <summary>
    /// Tests the GetErrors method by adding multiple errors to the store, retrieving the errors, and verifying the retrieved errors match the stored errors.
    /// </summary>
    /// <author> A. Badilla-Olivas b80874 </author>
    [Test]
    public void GetErrors_RetrievesStoredErrors()
    {
        // Arrange
        var errorMessages = new List<string> { "Error 1", "Error 2", "Error 3" };
        _errorStore.StoreErrors(errorMessages);

        // Act
        var retrievedErrors = _errorStore.GetErrors();

        // Assert
        CollectionAssert.AreEqual(errorMessages, retrievedErrors);
    }
}
