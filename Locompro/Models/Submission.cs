using Microsoft.Build.Framework;

namespace Locompro.Models;

public class Submission
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public DateTime EntryTime { get; set; }
    
    [Required]
    public Status Status { get; set; }
    
    [Required]
    public int Price { get; set; }
    
    public float Rating { get; set; }
    
    public int Description { get; set; }
    
    [Required]
    public string StoreName { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    public virtual Store Store { get; set; }
    
    public virtual Product Product { get; set; }
}