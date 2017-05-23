using System;

namespace Simplic.Cache
{
    /// <summary>
    /// WeakReference that keeps it type
    /// </summary>
    public class TypedWeakReference : WeakReference
    {
        //
        // Summary:
        //     Initializes a new instance of the System.WeakReference class, referencing the
        //     specified object.
        //
        // Parameters:
        //   target:
        //     The object to track or null.
        public TypedWeakReference(object target) : base(target)
        {
            this.TargetType = target?.GetType();
        }

        //
        // Summary:
        //     Initializes a new instance of the System.WeakReference class, referencing the
        //     specified object and using the specified resurrection tracking.
        //
        // Parameters:
        //   target:
        //     An object to track.
        //
        //   trackResurrection:
        //     Indicates when to stop tracking the object. If true, the object is tracked after
        //     finalization; if false, the object is only tracked until finalization.
        public TypedWeakReference(object target, bool trackResurrection) : base(target, trackResurrection)
        {
            this.TargetType = target?.GetType();
        }

        /// <summary>
        /// Gets the type of the target, even if it not alive
        /// </summary>
        public Type TargetType
        {
            get;
            private set;
        }
    }
}