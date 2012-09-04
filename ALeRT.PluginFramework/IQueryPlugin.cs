namespace ALeRT.PluginFramework
{
    public interface IQueryPlugin
    {
        string PluginCategory { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        System.Collections.Generic.List<string> TypesAccepted { get; }
    }

    interface IQueryPluginRBool : IQueryPlugin
    {
        bool Result(string input, bool sensitive);
    }

    interface IQueryPluginRString : IQueryPlugin
    {
        string Result(string input, bool sensitive);
    }
}