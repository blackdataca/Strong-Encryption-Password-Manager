using System.ComponentModel.DataAnnotations;

namespace MyIdWeb.Models;

public class SecretDetailModel
{
    public string Id { get; set; }
    public DateTime Deleted { get; set; } = DateTime.MinValue;

    [Required]
    [MaxLength(50)]
    public string Site { get; set; }

    [MaxLength(50)]
    [Display(Name = "User")]
    public string UserName { get; set; }

    [MaxLength(50)]
    public string Password { get; set; }

    [MaxLength(500)]
    public string Memo { get; set; }

    public Dictionary<string, string> Images { get; set; }

}
