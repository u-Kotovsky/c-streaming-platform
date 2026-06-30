using StreamingPlatformCore.Entities;

namespace StreamingPlatformCore.Services;

/// <summary>
/// Сервис для работы с текущим пользователем системы.
/// </summary>
public class UserService
{
    private readonly ApplicationContext _context;

    /// <summary>
    /// Текущий авторизованный пользователь (для простоты – первый в базе, либо создаётся тестовый).
    /// </summary>
    public User CurrentUser { get; private set; }

    /// <summary>
    /// Инициализирует сервис и определяет текущего пользователя.
    /// </summary>
    public UserService()
    {
        _context = ApplicationContext.GetInstance(
#if DEBUG
                true, true
#else
#endif
        );

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

    /// <summary>
    /// Позволяет сменить текущего пользователя (например, для демонстрации переключения).
    /// </summary>
    /// <param name="userId">Идентификатор нового пользователя.</param>
    public void SwitchUser(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user != null)
        {
            CurrentUser = user;
        }
    }
}