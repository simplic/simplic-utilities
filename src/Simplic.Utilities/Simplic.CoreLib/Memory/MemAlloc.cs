using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;

namespace Simplic.Memory
{
    /// <summary>
    /// Provides functions to embedd a memory usage limit in .net applications. Amount of available memory is calculated using this indicators:
    /// 1. Application architecture (x86, x64). TryAlloc and Alloc will be ignored on x64 by default
    /// 2. Current application memory usage
    /// 3. Free system memory, minus a minimum of free memory which must be left for the system to run and catch memory peaks
    /// </summary>
    public static class MemAlloc
    {
        #region Private Static Member

        private static ulong maxMemoryAllocation;
        private static ulong minFreeSystemMemory;
        private static bool isInUse;
        private static bool x86Only = true;
        private static Process mainProcess;
        private static ComputerInfo computerInfo;

        #endregion Private Static Member

        #region [Constructor]

        /// <summary>
        /// Set default values
        /// </summary>
        static MemAlloc()
        {
            // 1.5GB max memory "can be" allocated when using TryAlloc() or Alloc()
            // maxMemoryAllocation = 1610612736;
            maxMemoryAllocation = 314572800;

            // Amount of minimum memory, which must be free on the system
            minFreeSystemMemory = 209715200;

            x86Only = true;
            isInUse = true;
            mainProcess = Process.GetCurrentProcess();
            computerInfo = new ComputerInfo();
        }

        #endregion [Constructor]

        #region Public Static Memer

        /// <summary>
        /// Collect and wait for finalizing data
        /// </summary>
        public static void FinalizeCollectedData()
        {
            // Push none rooted elemets to finalizer queue
            GC.Collect();

            // Remove objects
            GC.WaitForPendingFinalizers();

            // Just do it again
            GC.Collect();

            // Remove objects
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Add memory which will be used in an unmanaged library
        /// </summary>
        /// <param name="bytes">Amount of unmanaged memory</param>
        public static void AddUnmanagedUsedMemory(long bytes)
        {
            GC.AddMemoryPressure(bytes);
        }

        /// <summary>
        /// Remove memory which will be used in an unmanaged library
        /// </summary>
        /// <param name="bytes">Amount of unmanaged memory</param>
        public static void RemoveUnmanagedUsedMemory(long bytes)
        {
            GC.RemoveMemoryPressure(bytes);
        }

        /// <summary>
        /// Return true if no more memory is allocated than the maximum allowed
        /// </summary>
        /// <returns>Return true if no more memory is allocated than the maximum allowed</returns>
        public static bool TryAlloc()
        {
            if ((x86Only && IntPtr.Size == 8) || !isInUse)
            {
                return true;
            }

            ulong gcTotalMem = (ulong)GC.GetTotalMemory(false);
            ulong available = computerInfo.AvailablePhysicalMemory - minFreeSystemMemory;

            if (gcTotalMem > available)
            {
                return false;
            }

            return maxMemoryAllocation > gcTotalMem;
        }

        /// <summary>
        /// Throws an exception, if more memory is allocated than the maximum allowed
        /// </summary>
        public static void Alloc()
        {
            if ((x86Only && IntPtr.Size == 8) || !isInUse)
            {
                return;
            }

            if (!TryAlloc())
            {
                throw new Exception(String.Format("No more memory can be allocated. Maximum `{0}`, current working-set: `{1}`", maxMemoryAllocation, mainProcess.WorkingSet64));
            }
        }

        /// <summary>
        /// Return true if no more memory is allocated than the maximum allowed
        /// </summary>
        /// <param name="bytes">Bytes which should be allocated</param>
        /// <returns>Return true if no more memory is allocated than the maximum allowed</returns>
        public static bool TryAlloc(ulong bytes)
        {
            if ((x86Only && IntPtr.Size == 8) || !isInUse)
            {
                return true;
            }

            ulong gcTotalMem = (ulong)GC.GetTotalMemory(false);
            ulong available = computerInfo.AvailablePhysicalMemory - minFreeSystemMemory;

            if (gcTotalMem + bytes > available)
            {
                return false;
            }

            return maxMemoryAllocation > gcTotalMem + bytes;
        }

        /// <summary>
        /// Throws an exception, if more memory is allocated than the maximum allowed
        /// </summary>
        /// <param name="bytes">Bytes which should be allocated</param>
        public static void Alloc(ulong bytes)
        {
            if ((x86Only && IntPtr.Size == 8) || !isInUse)
            {
                return;
            }

            if (!TryAlloc())
            {
                throw new Exception(String.Format("No more memory can be allocated. Maximum `{0}`, current working-set: `{1}`, trying to allocate: {2}", maxMemoryAllocation, mainProcess.WorkingSet64, bytes));
            }
        }

        #endregion Public Static Memer

        #region Public Static Member

        /// <summary>
        /// If set to true (default) the monitoring will only work for x86 processes
        /// </summary>
        public static bool X86Only
        {
            get
            {
                return x86Only;
            }

            set
            {
                x86Only = value;
            }
        }

        /// <summary>
        /// Maximum memory the application is allowed to allocate for private memory usage
        /// </summary>
        public static ulong MaxMemoryAllocation
        {
            get
            {
                return maxMemoryAllocation;
            }

            set
            {
                maxMemoryAllocation = value;
            }
        }

        /// <summary>
        /// Main process of the current application
        /// </summary>
        public static Process MainProcess
        {
            get
            {
                return mainProcess;
            }

            set
            {
                mainProcess = value;
            }
        }

        /// <summary>
        /// If set to false, memory allocation will always work
        /// </summary>
        public static bool IsInUse
        {
            get
            {
                return isInUse;
            }

            set
            {
                isInUse = value;
            }
        }

        /// <summary>
        /// Minimum of system memory which should be free
        /// </summary>
        public static ulong MinFreeSystemMemory
        {
            get
            {
                return minFreeSystemMemory;
            }

            set
            {
                minFreeSystemMemory = value;
            }
        }

        #endregion Public Static Member
    }
}