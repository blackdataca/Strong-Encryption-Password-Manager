using System.ComponentModel.DataAnnotations;

namespace MyIdWeb.Models;

public class SecretDetailModel
{
    public string Id { get; set; }
    public bool Deleted { get; set; } 

    [Required]
    [MaxLength(50)]
    public string Site { get; set; }

    [MaxLength(50)]
    public string User { get; set; }

    [MaxLength(50)]
    public string Password { get; set; }

    [MaxLength(500)]
    public string Memo { get; set; }

    public Dictionary<string, string> Images { get; set; }

}
