namespace MongoContainerAPI
{
    public interface IDB
    {
        void SaveValue(string value);
        IEnumerable<string> GetValue();
    }
}
