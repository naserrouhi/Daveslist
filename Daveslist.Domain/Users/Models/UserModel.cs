using Daveslist.Domain.Shared.Models;

namespace Daveslist.Domain.Users.Models;

public class UserModel : AggregateRoot<int>
{
    protected UserModel() { }

    public UserModel(string username,
                     string email,
                     string firstName,
                     string lastName)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        CreatedDate = DateTime.UtcNow;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedDate { get; set; }
}
