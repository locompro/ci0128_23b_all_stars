using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;

namespace Locompro.FunctionalTests.PageObjects;

public class Index : BasePage
{
    private readonly IWebDriver driver;

    public Index(IWebDriver browser) : base(browser)
    {
        driver = browser;
    }

    // Define By locators instead of using FindsBy
    private By searchBoxLocator = By.Id("searchBox");
    private By searchButtonLocator = By.Id("searchButton");
    private By advancedSearchButtonLocator = By.Id("advancedSearchButton");

    // ... (Rest of your class remains the same)

    // Access elements through By locators within methods:
    public void EnterSearchQuery(string query)
    {
        var searchBox = driver.FindElement(searchBoxLocator);
        searchBox.Clear();
        searchBox.SendKeys(query);
    }

    public void ClickSearchButton()
    {
        var searchButton = driver.FindElement(searchButtonLocator);
        searchButton.Click();
    }

    public void ClickAdvancedSearchButton()
    {
        var advancedSearchButton = driver.FindElement(advancedSearchButtonLocator);
        advancedSearchButton.Click();
    }
}