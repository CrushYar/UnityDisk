using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Microsoft.Identity.Client;

namespace UnityDisk
{
    public sealed class RemoteInitialization
    {
        public static async Task Start()
        {
            var test = UnityDisk.App.PublicClientApp.Users;
            try
            {
                    await UnityDisk.App.PublicClientApp.AcquireTokenSilentAsync(UnityDisk.App.Scopes,
                        UnityDisk.App.PublicClientApp.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");
                try
                {
                     await UnityDisk.App.PublicClientApp.AcquireTokenAsync(UnityDisk.App.Scopes);
                }
                catch (MsalException msalex)
                {
                    Debug.WriteLine($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
            }
        }
    }
}
