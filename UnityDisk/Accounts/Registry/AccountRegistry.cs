using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace UnityDisk.Accounts.Registry
{
    public class AccountRegistry : IAccountRegistry
    {
        public string UserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public BitmapImage UserImage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SpaceSize Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Count { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<RegistrySizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<RegistryChangedEventArg> ChangedRegistryEvent;
        public event EventHandler<RegistryLoadedEventArg> LoadedEvent;

        public IAccount Find(string login)
        {
            throw new NotImplementedException();
        }

        public void Registry(IAccount account)
        {
            throw new NotImplementedException();
        }
    }
}
