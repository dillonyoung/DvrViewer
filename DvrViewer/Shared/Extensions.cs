using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DvrViewer.Shared
{
    public static class Extensions
    {
        public static void CopyProperties(this object source, object destination)
        {
            if (source == null)
            {
                throw new ArgumentException("The source object is null");
            }

            if (destination == null)
            {
                throw new ArgumentException("The destination object is null");
            }

            Type typeSource = source.GetType();
            Type typeDestination = destination.GetType();

            PropertyInfo[] propertyInfosSource = typeSource.GetProperties();

            foreach (PropertyInfo propertyInfoSource in propertyInfosSource)
            {
                if (!propertyInfoSource.CanRead)
                {
                    continue;
                }

                PropertyInfo propertyInfoDestination = typeDestination.GetProperty(propertyInfoSource.Name);

                if (propertyInfoDestination == null)
                {
                    continue;
                }

                if (!propertyInfoDestination.CanWrite)
                {
                    continue;
                }

                if (propertyInfoDestination.GetSetMethod(true) != null && propertyInfoDestination.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }

                if ((propertyInfoDestination.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }

                if (!propertyInfoDestination.PropertyType.IsAssignableFrom(propertyInfoSource.PropertyType))
                {
                    continue;
                }

                propertyInfoDestination.SetValue(destination, propertyInfoSource.GetValue(source, null), null);
            }
        }

        public static void SaveBmpImage(this BitmapImage bitmapImage, string filename)
        {
            BitmapEncoder bitmapEncoder = new BmpBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                bitmapEncoder.Save(fileStream);
            }
        }
    }
}
