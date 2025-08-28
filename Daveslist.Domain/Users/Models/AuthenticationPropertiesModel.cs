namespace Daveslist.Domain.Users.Models;

public class AuthenticationPropertiesModel
{
    public string? RedirectUri { get; set; }
    public IDictionary<string, string?> Items { get; set; } = new Dictionary<string, string?>();
    public IList<string> Parameters { get; set; } = [];
}
