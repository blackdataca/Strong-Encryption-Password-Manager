using System.Runtime.CompilerServices;

namespace MyIdLibrary.Models;

public class SecretModel
{
    public Guid Id { get; set; }
    public string RecordId { get; set; }

    public string Payload { get; set; }  //Payload is serialized IdItem

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified {  get; set; } = DateTime.UtcNow;
    public bool Deleted { get; set; }
    public DateTime Synced { get; set; }

    public HashSet<string> UserIds { get; set; } = new();

    public string SecretKey { get; set; }

}
