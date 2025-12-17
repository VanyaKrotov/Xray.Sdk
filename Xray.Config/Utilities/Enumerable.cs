namespace Xray.Config.Utilities;

public static class EnumerableUtilities
{
    public static Dictionary<K, V> MergeDict<K, V>(Dictionary<K, V> a, Dictionary<K, V> b) where K : notnull
    {
        return a.Concat(b).GroupBy(x => x.Key).ToDictionary(x => x.Key, g => g.Last().Value);
    }
}