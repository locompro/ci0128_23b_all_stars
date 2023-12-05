using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.PageObjects.Account;

public class Profile : BasePage
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl = "https://localhost:7249/Account/Profile";

    // Locators
    private readonly By _userName = By.Id("UserName");
    private readonly By _name = By.Id("Name");
    private readonly By _email = By.Id("Email");
    private readonly By _address = By.Id("Address");
    private readonly By _contributions = By.Id("Contributions");
    private readonly By _rating = By.Id("Rating");
    private readonly By _contributionsButton = By.Id("ContributionsButton");
    private readonly By _updateUserDataButton = By.Id("UpdateUserDataButton");
    private readonly By _changePasswordButton = By.Id("ChangePasswordButton");
    

    // Constructor
    public Profile(IWebDriver driver) : base(driver)
    {
        this._driver = driver;
    }

    // Page actions as properties or methods
    public string GetUserName() => _driver.FindElement(_userName).Text;
    public string GetName() => _driver.FindElement(_name).Text;
    public string GetEmail() => _driver.FindElement(_email).Text;
    public string GetAddress() => _driver.FindElement(_address).Text;
    public int GetContributions() => int.Parse(_driver.FindElement(_contributions).Text);
    public double GetRating() => double.Parse(_driver.FindElement(_rating).Text);

    // Methods for interacting with the page elements
    public void ClickContributionsButton() => _driver.FindElement(_contributionsButton).Click();
    public void ClickUpdateProfile() => _driver.FindElement(_updateUserDataButton).Click();
    public void ClickChangePassword() => _driver.FindElement(_changePasswordButton).Click();
    
    public void GoTo()
    {
        _driver.Navigate().GoToUrl(_baseUrl);
    }
    
    public void Logout()
    {
        ClickLogout();
    }
    // Additional methods for interacting with modals
     public void OpenChangePasswordModal()
    {
        // Assuming _driver is your instance of IWebDriver
        try {
            var changePasswordButton = _driver.FindElement(_changePasswordButton);
            changePasswordButton.Click();
            return;
        }
        catch (NoSuchElementException)
        {
           var changePasswordButton = _driver.FindElement(By.Id("ChangePasswordButton"));
           changePasswordButton.Click();
        }
        
        WaitForElementToBeVisible(By.Id("ChangePasswordModal"), 5);
    }

    public bool ChechPasswordModalIsAsExpected()
     {
         try
         {
             _driver.FindElement(By.Id("CloseChangePasswordButton"));
             _driver.FindElement(By.Id("PasswordChange_CurrentPassword"));
             _driver.FindElement(By.Id("PasswordChange_NewPassword"));
             _driver.FindElement(By.Id("PasswordChange_ConfirmNewPassword"));
             return true;
         }
         catch (NoSuchElementException)
         {
             return false;
         }

     }

    // Method to fill and submit 'Change Password' form
    public void ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
    {
        _driver.FindElement(By.Id("PasswordChange_CurrentPassword")).SendKeys(currentPassword);
        _driver.FindElement(By.Id("PasswordChange_NewPassword")).SendKeys(newPassword);
        _driver.FindElement(By.Id("PasswordChange_ConfirmNewPassword")).SendKeys(confirmNewPassword);
        _driver.FindElement(By.Id("PostPasswordChangeButton")).Click();
    }
    
    public bool HasPasswordChanged()
    {
        var passwordChangedAlert = _driver.FindElement(By.Id("passwordChangedAlert"));
        
        try
        {
            return passwordChangedAlert.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }


    // Method to open 'Update User Data' modal
    public void OpenUpdateUserDataModal()
    {
        WaitForElementToBeClickable(_updateUserDataButton, 5);
        Driver.FindElement(_updateUserDataButton).Click();
        WaitForElementToBeVisible(By.Id("UpdateUserDataModal"), 5);
    }

// Method to fill and submit 'Update User Data' form
    public void UpdateUserData(string email, string province, string canton, string exactAddress)
    {
        _driver.FindElement(By.Id("UserDataUpdate_Email")).SendKeys(email);
        _driver.FindElement(By.Id("UserDataUpdate_Province")).SendKeys(province);
        _driver.FindElement(By.Id("UserDataUpdate_Canton")).SendKeys(canton);
        _driver.FindElement(By.Id("UserDataUpdate_ExactAddress")).SendKeys(exactAddress);
        WaitForElementToBeClickable(By.Id("PostUserUpdateButton"), 5);
        _driver.FindElement(By.Id("PostUserUpdateButton")).Click();
    
    }
    
    public bool WaitForElementToBeVisible(By locator, int timeoutInSeconds)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
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
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return (element != null && element.Enabled && element.Displayed) ? element : throw new WebDriverException("Element not clickable");
        });
    }
}
