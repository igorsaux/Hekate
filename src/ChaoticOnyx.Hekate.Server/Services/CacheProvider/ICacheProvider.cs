using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.CacheProvider
{
    public interface ICacheProvider<TKey, TValue>
    {
        public Task<TValue> Resolve(TKey key);

        public Task Update(TKey key, TValue value);
    }
}
