using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.PageObjects.Account;

public class Login : BasePage
{
    private By _usernameBy => By.Id("usernameInput");
    private By _passwordBy => By.Id("passwordInput");
    private By _signInBy => By.Id("signInSubmit");

    public Login(WebDriver driver) : base(driver)
    {
    }

    public void LoginAs(string username, string password)
    {
        Driver.FindElement(_usernameBy).SendKeys(username);
        Driver.FindElement(_passwordBy).SendKeys(password);
        Driver.FindElement(_signInBy).Click();
    }
    
    public bool IsLoggedIn()
    {
        return base.IsLogoutDisplayed();
    }
    
    public void GoTo()
    {
        Driver.Navigate().GoToUrl("https://localhost:7249/Account/Login");
    }
}