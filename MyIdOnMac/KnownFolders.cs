// MIT License - Copyright (c) 2019 Black Data

using System;
using System.IO;

namespace MyIdOnMac
{
    /// <summary>
    /// Class containing methods to retrieve specific file system paths.
    /// </summary>
    public static class KnownFolders
    {

        /// <summary>
        /// The directory stores data file without the file name
        /// </summary>
        public static string DataDir
        {
            get
            {
                string savedDataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // AppDomain.CurrentDomain.BaseDirectory;
                if (Xamarin.Forms.Application.Current != null && Xamarin.Forms.Application.Current.Properties.ContainsKey("DataFile"))
                {
                    savedDataDir = Xamarin.Forms.Application.Current.Properties["DataFile"] as string;
                    return Path.GetDirectoryName(savedDataDir);
                }
                else
                    return savedDataDir;
            }


        }

        /// <summary>
        /// The directory stores data file with the file name
        /// </summary>
        public static string DataFile
        {
            get
            {
                return Xamarin.Essentials.Preferences.Get("DataFile", "");
            }
            set
            {

                Xamarin.Essentials.Preferences.Set("DataFile", value);

            }
        }

    }
}