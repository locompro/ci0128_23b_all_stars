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
        _driver.FindElement(_changePasswordButton).Click();
        WaitForElementToBeVisible(By.Id("ChangePasswordModal"), 5);
    }

    // Method to fill and submit 'Change Password' form
    public void ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
    {
        OpenChangePasswordModal();

        _driver.FindElement(By.Id("PasswordChange_CurrentPassword")).SendKeys(currentPassword);
        _driver.FindElement(By.Id("PasswordChange_NewPassword")).SendKeys(newPassword);
        _driver.FindElement(By.Id("PasswordChange_ConfirmNewPassword")).SendKeys(confirmNewPassword);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click(); // Adjust if there's a specific ID or class
    }

    // Method to open 'Update User Data' modal
    public void OpenUpdateUserDataModal()
    {
        _driver.FindElement(_updateUserDataButton).Click();
        WaitForElementToBeVisible(By.Id("UpdateUserDataModal"), 5);
    }

    // Method to fill and submit 'Update User Data' form
    public void UpdateUserData(string email, string province, string canton, string exactAddress)
    {
        OpenUpdateUserDataModal();

        var emailField = _driver.FindElement(By.Id("UserDataUpdate_Email"));
        var provinceDropdown = _driver.FindElement(By.Id("UserDataUpdate_Province"));
        var cantonDropdown = _driver.FindElement(By.Id("UserDataUpdate_Canton"));
        var exactAddressField = _driver.FindElement(By.Id("UserDataUpdate_ExactAddress"));
        var saveButton = _driver.FindElement(By.Id("PostUserUpdateButton"));

        emailField.Clear();
        emailField.SendKeys(email);

        var provinceSelect = new SelectElement(provinceDropdown);
        provinceSelect.SelectByText(province);

        // Ensure that the canton dropdown options are populated after selecting a province
        if (!WaitForOptionsToBePopulated(By.Id("UserDataUpdate_Canton"), 5, 1))
        {
            throw new Exception("Canton options were not populated within the expected time.");
        }

        var cantonSelect = new SelectElement(cantonDropdown);
        cantonSelect.SelectByText(canton);

        exactAddressField.Clear();
        exactAddressField.SendKeys(exactAddress);

        saveButton.Click();
    }

    
    private IWebElement? WaitForElementToBeVisible(By locator, int timeoutInSeconds)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(drv =>
        {
            try
            {
                var element = drv.FindElement(locator);
                return element.Displayed ? element : throw new NotFoundException();
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });
    }
    private bool WaitForOptionsToBePopulated(By locator, int timeoutInSeconds, int minOptionCount)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(d =>
        {
            try
            {
                var options = d.FindElements(locator);
                return options.Count >= minOptionCount;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        });
    }
    // You can add more methods here for interacting with the page elements and modals
    // For example, filling forms within modals and submitting them
}
