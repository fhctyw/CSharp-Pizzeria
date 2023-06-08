namespace Pizza
{
    public interface ISerializer
    {
        public T Deserialize<T>(string path);
        public void Serialize<T>(string path, T obj);
    }
}
