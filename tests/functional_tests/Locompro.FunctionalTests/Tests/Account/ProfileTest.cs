using Locompro.FunctionalTests.PageObjects.Account;
using Locompro.FunctionalTests.PageObjects.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Locompro.FunctionalTests.Tests.Account;

public class ProfileTest
{
    private Login _login = null!;
    private Profile _profile = null!;
    private Register _register = null!;
    private TestUserData _loginData = new();
    
    
    [Test, Order(1)]
    public void UserDataIsCorrect()
    {
        // arrange
        var driver = new ChromeDriver();
        _register = new Register(driver);
        _register.GoTo();
        _register.RegisterAs(_loginData);
        _profile = new Profile(driver);

        // act
        _profile.GoTo();

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(_profile.GetUserName(), Is.EqualTo(_loginData.Username));
            Assert.That(_profile.GetEmail(), Is.EqualTo(_loginData.Email));
            Assert.That(_profile.GetAddress(), Is.Empty.Or.Null);
            Assert.That(_profile.GetContributions(), Is.EqualTo(0));
            Assert.That(_profile.GetRating(), Is.EqualTo(0));
        });
        _register.ClickLogout();
        driver.Close();
        driver.Quit();
    }

    [Test]
    public void UserDataIsUpdated()
    {
        // arrange
        var driver = new ChromeDriver();
        _login = new Login(driver);
        _login.GoTo();
        _login.LoginAs(_loginData);
        WaitUntilUserIsLoggedIn(driver);
        _profile = new Profile(driver);

        const string email = "ak7@gg.lol";
        const string address = "Alajuela, Alajuela, Parque de los mangos";
        // act
        _profile.GoTo();
        // wait for page to load

        _profile.OpenUpdateUserDataModal();
        _profile.UpdateUserData(email, "Alajuela", "Alajuela", "Parque de los mangos");

        // assert
        Assert.That(_profile.GetEmail(), Is.EqualTo(email));
        _profile.ClickLogout();
        driver.Close();
        driver.Quit();
        
    }

    [Test]
    public void ChangePassword()
    {
        // arrange
        var driver = new ChromeDriver();
        _login = new Login(driver);
        _login.GoTo();
        _login.LoginAs(_loginData);
        WaitUntilUserIsLoggedIn(driver);
        _profile = new Profile(driver);
        
        // act
        _profile.GoTo();
        _profile.OpenChangePasswordModal();
        
        // assert
        Assert.That(_profile.ChechPasswordModalIsAsExpected(), Is.True);
        driver.Close();
        driver.Quit();
    }

    public void WaitUntilUserIsLoggedIn(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => _login.IsLoggedIn());
    }
}