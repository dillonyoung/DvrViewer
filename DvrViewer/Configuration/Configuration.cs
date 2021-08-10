using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DvrViewer.Attributes;

namespace DvrViewer.Configuration
{
    public static class Configuration
    {
        public static T LoadConfiguration<T>() where T : new()
        {
            T result = new T();

            string filename = GetConfigurationFileName<T>();

            if (!File.Exists(filename))
            {
                return result;
            }

            try
            {
                string data = File.ReadAllText(filename);

                FileFormat fileFormat = JsonConvert.DeserializeObject<FileFormat>(data);

                if (fileFormat.Id != "DVRVIEW")
                {
                    return result;
                }

                var obj = JObject.Parse(fileFormat.Configuration.ToString());

                result = obj.ToObject<T>();

                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (!Attribute.IsDefined(propertyInfo, typeof(EncryptedConfigurationAttribute)))
                    {
                        continue;
                    }

                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        string value = propertyInfo.GetValue(result).ToString();

                        try
                        {
                            byte[] bytes = Convert.FromBase64String(value);

                            value = Encoding.Unicode.GetString(ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser));
                            propertyInfo.SetValue(result, value);
                        }
                        catch
                        {
                            propertyInfo.SetValue(result, string.Empty);
                        }
                    }
                }
            }
            catch
            {
                return result;
            }

            return result;
        }

        public static void SaveConfiguration<T>(T configuration)
        {
            T temp = configuration;

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!Attribute.IsDefined(propertyInfo, typeof(EncryptedConfigurationAttribute)))
                {
                    continue;
                }

                if (propertyInfo.PropertyType == typeof(string))
                {
                    string value = propertyInfo.GetValue(temp).ToString();

                    byte[] bytes = ProtectedData.Protect(Encoding.Unicode.GetBytes(value), null, DataProtectionScope.CurrentUser);
                    value = Convert.ToBase64String(bytes);
                    propertyInfo.SetValue(temp, value);
                }
            }

            FileFormat fileFormat = new FileFormat();

            DateTime? dateCreated = GetDateCreated<T>();

            fileFormat.Created = dateCreated ?? DateTime.Now;
            fileFormat.LastModified = DateTime.Now;
            fileFormat.SaveCount = GetSaveCount<T>() + 1;

            fileFormat.Configuration = temp;

            string data = JsonConvert.SerializeObject(fileFormat);

            File.WriteAllText(GetConfigurationFileName<T>(), data);
            Console.WriteLine(GetConfigurationFileName<T>());
        }

        private static DateTime? GetDateCreated<T>()
        {
            string filename = GetConfigurationFileName<T>();

            if (!File.Exists(filename))
            {
                return null;
            }

            try
            {
                string data = File.ReadAllText(filename);

                FileFormat fileFormat = JsonConvert.DeserializeObject<FileFormat>(data);

                if (fileFormat.Id != "DVRVIEW")
                {
                    return null;
                }

                return fileFormat.Created;
            }
            catch
            {
                return null;
            }
        }

        private static int GetSaveCount<T>()
        {
            string filename = GetConfigurationFileName<T>();

            if (!File.Exists(filename))
            {
                return 0;
            }

            try
            {
                string data = File.ReadAllText(filename);

                FileFormat fileFormat = JsonConvert.DeserializeObject<FileFormat>(data);

                if (fileFormat.Id != "DVRVIEW")
                {
                    return 0;
                }

                return fileFormat.SaveCount;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the file name for the configuration file
        /// </summary>
        /// <typeparam name="T">The type of configuration file</typeparam>
        /// <returns>Returns the full file name path for the configuration file</returns>
        private static string GetConfigurationFileName<T>()
        {
            Type type = typeof(T);

            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dillon Young", "DVR Viewer");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return Path.Combine(folder, $"{type.Name}.config");
        }
    }
}
