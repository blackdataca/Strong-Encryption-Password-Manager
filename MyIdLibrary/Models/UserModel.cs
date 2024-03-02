using System.ComponentModel.DataAnnotations.Schema;

namespace MyIdLibrary.Models;

public class UserModel
{
    public string Id { get; set; } //ObjectIdentifier
    public string Name { get; set; }
    public string Email { get; set; }

    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public int Status { get; set; }
    public int Perference { get; set; }
    public int IdleTimeout { get; set; }
    public DateTime LastActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Expiry { get; set; }
}
