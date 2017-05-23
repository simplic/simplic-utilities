using System.Security.Principal;

namespace Simplic.Security.Principal
{
    /// <summary>
    /// List of principal helper
    /// </summary>
    public class WindowsPrincipalHelper
    {
        /// <summary>
        /// Proof wether the current user has administration priviliges
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}