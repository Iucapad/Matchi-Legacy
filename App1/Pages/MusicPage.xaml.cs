using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.InteropServices;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatchiApp
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MusicPage : Page
    {
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public MusicPage()
        {
            this.InitializeComponent();

            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                compactOverlayButton.Visibility = Visibility.Visible;
            }
        }
        private async void Launch_Spotify(object sender, RoutedEventArgs e)
        {
            var ret = await Windows.System.Launcher.QueryUriSupportAsync(new Uri("spotify:"), Windows.System.LaunchQuerySupportType.Uri);
            if (ret == LaunchQuerySupportStatus.Available)
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("spotify:"));
            }
            else
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(@"ms-windows-store://pdp/?ProductId=9ncbcszsjrsb"));
            }
        }

        private void c1(object sender, RoutedEventArgs e)
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void c2(object sender, RoutedEventArgs e)
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void c3(object sender, RoutedEventArgs e)
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void show(object sender, RoutedEventArgs e)
        {
            keybd_event(0xAD, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }
        private async void compact(object sender, RoutedEventArgs e)
        {           
            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = new Windows.Foundation.Size(320, 220);
            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
            page_title.Visibility = Visibility.Collapsed;
            compactOverlayButton.Visibility = Visibility.Collapsed;
            normalbtn.Visibility = Visibility.Visible;
            mutebtn.Margin = new Thickness(90, 140, 0, 0);
        }
        private async void normal(object sender, RoutedEventArgs e)
        {            
            bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            page_title.Visibility = Visibility.Visible;
            compactOverlayButton.Visibility = Visibility.Visible;
            normalbtn.Visibility = Visibility.Collapsed;
            mutebtn.Margin = new Thickness(0, 140, 0, 0);
        }

    private void Resize_page(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)Window.Current.Content).ActualHeight < 500)
            {
                controls_box.Margin = new Thickness(0, 0, 0, 50);
                launchbtn.Visibility = Visibility.Collapsed;
                infotext.Visibility = Visibility.Collapsed;
            }
            else
            {
                controls_box.Margin = new Thickness(0, 0, 0, 210);
                launchbtn.Visibility = Visibility.Visible;
                infotext.Visibility = Visibility.Visible;
            }
        }
    }
}

