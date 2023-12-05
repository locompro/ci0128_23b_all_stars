using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;

namespace Locompro.FunctionalTests.PageObjects;

public class IndexPage : BasePage
{
    private readonly IWebDriver _driver;

    public IndexPage(IWebDriver browser) : base(browser)
    {
        _driver = browser;
    }

    // Define By locators instead of using FindsBy
    public IWebElement SearchBox => Driver.FindElement(By.Id("searchBox"));

    public IWebElement SearchButton => Driver.FindElement(By.Id("searchButton"));

    public IWebElement AdvancedSearchButton => Driver.FindElement(By.Id("advancedSearchButton"));

    public IWebElement AdvancedSearchModal => Driver.FindElement(By.Id("advancedSearchModal"));
    
    public void EnterSearchQuery(string query)
    {
        SearchBox.SendKeys(query);
    }

    public void ClickSearchButton()
    {
        SearchButton.Click();
    }

    public void ClickAdvancedSearchButton()
    {
        AdvancedSearchButton.Click();
    }
    
    public void GoTo()
    {
        Driver.Navigate().GoToUrl("https://localhost:7249/");
    }

    public void WaitForAdvancedSearchModalToBeVisible()
    {
        WaitForElementToBeVisible(By.Id("advancedSearchModal"), 5);
    }
    
    public void WaitForNavigationToSearchResults()
    {
        WaitForPageNavigation("https://localhost:7249/SearchResults/SearchResults", 5);
    }
}