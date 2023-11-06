namespace Locompro.FunctionalTests.PageObjects.Shared;

using OpenQA.Selenium;

public class BasePage
{
    public IWebDriver Driver { get; }

    public BasePage(IWebDriver driver)
    {
        Driver = driver;
    }

    // Common elements in the layout like header links, logout button, etc.

    protected IWebElement ContributeButton =>
        Driver.FindElement(By.Id("CreateSubmissionButton"));

    protected IWebElement ProfileButton =>
        Driver.FindElement(By.Id("ProfileButton"));

    protected IWebElement LogoutButton =>
        Driver.FindElement(By.Id("LogoutButton"));

    protected IWebElement LoginButton =>
        Driver.FindElement(By.Id("LoginButton"));

    // Common methods for interacting with layout elements

    public void ClickContribute()
    {
        ContributeButton.Click();
    }

    public void ClickProfile()
    {
        ProfileButton.Click();
    }

    public void ClickLogout()
    {
        LogoutButton.Click();
    }

    public void ClickLogin()
    {
        LoginButton.Click();
    }
    
    public bool IsLoggedIn()
    {
        return LogoutButton.Displayed;
    }
}