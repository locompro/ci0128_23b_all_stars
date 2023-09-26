﻿using Locompro.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    [StringLength(40, MinimumLength = 1)]
    public string Name { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; }

    public float Rating { get; set; } = 0;

    public Status Status { get; set; } = Status.Active;
}
