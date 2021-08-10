using System;

namespace DvrViewer.Attributes
{
    /// <summary>
    /// Identifies whether a property has to be stored as an encrypted value in the configuration file
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedConfigurationAttribute : Attribute
    {
    }
}
