using System;

namespace Simplic
{
    /// <summary>
    /// Bedingung zum Vergleichen von zwei Versionen
    /// </summary>
    public enum VersionCondition
    {
        /// <summary>
        /// Compare ==
        /// </summary>
        Equal,

        /// <summary>
        /// Compare <
        /// </summary>
        Smaller,

        /// <summary>
        /// Compare >
        /// </summary>
        Greater,

        /// <summary>
        /// Compare <=
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// Compare for >=
        /// </summary>
        SmallerEqual
    }

    /// <summary>
    /// Stellt Methods bereit, um mit Versionierung zu arbeiten
    /// </summary>
    public class VersionHelper
    {
        /// <summary>
        /// Vegleicht zwei Versionen
        /// </summary>
        /// <param name="Then">Version als System.Version (Bspl: 1.0.0.0)</param>
        /// <param name="Other">Version als String (Bspl: 1.0.0.0)</param>
        /// <param name="Condition">Vergleichsoperator</param>
        /// <returns>True, wenn die Bedingung erfüllt wird</returns>
        public static bool CompareVersion(Version Then, string Other, VersionCondition Condition)
        {
            return CompareVersion(Then.ToString(), Other, Condition);
        }

        /// <summary>
        /// Vegleicht zwei Versionen
        /// </summary>
        /// <param name="Then">Version als String (Bspl: 1.0.0.0)</param>
        /// <param name="Other">Version als String (Bspl: 1.0.0.0)</param>
        /// <param name="Condition">Vergleichsoperator</param>
        /// <returns>True, wenn die Bedingung erfüllt wird</returns>
        public static bool CompareVersion(string Then, string Other, VersionCondition Condition)
        {
            bool returnValue = false;
            Version v1 = null;
            Version v2 = null;

            try
            {
                v1 = Version.Parse(Then);
                v2 = Version.Parse(Other);
            }
            catch
            {
                throw new Exception(String.Format("Die Versionen konnten nicht in System.Version gecastet werden: {0} / {1}", Then, Other));
            }

            switch (Condition)
            {
                case VersionCondition.Equal:
                    returnValue = v1 == v2;
                    break;

                case VersionCondition.Greater:
                    returnValue = v1 > v2;
                    break;

                case VersionCondition.Smaller:
                    returnValue = v1 < v2;
                    break;

                case VersionCondition.GreaterEqual:
                    returnValue = v1 >= v2;
                    break;

                case VersionCondition.SmallerEqual:
                    returnValue = v1 <= v2;
                    break;
            }

            return returnValue;
        }
    }
}