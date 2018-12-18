namespace FxManager.Cache
{
    public interface ICacheProvider
    {
        bool Contains(string key);
        object TryGet(string key);
    }

    public class CacheProvider : ICacheProvider
    {
        public bool Contains(string key)
        {
            return false;
        }

        public object TryGet(string key)
        {
            return null;
        }
    }
}