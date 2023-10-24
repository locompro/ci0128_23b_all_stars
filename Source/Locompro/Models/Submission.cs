using System;
using Microsoft.Build.Framework;

namespace Locompro.Models;

/// <summary>
/// A price submission by a user.
/// </summary>
public class Submission
{
    [Required]
    public string Username { get; set; }
    // TODO: Rename to UserID
    
    [Required]
    public DateTime EntryTime { get; set; }
    
    [Required]
    public Status Status { get; set; } = Status.Active;
    
    [Required]
    public int Price { get; set; }
    
    public float Rating { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public string StoreName { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    public virtual User User { get; set; }
    
    public virtual Store Store { get; set; }
    
    public virtual Product Product { get; set; }
    
    // TODO: Add pictures to submission
}