
namespace MongoContainerAPI
{
    public class MemoryDB : IDB
    {
        private string _savedValue = "test";
        public string GetValue()
        {
            return _savedValue;
        }

        public void SaveValue(string value)
        {
            _savedValue = value;
        }

        IEnumerable<string> IDB.GetValue()
        {
            throw new NotImplementedException();
        }
    }
}
