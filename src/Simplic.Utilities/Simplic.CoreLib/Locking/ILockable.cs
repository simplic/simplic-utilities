using System;

namespace Simplic.Locking
{
    /// <summary>
    /// ILockable interface used for Simplic Locking
    /// </summary>
    public interface ILockable
    {
        Guid GetPersistantKey();
    }
}