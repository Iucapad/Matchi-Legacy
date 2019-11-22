using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
    public sealed partial class BigPicturePage : Page
    {
        public BigPicturePage()
        {
            this.InitializeComponent();
        }

        private void WindowSize(object sender, SizeChangedEventArgs e)
        {
            var bounds = Window.Current.Bounds;
            double height = bounds.Height;
            double width = bounds.Width;
            if (((Frame)).ActualWidth < 750)
            {
                grid_team1.Height = (height / 2) - 45;
                grid_team2.Height = (height / 2) - 45;
                grid_team2.Margin = new Thickness(30);
                grid_team1.Width = width-60;
                grid_team2.Width = width-60;
            }
            else
            {
                grid_team1.Height = height-260;
                grid_team2.Height = height-260;
                grid_team2.Margin = new Thickness(15,0,30,230);
                grid_team1.Width = (width / 2) - 45;
                grid_team2.Width = (width / 2) - 45;
            }
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            String app_setting = localSettings.Values["theme_setting"] as string;
            ApplyTheme(app_setting);
        }
        public void ApplyTheme(string app_setting)
        {
            //Définit les couleurs à remplir dans les cartes des équipes selon le paramètre utilisateur
            Color color1a = Color.FromArgb(0, 0, 0, 0);
            Color color1b = Color.FromArgb(0, 0, 0, 0);
            Color color2a = Color.FromArgb(0, 0, 0, 0);
            Color color2b = Color.FromArgb(0, 0, 0, 0);
            switch (app_setting)
            {
                case "1":
                    color1a = Color.FromArgb(255, 221, 101, 40);
                    color1b = Color.FromArgb(255, 244, 107, 126);
                    color2a = Color.FromArgb(255, 42, 101, 212);
                    color2b = Color.FromArgb(255, 130, 80, 192);
                    break;
                case "2":
                    color1a = Color.FromArgb(255, 255, 204, 51);
                    color1b = Color.FromArgb(255, 245, 251, 27);
                    color2a = Color.FromArgb(255, 75, 175, 253);
                    color2b = Color.FromArgb(255, 6, 239, 255);
                    break;
                case "3":
                    color1a = Color.FromArgb(255, 3, 77, 141);
                    color1b = Color.FromArgb(255, 50, 100, 160);
                    color2a = Color.FromArgb(255, 175, 37, 37);
                    color2b = Color.FromArgb(255, 225, 37, 37);
                    break;
                default:
                    color1a = Color.FromArgb(255, 221, 101, 40);
                    color1b = Color.FromArgb(255, 244, 107, 126);
                    color2a = Color.FromArgb(255, 42, 101, 212);
                    color2b = Color.FromArgb(255, 130, 80, 192);
                    break;
            }
            LinearGradientBrush leftcardBrush = new LinearGradientBrush();
            leftcardBrush.StartPoint = new Point(0.1, 0);
            leftcardBrush.EndPoint = new Point(0.25, 1);
            leftcardBrush.GradientStops.Add(new GradientStop { Color = color1a, Offset = 0.2 });
            leftcardBrush.GradientStops.Add(new GradientStop { Color = color1b, Offset = 0.9 });
            card_team1.Fill = leftcardBrush;

            LinearGradientBrush rightcardBrush = new LinearGradientBrush();
            rightcardBrush.StartPoint = new Point(0.1, 0);
            rightcardBrush.EndPoint = new Point(0.25, 1);
            rightcardBrush.GradientStops.Add(new GradientStop { Color = color2a, Offset = 0.2 });
            rightcardBrush.GradientStops.Add(new GradientStop { Color = color2b, Offset = 0.9 });
            card_team2.Fill = rightcardBrush;
        }
    }
}
