using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UnityDisk.Accounts;
using UnityDisk.FileStorages;
using UnityDisk.Settings.Accounts;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace UnityDisk
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            Loaded += MainPage_Loaded;
            this.InitializeComponent();
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn("shazhko.artem@gmail.com");
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "David Guetta ft Justin Bieber - 2U (SING OFF vs. Olly Murs).mp4",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };
            FileSavePicker openPicker = new FileSavePicker();
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            string type = file.Name.Substring(file.Name.LastIndexOf('.'));
            openPicker.FileTypeChoices.Add(file.Name,new []{ type });
            var localFile = await openPicker.PickSaveFileAsync();
            var uploader = file.Download(localFile);
            var list= await BackgroundDownloader.GetCurrentDownloadsAsync();
            
            uploader.Initialization(list.ToArray());
            await uploader.Start();
        }
    }
}
