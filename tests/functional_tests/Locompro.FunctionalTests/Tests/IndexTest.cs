using Locompro.FunctionalTests.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests;

[TestFixture]
public class IndexTest
{
    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver();
        _indexPage = new IndexPage(_driver);
        _indexPage.GoTo();
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Close();
        _driver?.Quit();
    }
    
    private IWebDriver? _driver;
    private IndexPage? _indexPage;
    
    /// <author> Ariel Arévalo Alvarado B50562 - Sprint 3 </author>
    [Test]
    public void SearchBoxExists()
    {
        Assert.That(_indexPage?.SearchBox, Is.Not.Null);
    }

    /// <author> Ariel Arévalo Alvarado B50562 - Sprint 3 </author>
    [Test]
    public void SearchButtonExists()
    {
        Assert.That(_indexPage?.SearchButton, Is.Not.Null);
    }

    /// <author> Ariel Arévalo Alvarado B50562 - Sprint 3 </author>
    [Test]
    public void AdvancedSearchButtonExists()
    {
        Assert.That(_indexPage?.AdvancedSearchButton, Is.Not.Null);
    }
    
    /// <author> Ariel Arévalo Alvarado B50562 - Sprint 3 </author>
    [Test]
    public void AdvancedSearchModalExists()
    {
        _indexPage?.GoTo();
        _indexPage?.ClickAdvancedSearchButton();
        _indexPage?.WaitForAdvancedSearchModalToBeVisible();
        Assert.That(_indexPage != null && _indexPage.AdvancedSearchModal.Displayed, Is.True);
    }
    
    /// <author> Ariel Arévalo Alvarado B50562 - Sprint 3 </author>
    [Test]
    public void SearchButtonNavigatesToResults()
    {
        _indexPage?.GoTo();
        _indexPage?.EnterSearchQuery("a");
        _indexPage?.ClickSearchButton();
        _indexPage?.WaitForNavigationToSearchResults();
        Assert.That(_driver?.Url, Is.EqualTo("https://localhost:7249/SearchResults/SearchResults"));
    }
}