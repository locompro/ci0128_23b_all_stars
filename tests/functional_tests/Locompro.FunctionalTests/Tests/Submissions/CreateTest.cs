using Locompro.FunctionalTests.PageObjects.Account;
using Locompro.FunctionalTests.PageObjects.Shared;
using Locompro.FunctionalTests.PageObjects.Submissions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests.Submissions;

[TestFixture]
public class CreatePageTests
{
    [SetUp]
    public void SetUp()
    {
        _driver = CreateDriverAndLogin();
        _createPage = new CreatePage(_driver);
        _createPage.GoTo();
    }

    [TearDown]
    public void TearDown()
    {
        _createPage.ClickLogout();
        _driver.Close();
        _driver.Quit();
    }

    private IWebDriver? _driver;
    private CreatePage? _createPage;
    private readonly TestUserData _loginData = new();
    private bool _isRegistered;

    private IWebDriver CreateDriverAndLogin()
    {
        var driver = new ChromeDriver();
        if (!_isRegistered)
        {
            var register = new Register(driver);
            register.GoTo();
            register.RegisterAs(_loginData);
            _isRegistered = true;
        }

        var login = new Login(driver);
        login.GoTo();
        login.LoginAs(_loginData.Username, _loginData.Password);
        return driver;
    }


    [Test]
    public void StoreSelectElementExists()
    {
        Assert.That(_createPage?.StoreSelect, Is.Not.Null);
    }

    /// <author> Brandon Alonso Mora Umaña - C15179 </author>
    [Test]
    public void AddStoreButtonExists()
    {
        Assert.That(_createPage?.AddStoreButton, Is.Not.Null);
    }

    /// <author> Brandon Alonso Mora Umaña - C15179 </author>
    [Test]
    public void ProductSelectElementExists()
    {
        Assert.That(_createPage?.ProductSelect, Is.Not.Null);
    }

    /// <author> Brandon Alonso Mora Umaña - C15179 </author>
    [Test]
    public void AddProductButtonExists()
    {
        Assert.That(_createPage?.AddProductButton, Is.Not.Null);
    }
    
    /// <summary>
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public void DescriptionTextAreaExists()
    {
        Assert.That(_createPage?.DescriptionTextArea, Is.Not.Null);
    }

    /// <summary>
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public void FileInputElementExists()
    {
        Assert.That(_createPage?.FileInput, Is.Not.Null);
    }
    
    /// <summary>
    ///     <author>Joseph Stuart Valverde Kong C18100 - Sprint 3</author>
    /// </summary>
    [Test]
    public void SubmitButtonExists()
    {
        Assert.That(_createPage?.SubmitButton, Is.Not.Null);
    }


    /// <summary>
    /// Checks if the store modal opens when the user clicks the 'Add Store' button.
    /// </summary>
    /// <author> A. Badilla Olivas - B80874 </author>
    /// <sprint> 3 </sprint>
    [Test]
    public void AddStoreModalOpens()
    {
        // Arrange
        _createPage.GoTo();
        _createPage.ClickAddStore();
        _createPage.WaitForStoreModalToBeVisible();

        // Assert
        Assert.That(_createPage.PartialStoreName.Displayed, Is.True);

        _createPage.WaitForHideStoreModalToBeClickable();
        _createPage.ClickHideAddStore();
    }

    /// <summary>
    /// Checks if the store modal map exists inside the store modal.
    /// </summary>
    /// <author> A. Badilla Olivas - B80874 </author>
    /// <sprint> 3 </sprint>
    [Test]
    public void StoreModalMapExists()
    {
        // Act
        _createPage.GoTo();
        _createPage.ClickAddStore();
        _createPage.WaitForStoreModalToBeVisible();
        // Assert
        Assert.That(_createPage.StoreModalMap, Is.Not.Null);
        _createPage.WaitForHideStoreModalToBeClickable();
        _createPage.ClickHideAddStore();
    }

    /// <summary>
    /// Checks if the product modal opens when the user clicks the 'Add Product' button.
    /// </summary>
    /// <author> A. Badilla Olivas - B80874 </author>
    /// <sprint> 3 </sprint>
    [Test]
    public void AddProductModalExist()
    {
        _createPage.GoTo();
        _createPage.ClickAddProduct();
        _createPage.WaitForProductModalToBeVisible();
        Assert.That(_createPage.PartialProductName.Displayed, Is.True);
        _createPage.WaitForHideProductModalToBeClickable();
        _createPage.ClickHideAddProduct();
    }
}