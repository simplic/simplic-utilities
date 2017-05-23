namespace Simplic.SAAP
{
    /// <summary>
    /// Simple empty collection controller
    /// </summary>
    /// <typeparam name="T">IProvider type</typeparam>
    internal class EmptyProviderCollectionController<T> : ProviderCollectionController<T> where T : IProvider
    {
        /// <summary>
        /// Crate empty provider host
        /// </summary>
        public EmptyProviderCollectionController()
        {
        }

        /// <summary>
        /// Name of the provider host
        /// </summary>
        public override string ProviderCollectionName
        {
            get { return (typeof(T).Name + "ProviderHost"); }
        }
    }
}