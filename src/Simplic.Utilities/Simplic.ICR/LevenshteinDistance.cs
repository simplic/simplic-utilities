using System;

namespace Simplic.ICR
{
    /// <summary>
    /// Provides algorithm for comparing strings
    /// </summary>
    public class LevenshteinDistance
    {
        /// <summary>
        /// Get the distance between two string via Levenshtein. The algorithm does not care about case sensitive
        /// </summary>
        /// <param name="source">Original text</param>
        /// <param name="target">Text to compare with</param>
        /// <returns>Difference in a percentage value. 0% == strings are equal, 100% = they are completely different</returns>
        public static double ComputeDistancePercentage(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 100.0;

            source = source.ToLower();
            target = target.ToLower();

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length))) * 100;
        }

        /// <summary>
        /// Compute the distance between two texts
        /// </summary>
        /// <param name="original"></param>
        /// <param name="modified"></param>
        /// <returns></returns>
        public static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
    }
}