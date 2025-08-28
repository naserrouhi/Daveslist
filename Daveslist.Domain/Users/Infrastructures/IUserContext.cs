namespace Daveslist.Domain.Users.Infrastructures;

public interface IUserContext
{
    int? GetCurrentUserId();
}
