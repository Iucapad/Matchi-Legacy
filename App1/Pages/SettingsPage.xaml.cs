using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatchiApp
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content; //prend la mainpage actuelle comme mainpage
        public SettingsPage()
        {
            this.InitializeComponent();
            String app_setting = localSettings.Values["theme_setting"] as string;
            switch (app_setting) {
                case "1":
                    selection_theme.SelectedItem = SET1;
                    break;
                case "2":
                    selection_theme.SelectedItem = SET2;
                    break;
                case "3":
                    selection_theme.SelectedItem = SET3;
                    break;
                default:
                    selection_theme.SelectedItem = SET1;
                    break;
            }            
        }

        private async void SelectionTheme(object sender, SelectionChangedEventArgs e)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            switch (selection_theme.SelectedIndex)
            {
                case 0:
                    localSettings.Values["theme_setting"] = "1";
                    break;
                case 1:
                    localSettings.Values["theme_setting"] = "2";
                    break;
                case 2:
                    localSettings.Values["theme_setting"] = "3";
                    break;
                default:
                    localSettings.Values["theme_setting"] = "1";
                    break;
            }
        }

        private void ResizeWindow(object sender, SizeChangedEventArgs e)
        {
            if (((mainFrame)).ActualWidth < 750)
            {
                header_title.Margin = new Thickness(50, 0, 0, 0);
            }
            else
            {
                header_title.Margin = new Thickness(20, 10, 0, 0);
            }
        }
    }
}
