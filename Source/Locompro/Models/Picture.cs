using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models;

public class Picture
{
    [Required]
    public string SubmissionUserId { get; set; }
    
    [Required]
    public DateTime SubmissionEntryTime { get; set; }
    
    [Required]
    public int Index { get; set; }
    public string PictureTitle { get; set; }
    public byte[] PictureData { get; set; }
    
    public virtual Submission Submission { get; set; }
}