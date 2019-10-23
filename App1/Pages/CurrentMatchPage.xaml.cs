using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CurrentMatchPage : Page
    {
        public CurrentMatchPage()
        {
            this.InitializeComponent();
        }

        private void show(object sender, RoutedEventArgs e)
        {
            if (!page.Children.Contains(new_round))
            {
                page.Children.Add(new_round);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            page.Children.Remove(new_round);
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)).ActualWidth < 750)
            {
                ui_container.Orientation = Orientation.Vertical;
                ui_container.Margin = new Thickness(0, 60, 0, 60);
                ui_container.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_container.CornerRadius = new CornerRadius(0);
                ui_leftcard.Height = 125;
                ui_rightcard.Height = 125;
            }
            else
            {
                ui_container.Orientation = Orientation.Horizontal;
                ui_container.Margin = new Thickness(20, 100, 20, 60);
                ui_container.HorizontalAlignment = HorizontalAlignment.Center;
                ui_container.CornerRadius = new CornerRadius(10);
                ui_leftcard.Height = 250;
                ui_rightcard.Height = 250;
            }
        }
    }
}
