namespace ALeRT.PluginFramework
{
    public interface IQueryPlugin
    {
        string PluginCategory { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        bool IsTypeAcceptable(string type); 
    }

    public interface IQueryPluginRBool : IQueryPlugin
    {
        bool Result(string input, bool sensitive);
    }

    public interface IQueryPluginRString : IQueryPlugin
    {
        string Result(string input, bool sensitive);
    }
}