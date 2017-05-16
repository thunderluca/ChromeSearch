using System.Linq;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToDictionary(k => k.Key, k => k.Value);
        }
    }
}
