using System.ComponentModel.DataAnnotations;

namespace MyIdCloud.Data;

public class ShareWith
{
    [EmailAddress]
    public string? Email { get; set; }
}
