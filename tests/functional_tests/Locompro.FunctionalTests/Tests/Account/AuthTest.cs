using Locompro.FunctionalTests.PageObjects.Account;
using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests.Account;

public class AuthTest
{

    private readonly TestUserData _loginData = new();

    private readonly TestUserData _registerData = new();

    
    [Test]
    public void Login_LoginSuccessful()
    {
        // Arrange
        var driver = new ChromeDriver();
        var register = new Register(driver);
        register.GoTo();
        register.RegisterAs(_loginData);
        var login = new Login(driver);
        login.GoTo();

        var username = _loginData.Username;
        var password = _loginData.Password;
        
        // Act
        login.LoginAs(username, password);

        // Assert
        Assert.That(login.IsLoggedIn(), Is.True);
        login.ClickLogout();
        driver.Close();
        driver.Quit();
    }

    [Test]
    public void Register_RegisterSuccessful()
    {
        // Arrange
        var driver = new ChromeDriver();
        var register = new Register(driver);
        register.GoTo();
        
        // Act
        register.RegisterAs(_registerData);

        // Assert
        Assert.That(register.IsRegistered(), Is.True);
        register.ClickLogout();
        driver.Close();
        driver.Quit();
    }
}