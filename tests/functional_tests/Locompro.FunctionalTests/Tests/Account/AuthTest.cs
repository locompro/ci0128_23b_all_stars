using Locompro.FunctionalTests.PageObjects.Account;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests.Account;

public class AuthTest
{
    [Test]
    public void Login_LoginSuccessful()
    {
        // Arrange
        var driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://localhost:7249/Account/Login");
        var login = new Login(driver);

        var username = "RegularGamer";
        var password = "fbm9zyg.kjz9ZHT6dce";

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
        driver.Navigate().GoToUrl("https://localhost:7249/Account/Register");
        var register = new Register(driver);

        var email = "Juan@juan";
        var username = "Juan";
        var password = "Password@123";

        // Act
        register.RegisterAs(email, username, password, password);

        // Assert
        Assert.That(register.IsRegistered(), Is.True);
    }
}