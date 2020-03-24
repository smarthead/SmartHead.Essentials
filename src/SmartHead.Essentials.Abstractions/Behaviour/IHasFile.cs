namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public interface IHasFile<out T>
    {
        string FilePath { get; }
        string IntegrityHash { get; }
        T SetFilePath(string path, string hash);
    }
}