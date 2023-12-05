using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;

namespace Locompro.FunctionalTests.PageObjects.Account;

public class Register : BasePage
{

    protected IWebElement emailInput => Driver.FindElement(By.Id("EmailInput"));
    protected IWebElement usernameInput => Driver.FindElement(By.Id("UsernameInput"));
    protected IWebElement passwordInput => Driver.FindElement(By.Id("PasswordInput"));
    protected IWebElement confirmPasswordInput => Driver.FindElement(By.Id("ConfirmedPasswordInput"));
    protected IWebElement registerButton => Driver.FindElement(By.Id("RegisterButton"));

    public Register(IWebDriver driver) : base(driver)
    {
    }

    public void RegisterAs(string email, string username, string password, string confirmPassword)
    {
        emailInput.SendKeys(email);
        usernameInput.SendKeys(username);
        passwordInput.SendKeys(password);
        confirmPasswordInput.SendKeys(confirmPassword);
        registerButton.Click();
    }
    
    public void RegisterAs(TestUserData userData)
    {
        emailInput.SendKeys(userData.Email);
        usernameInput.SendKeys(userData.Username);
        passwordInput.SendKeys(userData.Password);
        confirmPasswordInput.SendKeys(userData.Password);
        registerButton.Click();
    }

    public bool IsRegistered()
    {
        return base.IsLogoutDisplayed();
    }
    
    public void GoTo()
    {
        Driver.Navigate().GoToUrl("https://localhost:7249/Account/Register");
    }
    
}