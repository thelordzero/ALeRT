namespace ALeRT.PluginFramework
{
    public interface ITypePlugin
    {
        string PluginCategory { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        bool Result { get; }
    }
}