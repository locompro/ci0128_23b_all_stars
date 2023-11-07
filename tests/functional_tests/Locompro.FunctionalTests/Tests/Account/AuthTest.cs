using Locompro.FunctionalTests.PageObjects.Account;
using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests.Account;

public class AuthTest
{
    private readonly TestUserData _loginData = new();

    private readonly TestUserData _registerData = new();

    /// <summary>
    /// Checks if login is successful.
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli - C14826</author>
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

    /// <summary>
    /// Checks if the app registers an user successfully.
    /// </summary>
    /// <author> Brandon Alonso Mora Umaña - C15179 </author>
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