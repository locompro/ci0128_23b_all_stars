using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.PageObjects.Account;

public class Login : BasePage
{
    protected WebDriver _driver;
    public string Url => "https://localhost:5001/Account/Login";

    private By _usernameBy => By.Id("usernameInput");
    private By _passwordBy => By.Id("passwordInput");
    private By _signInBy => By.Id("signInSubmit");

    public Login(WebDriver driver) : base(driver)
    {
        _driver = driver;
        if (!_driver.Url.Contains("Account/Login"))
        {
            throw new Exception("This is not the login page");
        }
    }

    public void LoginAs(string username, string password)
    {
        _driver.FindElement(_usernameBy).SendKeys(username);
        _driver.FindElement(_passwordBy).SendKeys(password);
        _driver.FindElement(_signInBy).Click();
    }
}