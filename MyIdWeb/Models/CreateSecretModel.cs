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

    public string Memo1Line
    {
        get
        {
            string memo = "";
            if (!string.IsNullOrWhiteSpace(Memo))
            {
                memo = Memo.Replace("\n", " ");
            }
            if (Images != null && Images.Count > 0)
                memo = $"({Images.Count} file{(Images.Count == 1 ? "" : "s")}) {memo}";
            return memo;
        }
    }

    public Dictionary<string,string> Images { get; set; }

    public string GetImageContent(string fileName)
    {
        var fileData = Images[fileName];
        string type = Path.GetExtension(fileName);
        type = type.Trim('.');
        return $"data:image/{type};base64, " + fileData;
    }
}

