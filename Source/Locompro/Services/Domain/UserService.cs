﻿using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Factories;
using Locompro.Models.Results;

namespace Locompro.Services.Domain;

public class UserService : DomainService<User, string>, IUserService
{
    /// <summary>
    ///     The repository for performing user-related operations.
    /// </summary>
    private readonly IUserRepository _userRepository;
    
    private readonly IProductRepository _productRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="loggerFactory">The factory used to create loggers.</param>
    public UserService(
        IUnitOfWork unitOfWork,
        ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory)
    {
        _userRepository = UnitOfWork.GetSpecialRepository<IUserRepository>();
        _productRepository = UnitOfWork.GetSpecialRepository<IProductRepository>();
    }

    /// <inheritdoc/>
    public List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        return _userRepository.GetQualifiedUserIDs();
    }
    
    /// <inheritdoc />
    public int GetSubmissionsCountByUser(string userId)
    {
        return _userRepository.GetSubmissionsCountByUser(userId);
    }
    
    /// <inheritdoc />
    public int GetReportedSubmissionsCountByUser(string userId)
    {
        return _userRepository.GetReportedSubmissionsCountByUser(userId);
    }
    
    /// <inheritdoc />
    public int GetRatedSubmissionsCountByUser(string userId)
    {
        return _userRepository.GetRatedSubmissionsCountByUser(userId);
    }
    
    /// <inheritdoc />
    public List<MostReportedUsersResult> GetMostReportedUsersInfo()
    {
        var results = _userRepository.GetMostReportedUsersInfo();
        results = results.OrderByDescending(x => x.ReportedSubmissionCount).Take(10).ToList();
        return results;
    }

    /// <inheritdoc />
    public async Task<ShoppingListDto> GetShoppingList(string userId)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        
        List<Product> shoppingList = user.ShoppedProducts.ToList();
        
        ShoppingListProductFactory factory = new ShoppingListProductFactory();
        
        return new ShoppingListDto
        {
            UserId = userId,
            Products = shoppingList.Select(product => factory.ToDto(product)).ToList()
        };
    }

    /// <inheritdoc />
    public async Task AddProductToShoppingList(string userId, int productId)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        
        Product product = await _productRepository.GetByIdAsync(productId);
        if (product == null) throw new Exception("Product not found");
        
        user.ShoppedProducts.Add(product);
        await _userRepository.UpdateAsync(userId, user);
        await UnitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteProductFromShoppingList(string userId, int productId)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        
        Product product = await _productRepository.GetByIdAsync(productId);
        if (product == null) throw new Exception("Product not found");
        
        user.ShoppedProducts.Remove(product);
        await _userRepository.UpdateAsync(userId, user);
        await UnitOfWork.SaveChangesAsync();
    }
}