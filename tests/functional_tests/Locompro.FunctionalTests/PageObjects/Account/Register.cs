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

    public bool IsRegistered()
    {
        return base.IsLoggedIn();
    }

    public bool IsRegisterPage()
    {
        return Driver.Url.Contains("Account/Register");
    }
}