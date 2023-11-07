using System.Linq.Expressions;
using Locompro.Common.Search;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Services.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Locompro.Tests.Services;

[TestFixture]
public class SubmissionServiceTest
{
    [SetUp]
    public void Setup()
    {
        var loggerFactoryMock = new Mock<ILoggerFactory>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _submissionCrudRepositoryMock = new Mock<ISubmissionRepository>();

        _unitOfWorkMock
            .Setup(unit => unit.GetSpecialRepository<ISubmissionRepository>())
            .Returns(_submissionCrudRepositoryMock.Object);

        _submissionService = new SubmissionService(_unitOfWorkMock.Object, loggerFactoryMock.Object);
    }

    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<ISubmissionRepository> _submissionCrudRepositoryMock = null!;
    private SubmissionService _submissionService = null!;

    /// <summary>
    ///     tests that the search by canton and province returns the expected results when the canton and province are
    ///     mentioned in the submissions
    ///     <author> A. Badilla Olivas B80874 </author>
    /// </summary>
    [Test]
    public async Task GetSubmissionsByCantonAndProvince_ValidCantonAndProvince_SubmissionsReturned()
    {
        // Arrange
        var canton = "Canton1";
        var province = "Province1";
        MockDataSetup();

        var searchQueries = new SearchQueries
        {
            SearchQueryFunctions = new List<Expression<Func<Submission, bool>>>
            {
                submission => submission.Store.Canton.Name == canton,
                submission => submission.Store.Canton.ProvinceName == province
            }
        };

        // Act
        var results = await _submissionService.GetSearchResults(searchQueries);

        // Assert
        var submissions = results as Submission[] ?? results.ToArray();
        Assert.That(submissions, Is.Not.Null);
        Assert.That(submissions.Count(), Is.GreaterThan(0));
        var all = true;
        foreach (var sub in submissions)
            if (sub.Store.Canton.Name != canton || sub.Store.Canton.ProvinceName != province)
            {
                all = false;
                break;
            }

        Assert.That(all, Is.True);
    }

    /// <summary>
    ///     Tests that an empty list is returned when the canton and province are not mentioned in any submission
    ///     <author> A. Badilla Olivas B80874 </author>
    /// </summary>
    [Test]
    public async Task GetSubmissionsByCantonAndProvince_InvalidCantonAndProvince_EmptyListReturned()
    {
        // Arrange
        var canton = "InvalidCanton";
        var province = "InvalidProvince";
        MockDataSetup();

        // Act
        var searchQueries = new SearchQueries
        {
            SearchQueryFunctions = new List<Expression<Func<Submission, bool>>>
            {
                submission => submission.Store.Canton.Name == canton,
                submission => submission.Store.Canton.ProvinceName == province
            }
        };

        var results = await _submissionService.GetSearchResults(searchQueries);

        // Assert
        var submissions = results as Submission[] ?? results.ToArray();
        Assert.That(submissions, Is.Not.Null);
        Assert.That(submissions.Count(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that the search by store name returns the expected results when
    /// the store name is mentioned in the submissions
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task AddingFirstRatingToSubmissionResultsInNewSubmissionRatingBeingSame()
    {
        MockDataSetup();
        var newRating = new RatingVm
        {
            SubmissionUserId = "User1",
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = "5"
        };

        await _submissionService.UpdateSubmissionRating(newRating);

        var changedSubmission = await _submissionCrudRepositoryMock.Object.GetByIdAsync(new SubmissionKey
        {
            UserId = "User1",
            EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc)
        });

        Assert.That(changedSubmission.Rating, Is.EqualTo(5));
    }

    /// <summary>
    /// Tests that the average of the ratings is calculated correctly
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task AddingSecondRatingToSubmissionResultsInNewSubmissionRatingBeingAverage()
    {
        MockDataSetup();
        var newRating = new RatingVm
        {
            SubmissionUserId = "User1",
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = "5"
        };

        await _submissionService.UpdateSubmissionRating(newRating);

        newRating.Rating = "3";
        await _submissionService.UpdateSubmissionRating(newRating);

        var changedSubmission = await _submissionCrudRepositoryMock.Object.GetByIdAsync(new SubmissionKey
        {
            UserId = "User1",
            EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc)
        });

        Assert.That(changedSubmission.Rating, Is.EqualTo(4));
    }

    /// <summary>
    /// Tests that an exception is thrown when the rating view model is null or
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task AddingRatingOnNonExistentSubmissionThrowsException()
    {
        MockDataSetup();
        var newRating = new RatingVm
        {
            SubmissionUserId = "User1",
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = "5"
        };

        await _submissionService.UpdateSubmissionRating(newRating);

        newRating.SubmissionUserId = "User2";
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _submissionService.UpdateSubmissionRating(newRating));
    }

    /// <summary>
    /// Tests that an exception is thrown when the rating view model is null or
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task AddingNullRatingComponentThrowsException()
    {
        MockDataSetup();
        var newRating = new RatingVm
        {
            SubmissionUserId = "User1",
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = "5"
        };

        await _submissionService.UpdateSubmissionRating(newRating);

        newRating.Rating = null;
        Assert.ThrowsAsync<ArgumentException>(async () => await _submissionService.UpdateSubmissionRating(newRating));
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the rating view model is null or
    /// the rating string is not a number between 1 and 5
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task AddingRatingComponentWithInvalidRatingThrowsException()
    {
        MockDataSetup();
        var newRating = new RatingVm
        {
            SubmissionUserId = "User1",
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = "5"
        };

        await _submissionService.UpdateSubmissionRating(newRating);


        Assert.Multiple(() =>
        {
            newRating.Rating = "6";
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _submissionService.UpdateSubmissionRating(newRating));
            newRating.Rating = "0";
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _submissionService.UpdateSubmissionRating(newRating));
        });
    }

    /// <summary>
    /// Tests that an exception is thrown when the rating view model is null or the rating string
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public void AddingRatingWithInvalidParametersThrowsException()
    {
        MockDataSetup();
        var newRating = new RatingVm
        {
            SubmissionUserId = null,
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = "5"
        };

        var newRating1 = new RatingVm
        {
            SubmissionUserId = "User1",
            SubmissionEntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
            Rating = null
        };

        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _submissionService.UpdateSubmissionRating(newRating));
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _submissionService.UpdateSubmissionRating(newRating1));
        });
    }

    /// <summary>
    /// For a corner case where there is a rating and somehow the amount of ratings is 0
    /// Would usually happen only for seeded data
    /// <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task AddRatingOnSubmissionWithRatingButZeroRatingsAmountResultsInValidAverage()
    {
        MockDataSetup();

        var newRating = new RatingVm
        {
            SubmissionUserId = "User8",
            SubmissionEntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
            Rating = "3"
        };

        await _submissionService.UpdateSubmissionRating(newRating);

        var changedSubmission = await _submissionCrudRepositoryMock.Object.GetByIdAsync(new SubmissionKey
        {
            UserId = "User8",
            EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc)
        });

        Assert.That(changedSubmission.Rating, Is.EqualTo(3.6500001f));
    }



    /// <summary>
    ///     Sets up the mock for the submission repository so that it behaves as expected for the tests
    /// </summary>
    private void MockDataSetup()
    {
        var country = new Country { Name = "Country" };

        var province1 = new Province { Name = "Province1", CountryName = "Country", Country = country };
        var province2 = new Province { Name = "Province2", CountryName = "Country", Country = country };

        var canton1 = new Canton { Name = "Canton1", ProvinceName = "Province1", Province = province1 };
        var canton2 = new Canton { Name = "Canton2", ProvinceName = "Province2", Province = province2 };

        // Add users
        var users = new List<User>
        {
            new()
            {
                Name = "User1"
            },
            new()
            {
                Name = "User2"
            },
            new()
            {
                Name = "User3"
            }
        };

        // Add stores
        var stores = new List<Store>
        {
            new()
            {
                Name = "Store1",
                Canton = canton1,
                Address = "Address1",
                Telephone = "Telephone1"
            },
            new()
            {
                Name = "Store2",
                Canton = canton1,
                Address = "Address2",
                Telephone = "Telephone2"
            },
            new()
            {
                Name = "Store3",
                Canton = canton2,
                Address = "Address3",
                Telephone = "Telephone3"
            },
            new()
            {
                Name = "Store4",
                Canton = canton2,
                Address = "Address4",
                Telephone = "Telephone4"
            }
        };

        // Add products
        var products = new List<Product>
        {
            new()
            {
                Id = 1,
                Name = "Product1",
                Model = "Model1",
                Brand = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Product2",
                Model = "Model2",
                Brand = "Brand2"
            },
            new()
            {
                Id = 3,
                Name = "Product3",
                Model = "Model3",
                Brand = "Brand3"
            },
            new()
            {
                Id = 4,
                Name = "Product4",
                Model = "Model4",
                Brand = "Brand4"
            },
            new()
            {
                Id = 5,
                Name = "Product5",
                Model = "Model5",
                Brand = "Brand5"
            },
            new()
            {
                Id = 6,
                Name = "Product6",
                Model = "Model6",
                Brand = "Brand6"
            },
            new()
            {
                Id = 7,
                Name = "Product7",
                Model = "Model7",
                Brand = "Brand7"
            }
        };

        // Add submissions
        var submissions = new List<Submission>
        {
            new()
            {
                UserId = "User1",
                EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
                Price = 100,
                Rating = 0f,
                Description = "Description for Submission 1",
                StoreName = "Store1",
                ProductId = 1,
                User = users[0],
                Store = stores[0],
                Product = products[0]
            },
            new()
            {
                UserId = "User2",
                EntryTime = DateTime.Now.AddDays(-1),
                Price = 200,
                Rating = 3.8f,
                Description = "Description for Submission 2",
                StoreName = "Store2",
                ProductId = 2,
                User = users[1],
                Store = stores[1],
                Product = products[1]
            },
            new()
            {
                UserId = "User3",
                EntryTime = DateTime.Now.AddDays(-3),
                Price = 50,
                Rating = 4.2f,
                Description = "Description for Submission 3",
                StoreName = "Store3",
                ProductId = 3,
                User = users[2],
                Store = stores[2],
                Product = products[2]
            },
            new()
            {
                UserId = "User4",
                EntryTime = DateTime.Now.AddDays(-4),
                Price = 150,
                Rating = 4.0f,
                Description = "Description for Submission 4",
                StoreName = "Store4",
                ProductId = 4,
                User = users[0],
                Store = stores[0],
                Product = products[3]
            },
            new()
            {
                UserId = "User5",
                EntryTime = DateTime.Now.AddDays(-5),
                Price = 75,
                Rating = 3.9f,
                Description = "Description for Submission 5",
                StoreName = "Store5",
                ProductId = 5,
                User = users[1],
                Store = stores[1],
                Product = products[4]
            },
            new()
            {
                UserId = "User6",
                EntryTime = DateTime.Now.AddDays(-6),
                Price = 220,
                Rating = 4.6f,
                Description = "Description for Submission 6",
                StoreName = "Store6",
                ProductId = 6,
                User = users[2],
                Store = stores[2],
                Product = products[5]
            },
            new()
            {
                UserId = "User7",
                EntryTime = DateTime.Now.AddDays(-7),
                Price = 90,
                Rating = 3.7f,
                Description = "Description for Submission 7",
                StoreName = "Store7",
                ProductId = 7,
                User = users[0],
                Store = stores[0],
                Product = products[6]
            },
            new()
            {
                UserId = "User8",
                EntryTime = new DateTime(2023, 10, 5, 12, 0, 0, DateTimeKind.Utc),
                Price = 180,
                Rating = 4.3f,
                Description = "Description for Submission 8",
                StoreName = "Store1",
                ProductId = 1,
                User = users[0],
                Store = stores[0],
                Product = products[0]
            },
            new()
            {
                UserId = "User9",
                EntryTime = DateTime.Now.AddDays(-9),
                Price = 120,
                Rating = 4.1f,
                Description = "Description for Submission 9",
                StoreName = "Store9",
                ProductId = 9,
                User = users[2],
                Store = stores[2],
                Product = products[1]
            },
            new()
            {
                UserId = "User10",
                EntryTime = DateTime.Now.AddDays(-10),
                Price = 70,
                Rating = 3.5f,
                Description = "Description for Submission 10",
                StoreName = "Store10",
                ProductId = 10,
                User = users[0],
                Store = stores[0],
                Product = products[2]
            },
            new()
            {
                UserId = "User11",
                EntryTime = DateTime.Now.AddDays(-11),
                Price = 110,
                Rating = 4.4f,
                Description = "Description for Submission 11",
                StoreName = "Store11",
                ProductId = 11,
                User = users[1],
                Store = stores[1],
                Product = products[3]
            },
            new()
            {
                UserId = "User12",
                EntryTime = DateTime.Now.AddDays(-12),
                Price = 240,
                Rating = 4.8f,
                Description = "Description for Submission 12",
                StoreName = "Store12",
                ProductId = 12,
                User = users[2],
                Store = stores[2],
                Product = products[4]
            },
            new()
            {
                UserId = "User13",
                EntryTime = DateTime.Now.AddDays(-13),
                Price = 85,
                Rating = 3.6f,
                Description = "Description for Submission 13",
                StoreName = "Store13",
                ProductId = 13,
                User = users[0],
                Store = stores[0],
                Product = products[5]
            },
            new()
            {
                UserId = "User14",
                EntryTime = DateTime.Now.AddDays(-14),
                Price = 130,
                Rating = 4.0f,
                Description = "Description for Submission 14",
                StoreName = "Store14",
                ProductId = 14,
                User = users[1],
                Store = stores[1],
                Product = products[6]
            },
            new()
            {
                UserId = "User15",
                EntryTime = DateTime.Now.AddDays(-15),
                Price = 190,
                Rating = 4.2f,
                Description = "Description for Submission 15",
                StoreName = "Store2",
                ProductId = 1,
                User = users[2],
                Store = stores[2],
                Product = products[0]
            },
            new()
            {
                UserId = "User16",
                EntryTime = DateTime.Now.AddDays(-16),
                Price = 65,
                Rating = 3.4f,
                Description = "Description for Submission 16",
                StoreName = "Store16",
                ProductId = 16,
                User = users[0],
                Store = stores[0],
                Product = products[1]
            },
            new()
            {
                UserId = "User17",
                EntryTime = DateTime.Now.AddDays(-17),
                Price = 160,
                Rating = 4.1f,
                Description = "Description for Submission 17",
                StoreName = "Store17",
                ProductId = 17,
                User = users[1],
                Store = stores[1],
                Product = products[2]
            },
            new()
            {
                UserId = "User18",
                EntryTime = DateTime.Now.AddDays(-18),
                Price = 210,
                Rating = 4.6f,
                Description = "Description for Submission 18",
                StoreName = "Store18",
                ProductId = 18,
                User = users[2],
                Store = stores[2],
                Product = products[3]
            },
            new()
            {
                UserId = "User19",
                EntryTime = DateTime.Now.AddDays(-19),
                Price = 80,
                Rating = 3.7f,
                Description = "Description for Submission 19",
                StoreName = "Store19",
                ProductId = 19,
                User = users[0],
                Store = stores[0],
                Product = products[4]
            },
            new()
            {
                UserId = "User20",
                EntryTime = DateTime.Now.AddDays(-20),
                Price = 140,
                Rating = 3.9f,
                Description = "Description for Submission 20",
                StoreName = "Store20",
                ProductId = 20,
                User = users[1],
                Store = stores[1],
                Product = products[5]
            }
        };

        _submissionCrudRepositoryMock
            .Setup(repository => repository.GetSearchResults(It.IsAny<SearchQueries>()))
            .ReturnsAsync((SearchQueries searchQueries) =>
            {
                // initiate the query
                IQueryable<Submission> submissionsResults = submissions.AsQueryable()
                    .Include(submission => submission.Product);

                // append the search queries to the query
                submissionsResults =
                    searchQueries.SearchQueryFunctions.Aggregate(submissionsResults,
                        (current, query) => current.Where(query));

                // get and return the results
                return submissionsResults;
            });

        _submissionCrudRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<SubmissionKey>()))
            .ReturnsAsync((SubmissionKey submissionKey) =>
            {
                return submissions.SingleOrDefault(submission => submission.UserId == submissionKey.UserId &&
                                                                 submission.EntryTime == submissionKey.EntryTime);
            });
    }
}