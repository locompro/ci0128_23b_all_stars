using System.Security.Claims;
using Locompro.Common.ErrorStore;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Pages.Account;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace Locompro.Tests.Pages.Account;

/// <summary>
///     This class contains unit tests for the ProfileModel class.
/// </summary>
[TestFixture]
public class ProfileModelTest
{
    /// <summary>
    ///     Initializes mock objects and a new instance of the ProfileModel class for each test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _userServiceMock.Reset();
        _authServiceMock.Reset();
        _cantonServiceMock.Reset();
        _moderationServiceMock.Reset();
        var errorCountUserUpdateModal = 0;
        _userDataModalStoreMock.Setup(store => store.StoreError(It.IsAny<string>()))
            .Callback(() => errorCountUserUpdateModal++);
        _userDataModalStoreMock.Setup(store => store.HasErrors).Returns(() => errorCountUserUpdateModal > 0);
        var errorCountPasswordModal = 0;
        _passwordModalStoreMock.Setup(store => store.StoreError(It.IsAny<string>()))
            .Callback(() => errorCountPasswordModal++);
        _passwordModalStoreMock.Setup(store => store.HasErrors).Returns(() => errorCountPasswordModal > 0);
    }

    private readonly Mock<IDomainService<User, string>> _userServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IDomainService<Canton, string>> _cantonServiceMock;
    private readonly Mock<IErrorStore> _passwordModalStoreMock;
    private readonly Mock<IErrorStore> _userDataModalStoreMock;
    private readonly Mock<IErrorStore> _declineModerationModalStoreMock;
    private readonly Mock<IUserManagerService> _userManagerServiceMock;
    private readonly Mock<IModerationService> _moderationServiceMock;

    private readonly ProfileModel _profileModel;

    public ProfileModelTest()
    {
        _userServiceMock = new Mock<IDomainService<User, string>>();
        _authServiceMock = new Mock<IAuthService>();
        _cantonServiceMock = new Mock<IDomainService<Canton, string>>();
        _userManagerServiceMock = new Mock<IUserManagerService>();
        _moderationServiceMock = new Mock<IModerationService>();
        Mock<IErrorStoreFactory> errorStoreFactoryMock = new();
        errorStoreFactoryMock.Setup(factory => factory.Create()).Returns(new Mock<IErrorStore>().Object);

        _profileModel = new ProfileModel(
            _userServiceMock.Object,
            _authServiceMock.Object,
            _cantonServiceMock.Object,
            errorStoreFactoryMock.Object,
            _userManagerServiceMock.Object,
            _moderationServiceMock.Object
        );

        _passwordModalStoreMock = new Mock<IErrorStore>();
        _userDataModalStoreMock = new Mock<IErrorStore>();
        _declineModerationModalStoreMock = new Mock<IErrorStore>();
        _profileModel.ChangePasswordModalErrors = _passwordModalStoreMock.Object;
        _profileModel.UpdateUserDataModalErrors = _userDataModalStoreMock.Object;
        _profileModel.DeclineModerationModalErrors = _declineModerationModalStoreMock.Object;
    }

    /// <summary>
    ///     Tests the OnGetAsync method by simulating a scenario where the user is not found,
    ///     and verifying the method redirects to the Login page.
    /// </summary>
    [Test]
    public async Task OnGetAsync_UserNotFound_RedirectsToLoginPage()
    {
        // Arrange
        _userManagerServiceMock
            .Setup(us => us.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _profileModel.OnGetAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
    }

    /// <summary>
    ///     Tests the OnGetAsync method by simulating a scenario where the user is found
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnGetAsync_UserFound_ReturnsPage()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(ums => ums.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        // Instantiate a new TempDataDictionary
        var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

        // Set the TempData property on your ProfileModel instance
        _profileModel.TempData = tempData;

        // Act
        var result = await _profileModel.OnGetAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<PageResult>());
    }


    /// <summary>
    ///     Tests the OnPostChangePasswordAsync method by simulating a scenario where the user is not found,
    ///     and verifying the method redirects to the Login page.
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnPostChangePasswordAsync_UserNotFound_RedirectsToLoginPage()
    {
        // Arrange
        _userManagerServiceMock
            .Setup(us => us.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _profileModel.OnPostChangePasswordAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
    }

    // Test for invalid password change
    [Test]
    public async Task OnPostChangePasswordAsync_InvalidCurrentPassword_RendersPageWithError()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _authServiceMock.Setup(auth => auth.IsCurrentPasswordCorrect(It.IsAny<string>())).ReturnsAsync(false);
        _profileModel.PasswordChange = new PasswordChangeVm
        {
            CurrentPassword = "incorrectPassword",
            NewPassword = "newPassword123",
            ConfirmNewPassword = "newPassword123"
        };

        // Act
        var result = await _profileModel.OnPostChangePasswordAsync();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_passwordModalStoreMock.Object.HasErrors, Is.EqualTo(true));
        });
    }

    // Test for valid password change
    [Test]
    public async Task OnPostChangePasswordAsync_ValidPasswordChange_RedirectsToPage()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _authServiceMock.Setup(auth => auth.IsCurrentPasswordCorrect(It.Is<string>(s => s == "correctPassword")))
            .ReturnsAsync(true);
        _authServiceMock.Setup(auth =>
            auth.ChangePassword(It.Is<string>(s => s == "correctPassword"), It.Is<string>(s => s == "newPassword123")));

        _profileModel.PasswordChange = new PasswordChangeVm
        {
            CurrentPassword = "correctPassword",
            NewPassword = "newPassword123",
            ConfirmNewPassword = "newPassword123"
        };

        // Act
        var result = await _profileModel.OnPostChangePasswordAsync();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(_passwordModalStoreMock.Object.HasErrors, Is.EqualTo(false));
        });
        _authServiceMock.Verify(
            auth => auth.ChangePassword(It.Is<string>(s => s == "correctPassword"),
                It.Is<string>(s => s == "newPassword123")), Times.Once);
    }


    /// <summary>
    ///     Tests the OnPostUpdateUserDataAsync method by simulating a scenario where the user is not found,
    ///     and verifying the method redirects to the Login page.
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnPostUpdateUserDataAsync_UserNotFound_RedirectsToLoginPage()
    {
        // Arrange
        _userManagerServiceMock
            .Setup(us => us.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _profileModel.OnPostUpdateUserDataAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
    }

    [Test]
    public async Task OnPostUpdateUserDataAsync_UserFound_ValidUpdate()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        var userDataUpdate = new UserDataUpdateVm
        {
            Email = "a@a.com",
            Province = " San jose",
            Canton = "Mentesdeoca",
            ExactAddress = "quinto piso edificio anexo ecci para saltar al vacio"
        };
        _profileModel.UserDataUpdate = userDataUpdate;

        // Act
        var result = await _profileModel.OnPostUpdateUserDataAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        _userServiceMock.Verify(us => us.Update(It.IsAny<string>(), It.Is<User>(u => u.Id == user.Id)), Times.Once);
        _userServiceMock.Verify(us => us.Update(It.IsAny<string>(), It.Is<User>(u => u.Email == userDataUpdate.Email)), Times.Once);
        _userServiceMock.Verify(us => us.Update(It.IsAny<string>(), It.Is<User>(u => u.Address == userDataUpdate.GetAddress())),
            Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(_profileModel.TempData["IsUserDataUpdated"], Is.EqualTo(true));
            Assert.That(_userDataModalStoreMock.Object.HasErrors, Is.EqualTo(false));
        });
    }

    [Test]
    public async Task OnPostUpdateUserDataAsync_UserFound_InvalidUpdate()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userServiceMock.Setup(us => us.Get(It.IsAny<string>())).ReturnsAsync(user);

        var userDataUpdate = new UserDataUpdateVm
        {
            Email = null,
            Province = "San Jose",
            Canton = null,
            ExactAddress = null
        };
        _profileModel.UserDataUpdate = userDataUpdate;

        // Act
        var result = await _profileModel.OnPostUpdateUserDataAsync();
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_profileModel.TempData["IsUserDataUpdated"], Is.Null.Or.EqualTo(false));
            Assert.That(_userDataModalStoreMock.Object.HasErrors, Is.EqualTo(true));
        });
        _userServiceMock.Verify(us => us.Update(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _userDataModalStoreMock.Verify(store => store.StoreError(It.IsAny<string>()), Times.AtLeastOnce);
    }

    /// <summary>
    ///     Tests the OnGetCantonsAsync method by simulating a scenario where the province is null or empty,
    ///     and verifying the method returns an empty list.
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnGetCantonsAsync_ProvinceNullOrEmpty_ReturnsEmptyList()
    {
        // Arrange
        var province = string.Empty;

        // Act
        var result = await _profileModel.OnGetCantonsAsync(province);

        // Assert
        Assert.That(result, Is.Not.Null);
        var cantons = result.Value as List<CantonDto>;
        Assert.That(cantons, Is.Not.Null);
        Assert.That(cantons!, Is.Empty);
    }

    /// <summary>
    ///     Tests the OnPostChangePasswordAsync method by simulating a scenario where the current password is incorrect,
    ///     and verifying the method re-renders the page with error messages.
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnPostChangePasswordAsync_IncorrectCurrentPassword_RendersPageWithError()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _authServiceMock.Setup(auth => auth.IsCurrentPasswordCorrect(It.IsAny<string>())).ReturnsAsync(false);
        _profileModel.PasswordChange = new PasswordChangeVm
        {
            CurrentPassword = "incorrectPassword",
            NewPassword = "newPassword123"
        };

        // Act
        var result = await _profileModel.OnPostChangePasswordAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<PageResult>());
        _passwordModalStoreMock.Verify(store => store.StoreError("La contraseña actual no es correcta."), Times.Once);
    }

    /// <summary>
    ///     Tests the OnPostUpdateUserDataAsync method by simulating a scenario where the update data is invalid,
    ///     and verifying the method re-renders the page with an error message.
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnPostUpdateUserDataAsync_InvalidUpdateData_RendersPageWithError()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _profileModel.UserDataUpdate = new UserDataUpdateVm(); // Assume IsUpdateValid() returns false

        // Act
        var result = await _profileModel.OnPostUpdateUserDataAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<PageResult>());
        _userDataModalStoreMock.Verify(store => store.StoreError(It.IsAny<string>()), Times.AtLeastOnce);
    }

    /// <summary>
    ///     Tests the OnGetCantonsAsync method by simulating a scenario where a valid province is specified,
    ///     and verifying the method returns a list of cantons.
    /// </summary>
    /// <author> A.Badila-Olivas b80874 </author>
    [Test]
    public async Task OnGetCantonsAsync_ValidProvince_ReturnsListOfCantons()
    {
        // Arrange
        var province = "San Jose";
        var cantonList = new List<Canton>
        {
            new() { Name = "Canton1", ProvinceName = province, Province = new Province { Name = province } },
            new() { Name = "Canton2", ProvinceName = province, Province = new Province { Name = province } }
        };
        _cantonServiceMock.Setup(cs => cs.GetAll()).ReturnsAsync(cantonList);

        // Act
        var result = await _profileModel.OnGetCantonsAsync(province);

        // Assert
        var jsonResult = result;
        Assert.That(jsonResult, Is.Not.Null);
        var returnedCantonList = jsonResult.Value as List<CantonDto>;
        Assert.That(returnedCantonList, Is.Not.Null);
        if (returnedCantonList != null)
        {
            Assert.That(returnedCantonList, Has.Count.EqualTo(cantonList.Count));
            Assert.Multiple(() =>
            {
                Assert.That(returnedCantonList[0].Name, Is.EqualTo(cantonList[0].Name));
                Assert.That(returnedCantonList[1].Name, Is.EqualTo(cantonList[1].Name));
            });
        }
    }

    // Test for successful moderation role declination
    [Test]
    public async Task OnPostDeclineModerationAsync_SuccessfulDeclination_RedirectsToPage()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _authServiceMock.Setup(auth => auth.IsCurrentPasswordCorrect(It.IsAny<string>())).ReturnsAsync(true);
        _userManagerServiceMock.Setup(ums => ums.DeleteClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()));
        _userManagerServiceMock.Setup(ums => ums.AddClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()));
        _authServiceMock.Setup(auth => auth.RefreshUserLogin());

        // Set the current password to something
        _profileModel.PasswordChange = new PasswordChangeVm
        {
            CurrentPassword = "correctPassword"
        };

        // Act
        var result = await _profileModel.OnPostDeclineModerationAsync();

        // Assert
        _userManagerServiceMock.Verify(ums => ums.DeleteClaimAsync(user, It.IsAny<Claim>()), Times.Once);
        _userManagerServiceMock.Verify(ums => ums.AddClaimAsync(user, It.IsAny<Claim>()), Times.Once);
        _authServiceMock.Verify(auth => auth.RefreshUserLogin(), Times.Once);
        Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
    }

// Test for user not found
    [Test]
    public async Task OnPostDeclineModerationAsync_UserNotFound_RedirectsToLogin()
    {
        // Arrange
        _userManagerServiceMock.Setup(ums => ums.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        // Act
        var result = await _profileModel.OnPostDeclineModerationAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToRouteResult>());
        var redirectToRouteResult = (RedirectToRouteResult)result;
        Assert.That(redirectToRouteResult.RouteName, Is.EqualTo("Account/Login"));
    }

// Test for incorrect password
    [Test]
    public async Task OnPostDeclineModerationAsync_IncorrectPassword_RendersPageWithError()
    {
        // Arrange
        var user = CreateFakeUserDefault();
        _userManagerServiceMock.Setup(us => us.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _authServiceMock.Setup(auth => auth.IsCurrentPasswordCorrect(It.IsAny<string>())).ReturnsAsync(false);

        _profileModel.DeclineRoleUpdate = new DeclineModeratorViewModel
        {
            CurrentPassword = "incorrectPassword"
        };

        // Act
        var result = await _profileModel.OnPostDeclineModerationAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<PageResult>());
        _declineModerationModalStoreMock.Verify(store => store.StoreError(It.IsAny<string>()), Times.Once);
    }

    private User CreateFakeUserDefault()
    {
        var user = new User
        {
            Id = "user1", Email = "some@email.com", Rating = 1, Name = "Name", Address = "some address",
            Submissions = new List<Submission>()
        };
        return user;
    }
}