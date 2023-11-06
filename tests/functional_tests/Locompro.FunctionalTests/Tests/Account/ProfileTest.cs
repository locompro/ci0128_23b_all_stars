using Locompro.FunctionalTests.PageObjects.Account;
using Locompro.Models.Entities;
using OpenQA.Selenium.Chrome;

namespace Locompro.FunctionalTests.Tests.Account;

public class ProfileTest
{
    class UserData
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    private readonly Register _register = new(new ChromeDriver());
    private readonly Login _login = new(new ChromeDriver());
    private readonly Profile _profile = new(new ChromeDriver());

    private readonly UserData _userData = new()
    {
        Email = "Juan@juan",
        Username = "Juan",
        Password = "Password@123"
    };


    [SetUp]
    public void Setup()
    {
        _register.Driver.Navigate().GoToUrl("https://localhost:7249/Account/Register");
        _register.RegisterAs(_userData.Email, _userData.Username, _userData.Password, _userData.Password);
        _login.Driver.Navigate().GoToUrl("https://localhost:7249/Account/Login");
        _login.LoginAs(_userData.Username, _userData.Password);
    }

    [Test]
    public void UserDataIsCorrect()
    {
        _profile.GoTo();

        Assert.Multiple(() =>
        {
            Assert.That(_profile.GetUserName(), Is.EqualTo(_userData.Username));
            Assert.That(_profile.GetEmail(), Is.EqualTo(_userData.Email));
            Assert.That(_profile.GetAddress(), Is.Empty.Or.Null);
            Assert.That(_profile.GetContributions(), Is.EqualTo(0));
            Assert.That(_profile.GetRating(), Is.EqualTo(0));
        });
    }
}  