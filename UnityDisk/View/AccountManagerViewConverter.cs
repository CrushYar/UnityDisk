﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using WinRTMultibinding.Foundation.Interfaces;

namespace UnityDisk.View
{
    public class GroupNameListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IList<string> groups = value as IList<string>;
            if (groups == null) return "";

            string result = "";

            int i = 0;
            foreach (var group in groups)
            {
                result += group;

                i++;
                if (i < group.Length)
                    result += ", ";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class IconServerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string serverName = value as string;
            if (serverName == null) return null;

            switch (serverName)
            {
                case "+":
                    return "ms-appx:///Assets/manager_accounts/add200x200.png";
                case "OneDrive":
                    return "ms-appx:///Assets/manager_accounts/oneDrive200x200.png";
                default:
                    throw new ArgumentException("Unknow the type");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
