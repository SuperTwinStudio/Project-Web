using System;

public class CloseAction : IMenuAction
{
    Action action;

    public CloseAction(Action _action)
    {
        action = _action;
    }

    public void Execute()
    {
        action.Invoke();
    }
}
