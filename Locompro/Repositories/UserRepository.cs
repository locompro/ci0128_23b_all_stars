﻿using Locompro.Data;
using Locompro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories;

/// <summary>
/// Repository for Country entities.
/// </summary>
public class UserRepository : AbstractRepository<User, string>
{
    /// <summary>
    /// Constructs a Country repository for a given context.
    /// </summary>
    /// <param name="context">Context to base the repository on.</param>
    /// <param name="loggerFactory">Factory for repository logger.</param>
    public UserRepository(LocomproContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }
}