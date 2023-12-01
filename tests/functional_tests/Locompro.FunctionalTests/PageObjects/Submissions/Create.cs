using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.PageObjects.Submissions;

public class CreatePage: BasePage
{
    // elements of the main create page
    public IWebElement StoreSelect => Driver.FindElement(By.Id("mainStoreName"));

    public IWebElement AddStoreButton => Driver.FindElement(By.Id("showAddStoreBtn"));

    public IWebElement ProductSelect => Driver.FindElement(By.Id("mainProductName"));

    public IWebElement AddProductButton => Driver.FindElement(By.Id("showAddProductBtn"));
    
    public IWebElement hideAddStoreBtn => Driver.FindElement(By.Id("hideAddStoreBtn"));

    public IWebElement DescriptionTextArea => Driver.FindElement(By.Id("mainDescription"));

    public IWebElement FileInput => Driver.FindElement(By.Id("file"));

    public IWebElement SubmitButton => Driver.FindElement(By.Id("addSubmissionBtn"));
    
    // add store modal elements
    public IWebElement StoreModalMap => Driver.FindElement(By.Id("StoreModalMap"));
    public IWebElement PartialStoreName => Driver.FindElement(By.Id("partialStoreName"));
    public IWebElement PartialStoreProvinceSelect => Driver.FindElement(By.Id("partialStoreProvince"));
    public IWebElement PartialStoreCantonSelect => Driver.FindElement(By.Id("partialStoreCanton"));
    public IWebElement StoreTelephoneInput => Driver.FindElement(By.Id("Telephone"));
    public IWebElement StoreAddressTextArea => Driver.FindElement(By.Id("Address"));
    
    // add product modal elements
    // Modal elements for adding a product
    public IWebElement PartialProductName => Driver.FindElement(By.Id("partialProductName"));
    public IWebElement PartialProductModel => Driver.FindElement(By.Id("Model"));
    public IWebElement PartialProductBrand => Driver.FindElement(By.Id("Brand"));
    public IWebElement PartialProductCategory => Driver.FindElement(By.Id("partialProductCategory"));
    
    public IWebElement hideAddProductBtn => Driver.FindElement(By.Id("hideAddProductBtn"));

    // Constructor
    public CreatePage(IWebDriver driver): base (driver)
    {
    }

    public void SelectStore(string storeName)
    {
        StoreSelect.SendKeys(storeName);
    }

    public void ClickAddStore()
    {
        AddStoreButton.Click();
    }
    
    public void ClickHideAddStore()
    {
        hideAddStoreBtn.Click();
    }
    public void ClickHideAddProduct()
    {
        hideAddProductBtn.Click();
    }
    public void SelectProduct(string productName)
    {
        ProductSelect.SendKeys(productName);
    }

    public void ClickAddProduct()
    {
        AddProductButton.Click();
    }

    public void EnterDescription(string description)
    {
        DescriptionTextArea.SendKeys(description);
    }

    public void UploadFile(string filePath)
    {
        FileInput.SendKeys(filePath);
    }

    public void Submit()
    {
        SubmitButton.Click();
    }
    
    public void GoTo()
    {
        Driver.Navigate().GoToUrl("https://localhost:7249/Submissions/Create");
    }

    public void EnterStoreName(string storeName)
    {
        PartialStoreName.SendKeys(storeName);
    }

    public void SelectStoreProvince(string province)
    {
        PartialStoreProvinceSelect.SendKeys(province);
    }

    public void SelectStoreCanton(string canton)
    {
        PartialStoreCantonSelect.SendKeys(canton);
    }

    public void EnterStoreTelephone(string telephone)
    {
        StoreTelephoneInput.SendKeys(telephone);
    }

    public void EnterStoreAddress(string address)
    {
        StoreAddressTextArea.SendKeys(address);
    }
    
    public void EnterPartialProductName(string name)
    {
        PartialProductName.SendKeys(name);
    }

    public void EnterPartialProductModel(string model)
    {
        PartialProductModel.SendKeys(model);
    }

    public void EnterPartialProductBrand(string brand)
    {
        PartialProductBrand.SendKeys(brand);
    }

    public void SelectPartialProductCategory(string category)
    {
        PartialProductCategory.SendKeys(category);
    }
    public bool WaitForElementToBeVisible(By locator, int timeoutInSeconds)
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
    
    public IWebElement WaitForElementToBeClickable(By locator, int timeoutInSeconds)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return (element != null && element.Enabled && element.Displayed) ? element : throw new WebDriverException("Element not clickable");
        });
    }
    
    public void WaitForStoreModalToBeVisible()
    {
        WaitForElementToBeVisible(By.Id("StoreModalMap"), 5);
    }
    public void WaitForHideStoreModalToBeClickable()
    {
        WaitForElementToBeClickable(By.Id("hideAddStoreBtn"), 5);
    }
    
    public void WaitForProductModalToBeVisible()
    {
        WaitForElementToBeVisible(By.Id("partialProductName"), 5);
    }
    
    public void WaitForHideProductModalToBeClickable()
    {
        WaitForElementToBeClickable(By.Id("hideAddProductBtn"), 5);
    }
}