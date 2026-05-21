using StreamingPlatformCore.Entities;

namespace StreamingPlatformCore.Services;

public class UserService
{
    private readonly ApplicationContext _context;
    public User CurrentUser { get; private set; }

    public UserService()
    {
        _context = ApplicationContext.GetInstance();

        if (_context.Users.Any())
        {
            CurrentUser = _context.Users.First();
        }
        else
        {
            CurrentUser = new User("test", "test");
            _context.Users.Add(CurrentUser);
            _context.SaveChanges();
        }
    }
}