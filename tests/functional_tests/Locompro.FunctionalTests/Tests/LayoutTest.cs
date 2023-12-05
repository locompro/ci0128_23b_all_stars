using Locompro.FunctionalTests.PageObjects.Account;
using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.Tests;

public class LayoutTest
{
    private Login _login = null!;
    private Register _register = null!;
    private IWebDriver _driver = null!;
    private BasePage _basePage = null!;

    private readonly TestUserData _loginData = new();
    
    /// <summary>
    /// Checks if Login button exists
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli - C14826 - Sprint 3</author>
    [Test]
    public void LoginButtonExists()
    {
        _driver = new ChromeDriver();
        _basePage = new BasePage(_driver);
        _basePage.GoTo();
        Assert.That(_basePage.LoginButton, Is.Not.Null);
        _driver.Close();
        _driver.Quit();
    }

    
    /// <summary>
    /// Checks if profile button exists
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli - C14826 - Sprint 3</author>
    [Test]
    public void ProfileButtonExists()
    {
        _driver = new ChromeDriver();
        _register = new Register(_driver);
        _register.GoTo();
        _register.RegisterAs(_loginData);
        _login = new Login(_driver);
        _login.GoTo();
        _login.LoginAs(_loginData);
        WaitUntilUserIsLoggedIn(_driver);
        _basePage = new BasePage(_driver);
        _basePage.GoTo();
        
        Assert.That(_basePage.ProfileButton, Is.Not.Null);
        _register.ClickLogout();
        _driver.Close();
        _driver.Quit();
    }

    /// <summary>
    /// Checks if ShoppingList button exists
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli - C14826 - Sprint 3</author>
    [Test]
    public void ShoppingListButtonExists()
    {
        _driver = new ChromeDriver();
        _register = new Register(_driver);
        _register.GoTo();
        _register.RegisterAs(_loginData);
        _login = new Login(_driver);
        _login.GoTo();
        _login.LoginAs(_loginData);
        WaitUntilUserIsLoggedIn(_driver);
        _basePage = new BasePage(_driver);
        _basePage.GoTo();

        Assert.That(_basePage.ShoppingListButton, Is.Not.Null);
        _register.ClickLogout();
        _driver.Close();
        _driver.Quit();
    }

    /// <summary>
    /// Checks if Logout button exists
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli - C14826 - Sprint 3</author>
    [Test]
    public void LogoutButtonExists()
    {
        _driver = new ChromeDriver();
        _register = new Register(_driver);
        _register.GoTo();
        _register.RegisterAs(_loginData);
        _login = new Login(_driver);
        _login.GoTo();
        _login.LoginAs(_loginData);
        WaitUntilUserIsLoggedIn(_driver);
        _basePage = new BasePage(_driver);
        _basePage.GoTo();

        Assert.That(_basePage.LogoutButton, Is.Not.Null);
        _register.ClickLogout();
        _driver.Close();
        _driver.Quit();
    }
    
    /// <summary>
    /// Checks if Contribute button exists
    /// </summary>
    /// <author> Gabriel Molina Bulgarelli - C14826 - Sprint 3</author>
    [Test]
    public void ContributeButtonExists()
    {
        _driver = new ChromeDriver();
        _register = new Register(_driver);
        _register.GoTo();
        _register.RegisterAs(_loginData);
        _login = new Login(_driver);
        _login.GoTo();
        _login.LoginAs(_loginData);
        WaitUntilUserIsLoggedIn(_driver);
        _basePage = new BasePage(_driver);
        _basePage.GoTo();

        Assert.That(_basePage.ContributeButton, Is.Not.Null);
        _register.ClickLogout();
        _driver.Close();
        _driver.Quit();

    }
    
    private void WaitUntilUserIsLoggedIn(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => _login.IsLoggedIn());
    }

}