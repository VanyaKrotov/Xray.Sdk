using System.Security.Cryptography;
using System.Text;

namespace Xray.Config.Utilities;

public static class RandomUtilities
{
    private static Random rnd = new Random();

    private static readonly int[] numSeq = new int[10];
    private static readonly int[] lowerSeq = new int[26];
    private static readonly int[] upperSeq = new int[26];
    private static readonly int[] numLowerSeq = new int[36];
    private static readonly int[] numUpperSeq = new int[36];
    private static readonly int[] allSeq = new int[62];

    static RandomUtilities()
    {
        // 0–9
        for (int i = 0; i < 10; i++)
            numSeq[i] = '0' + i;

        // a–z, A–Z
        for (int i = 0; i < 26; i++)
        {
            lowerSeq[i] = 'a' + i;
            upperSeq[i] = 'A' + i;
        }

        // num + lower
        Array.Copy(numSeq, 0, numLowerSeq, 0, numSeq.Length);
        Array.Copy(lowerSeq, 0, numLowerSeq, numSeq.Length, lowerSeq.Length);

        // num + upper
        Array.Copy(numSeq, 0, numUpperSeq, 0, numSeq.Length);
        Array.Copy(upperSeq, 0, numUpperSeq, numSeq.Length, upperSeq.Length);

        // num + lower + upper
        Array.Copy(numSeq, 0, allSeq, 0, numSeq.Length);
        Array.Copy(lowerSeq, 0, allSeq, numSeq.Length, lowerSeq.Length);
        Array.Copy(upperSeq, 0, allSeq, numSeq.Length + lowerSeq.Length, upperSeq.Length);
    }

    public static string Seq(int n)
    {
        var sb = new StringBuilder(n);

        for (int i = 0; i < n; i++)
        {
            int idx = RandomNumberGenerator.GetInt32(allSeq.Length);

            sb.Append(new Rune(allSeq[idx]).ToString());
        }

        return sb.ToString();
    }


    public static int GetInRange(int from, int to)
    {
        return rnd.Next(from, to);
    }
}

