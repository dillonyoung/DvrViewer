using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DvrViewer.Shared
{
    /// <summary>
    /// Based off of the article http://blog.functionalfun.net/2008/06/wpf-passwordbox-and-data-binding.html
    /// </summary>
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatePassword = DependencyProperty.RegisterAttached("UpdatePassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = dependencyObject as PasswordBox;

            if (dependencyObject == null || !GetBindPassword(dependencyObject))
            {
                return;
            }

            passwordBox.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(passwordBox))
            {
                passwordBox.Password = newPassword;
            }

            passwordBox.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = dependencyObject as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }

            bool wasBound = (bool)e.OldValue;
            bool needToBind = (bool)e.NewValue;

            if (wasBound)
            {
                passwordBox.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                passwordBox.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            SetUpdatingPassword(passwordBox, true);

            SetBoundPassword(passwordBox, passwordBox.Password);
            SetUpdatingPassword(passwordBox, false);
        }

        public static bool GetBindPassword(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(BindPassword);
        }

        public static void SetBindPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(BindPassword, value);
        }

        public static bool GetBoundPassword(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(BoundPassword, value);
        }

        public static bool GetUpdatingPassword(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(UpdatePassword);
        }

        public static void SetUpdatingPassword(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(UpdatePassword, value);
        }
    }
}
