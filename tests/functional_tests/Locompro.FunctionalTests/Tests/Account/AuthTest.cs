using Locompro.FunctionalTests.PageObjects.Account;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests.Account;

public class AuthTest
{
    class UserData
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    private readonly UserData _loginData = new()
    {
        Email = "Juan@juan",
        Username = "Pruebitas",
        Password = "Password@123"
    };
    
    private readonly UserData _registerData = new()
    {
        Email = "Juan@juan2",
        Username = "Juan",
        Password = "Password@123"
    };    
        
        
    [SetUp]
    public void Setup()
    {
        var driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://localhost:7249/Account/Register");
        var register = new Register(driver);
        register.RegisterAs(_loginData.Email, _loginData.Username, _loginData.Password, _loginData.Password);
    }

    [Test]
    public void Login_LoginSuccessful()
    {
        // Arrange
        var driver = new ChromeDriver();
        var login = new Login(driver);
        login.GoTo();

        var username = _loginData.Username;
        var password = _loginData.Password;
        
        // Act
        login.LoginAs(username, password);

        // Assert
        Assert.That(login.IsLoggedIn(), Is.True);
    }

    [Test]
    public void Register_RegisterSuccessful()
    {
        // Arrange
        var driver = new ChromeDriver();
        var register = new Register(driver);
        register.GoTo();

        var email = _registerData.Email;
        var username = _registerData.Username;
        var password = _registerData.Password;

        // Act
        register.RegisterAs(email, username, password, password);

        // Assert
        Assert.That(register.IsRegistered(), Is.True);
    }
}