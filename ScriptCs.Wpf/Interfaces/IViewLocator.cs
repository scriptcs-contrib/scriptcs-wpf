namespace ScriptCs.Wpf.Interfaces
{
    public interface IViewLocator
    {
        string LocateViewFor<T>() where T : class;

        string LocateViewFor(string viewModelName);
    }
}