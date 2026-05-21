using System.Windows.Input;

namespace StreamingPlatform.Helpers;

/// <summary>
/// Helper class to receive UI events and execute functions.
/// </summary>
public class RelayCommand : ICommand // I just missed this interface and spent a few hours figuring out why events don't come ... buh
{
    private readonly Action<object?> execute;
    private readonly Func<object?, bool>? canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);
    public void Execute(object? parameter) => execute(parameter);
}