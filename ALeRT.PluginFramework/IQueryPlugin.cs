namespace ALeRT.PluginFramework
{
    interface IQueryPlugin
    {
        string PluginCategory { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        string Result { get; }
    }
}