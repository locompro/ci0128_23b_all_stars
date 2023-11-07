namespace Locompro.FunctionalTests.PageObjects.Shared;

public class TestUserData
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    // The constructor name should match the class name.
    // Also, it should not return anything, hence no need for UserData type.
    public TestUserData()
    {
        var testNumber = new Random().Next(0, 10000000);
        // Use 'this' to refer to the current instance's properties.
        Email = $"test{testNumber}@user.com";
        Username = $"testuser{testNumber}";
        Password = "Test123!aa##";
    }
}