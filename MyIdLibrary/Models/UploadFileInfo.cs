
namespace MyIdLibrary.Models;

public class UploadFileInfo
{
    public string FileKey { get; set; } //User's encrypted file name
    public string FileName { get; set; } //User's original file name
    public string FileData { get; set; } //base64 encode bytes  

    public string FileContent
    {
        get
        {
            string type = Path.GetExtension(FileName);
            type = type.Trim('.');
            return $"data:image/{type};base64, " + FileData;
        }
    }
}
