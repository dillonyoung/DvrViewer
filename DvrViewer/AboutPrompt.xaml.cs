using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DvrViewer
{
    /// <summary>
    /// Interaction logic for AboutPrompt.xaml
    /// </summary>
    public partial class AboutPrompt : Window
    {
        public Uri SupportUri { get; set; }
        
        public AboutPrompt()
        {
            InitializeComponent();
        }

        private void AboutPrompt_OnLoaded(object sender, RoutedEventArgs e)
        {
            string location = Assembly.GetExecutingAssembly().Location;
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(location);

            Title = $"About {fileVersionInfo.FileDescription}";

            LabelApplicationTitle.Content = fileVersionInfo.FileDescription;
            LabelApplicationName.Content = fileVersionInfo.FileDescription;
            LabelApplicationCopyright.Content = fileVersionInfo.LegalCopyright;
            LabelApplicationVersion.Content = $"{fileVersionInfo.ProductVersion}";
            LabelCompany.Content = fileVersionInfo.CompanyName;
            LabelComments.Content = fileVersionInfo.Comments;
            LabelApplicationAuthor.Content = "Dillon Young";

            if (SupportUri == null)
            {
                StackPanelSupport.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextBlockSupport.Text = SupportUri.AbsoluteUri;
            }

            if (string.IsNullOrEmpty(fileVersionInfo.CompanyName))
            {
                StackPanelCompany.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(fileVersionInfo.Comments))
            {
                StackPanelComments.Visibility = Visibility.Collapsed;
            }
        }
        
        private void ButtonLinkSupport_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(SupportUri.AbsoluteUri);
        }
    }
}
