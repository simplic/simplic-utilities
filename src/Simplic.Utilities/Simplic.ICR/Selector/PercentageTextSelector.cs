using System;

namespace Simplic.ICR.Selector
{
    /// <summary>
    /// Compare string with a percentage value. The configuration must be look like this: string%int
    /// </summary>
    public class PercentageTextSelector : ISelector
    {
        #region Fields

        private double percentage;
        private string text;

        #endregion Fields

        /// <summary>
        /// Create percentage selector
        /// </summary>
        /// <param name="configuration">Configure the selector: text%min-percentage</param>
        public void Initialize(string configuration)
        {
            if (!configuration.Contains("%"))
            {
                throw new Exception("PercentageTextSelector initialize error, missing % [Wrong syntax]");
            }

            string[] splitted = configuration.Split(new char[] { '%' });

            if (splitted.Length == 2)
            {
                text = splitted[0];

                if (string.IsNullOrWhiteSpace(splitted[0]))
                {
                    throw new Exception("PercentageTextSelector initialize error, text to match not correct. [Wrong syntax]");
                }

                if (double.TryParse(splitted[1], out percentage) == false)
                {
                    throw new Exception("PercentageTextSelector initialize error, percentage value is no double. [Wrong syntax]");
                }

                if (percentage < 0 || percentage > 100)
                {
                    throw new Exception("PercentageTextSelector initialize error. Percentage value must be between 0 and 100.");
                }
            }
            else
            {
                throw new Exception("PercentageTextSelector initialize error. [Wrong syntax]");
            }
        }

        /// <summary>
        /// Execute selector rules
        /// </summary>
        /// <param name="input">Text to proof</param>
        /// <returns>Return true if match is ok</returns>
        public bool Compare(string input, out string _out)
        {
            double value = LevenshteinDistance.ComputeDistancePercentage(input, text);
            var res = value >= percentage;

            _out = null;

            if (res)
            {
                _out = text;
            }

            return res;
        }
    }
}