namespace MyIdLibrary.Models;

public class SecretModel
{
    public Guid Id { get; set; }
    public string RecordId { get; set; }

    public string Payload { get; set; }  //Payload is serialized IdItem

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified {  get; set; }
    public DateTime Deleted { get; set; }
    public DateTime Synced { get; set; }

    public HashSet<string> UserIds { get; set; } = new();

    public string SecretKey { get; set; }
}
