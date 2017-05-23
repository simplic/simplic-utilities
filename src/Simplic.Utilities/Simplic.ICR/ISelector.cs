namespace Simplic.ICR
{
    /// <summary>
    /// Base-Selector which must be implemented in all selector classes
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// Initialize the current selector
        /// </summary>
        /// <param name="configuration">Configuration string</param>
        void Initialize(string configuration);

        /// <summary>
        /// Proof, whether the selector match on the input string
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="match">Output match</param>
        /// <returns>True if the selector match</returns>
        bool Compare(string input, out string match);
    }
}