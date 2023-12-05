using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.PageObjects.Shared;

using OpenQA.Selenium;

public class BasePage
{
    protected IWebDriver Driver { get; }
    private readonly string _baseUrl = "https://localhost:7249/";

    public BasePage(IWebDriver driver)
    {
        Driver = driver;
    }

    // Common elements in the layout like header links, logout button, etc.

    public IWebElement ContributeButton =>
        Driver.FindElement(By.Id("CreateSubmissionButton"));

    public IWebElement ProfileButton =>
        Driver.FindElement(By.Id("ProfileButton"));

    public IWebElement ShoppingListButton =>
        Driver.FindElement(By.Id("ShoppingListButton"));
    
    public IWebElement LogoutButton =>
        Driver.FindElement(By.Id("LogoutButton"));

    public IWebElement LoginButton =>
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
    
    public bool IsLogoutDisplayed()
    {
        return LogoutButton.Displayed;
    }
    
    public void GoTo()
    {
        Driver.Navigate().GoToUrl(_baseUrl);
    }

    
    protected bool WaitForElementToBeVisible(By locator, int timeoutInSeconds)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        try
        {
            return wait.Until(drv =>
            {
                var element = drv.FindElement(locator);
                return element.Displayed;
            });
        }
        catch (WebDriverTimeoutException)
        {
            // If the wait times out, return false indicating the element is not visible
            return false;
        }
        catch (NoSuchElementException)
        {
            // If the element is not found, return false
            return false;
        }
    }
    
    protected bool WaitForPageNavigation(string expectedUrl, int timeoutInSeconds)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        try
        {
            return wait.Until(drv => drv.Url.Contains(expectedUrl));
        }
        catch (WebDriverTimeoutException)
        {
            // If the wait times out, return false indicating the URL did not change as expected
            return false;
        }
    }
}