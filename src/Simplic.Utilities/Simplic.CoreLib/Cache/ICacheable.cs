namespace Simplic.Cache
{
    /// <summary>
    /// Stellt ein Interface da, welche gecached werden kann und mit dem CacheManager verwaltet wird
    /// </summary>
    public interface ICacheable
    {
        #region Public Methods

        /// <summary>
        /// Wird aufgerufen, wenn bevor das Objekt aus dem Cache gelöscht wird
        /// </summary>
        void OnRemove();

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// Eindeutiger Key des Cache-Objeczs
        /// </summary>
        string Key
        {
            get;
        }

        #endregion Public Member
    }
}