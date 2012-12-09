namespace ALeRT.PluginFramework
{
    public interface IQueryPlugin
    {
        string PluginCategory { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        System.Collections.Generic.List<string> TypesAccepted { get; }
        string Result(string input, string type, bool sensitive);
    }
}