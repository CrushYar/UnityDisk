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
using StructureMap;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace UnityDisk
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public interface IClass { }

        public interface IClass1 : IClass { }

        public interface IClass2 : IClass { }

        public class Class1 : IClass1 { }

        public class Class2 : IClass2 { }
        public class RegisterByContainer
        {
            public IContainer Container;

            public RegisterByContainer()
            {
                Container = new Container(x => {
                    x.For<IClass1>().Use<Class1>();
                    x.For<IClass2>().Use<Class2>();
                });
            }
        }
        public MainPage()
        {
            var container = new RegisterByContainer().Container;
            var class1Inst = container.GetInstance<IClass1>();
            var class2Inst = container.GetInstance<IClass2>();

            this.InitializeComponent();
        }
    }
}
