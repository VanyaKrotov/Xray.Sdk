namespace Xray.Config.Utilities;

public static class RandomUtilities
{
    public static int[] Seq(this Random rnd, int n)
    {
        var arr = Enumerable.Range(0, n).ToArray();
        for (int i = n - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        
        return arr;
    }
}