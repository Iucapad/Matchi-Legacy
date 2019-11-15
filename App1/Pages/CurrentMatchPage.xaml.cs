using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
    public sealed partial class CurrentMatchPage : Page
    {

        private Matchimpro matchimpro;
        private ObservableCollection<string> source_list_of_categories = new ObservableCollection<string>();
        private MatchStorage store = new MatchStorage();//objet relatif au stockage
        private DispatcherTimer timer = new DispatcherTimer();
        private ObservableCollection<string> source_time_choice = new ObservableCollection<string>();
        private int min;
        private int sec;
        int round_value = 1;
        int scoreleft = 0;
        int scoreright = 0;

        public CurrentMatchPage()
        {
            this.InitializeComponent();
            new_round.Visibility = Visibility.Visible;
            ui_infocard.Visibility = Visibility.Visible;
            SharedShadow.Receivers.Add(Receiver);            
            MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content;
            if (mainFrame.navigationView.MenuItems.Count < 4)
            {
                mainFrame.navigationView.MenuItems.Insert(0, new NavigationViewItem
                {
                    Name = "CURRENT",
                    IsSelected = true,
                    Content = "Match en cours",
                    Icon = new SymbolIcon((Symbol)0xE945),
                    Tag = "currentNav"                    
                });
            }
            for (int i = 1; i <= 10; i++)
                source_time_choice.Add(i+" min");
            timer_selection.ItemsSource = source_time_choice;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timerdown;
        }
        private async void ListInitialization()
        {
            StorageFolder storageFolder = store.Folder;
            IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync();

            if (!File.Exists(storageFolder.Path + Path.DirectorySeparatorChar + "Categories.catei"))
                return; //TODO : Erreur ?

            StorageFile cate_file = await storageFolder.GetFileAsync("Categories.catei");
            source_list_of_categories = new ObservableCollection<string>((await FileIO.ReadLinesAsync(cate_file)).Distinct());
            list_of_categories.ItemsSource = source_list_of_categories;
        }
        private void show()
        {
            if (!page.Children.Contains(ui_endround))
            {
                page.Children.Remove(ui_infocard);
                page.Children.Add(ui_endround);
                ui_trans1.Opacity = 0.4;
                ui_trans2.Opacity = 0.4;
            }
        }
        public static void HideNav(MainPage page)
        {
            page.navigationView.Visibility = Visibility.Collapsed;
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)).ActualWidth < 750)
            {
                ui_scroll.Margin = new Thickness(0, 60, 0, 100);
                ui_container.Orientation = Orientation.Vertical;
                ui_container.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_container.Padding = new Thickness(30,5,30,5);
                ui_vs.Height = 25;
                ui_vs.CornerRadius = new CornerRadius(15);
                ui_vs.Padding = new Thickness(2);
                ui_leftcard.Height = 125;
                ui_leftcard.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_leftcard.Width = double.NaN;
                ui_rightcard.Height = 125;
                ui_rightcard.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_rightcard.Width = double.NaN;
                ui_leftname.TextAlignment = TextAlignment.Left;
                ui_rightname.TextAlignment = TextAlignment.Left;
                ui_catname.HorizontalAlignment = HorizontalAlignment.Center;
                ui_catname.Margin = new Thickness(0, 10, 0, 60);
                ui_nbjouteurs.HorizontalAlignment = HorizontalAlignment.Center;
                ui_nbjouteurs.Margin = new Thickness(0, 60, 0, 20);
            }
            else
            {
                ui_scroll.Margin = new Thickness(0, 100, 0, 0);
                ui_container.Orientation = Orientation.Horizontal;
                ui_container.HorizontalAlignment = HorizontalAlignment.Center;
                ui_container.Padding = new Thickness(20);
                ui_vs.Height = 50;
                ui_vs.CornerRadius = new CornerRadius(25);
                ui_vs.Padding = new Thickness(15);
                ui_leftcard.Height = 250;
                ui_leftcard.HorizontalAlignment = HorizontalAlignment.Center;
                ui_leftcard.Width = 300;
                ui_rightcard.Height = 250;
                ui_rightcard.HorizontalAlignment = HorizontalAlignment.Center;
                ui_rightcard.Width = 300;
                ui_leftname.TextAlignment = TextAlignment.Center;
                ui_rightname.TextAlignment = TextAlignment.Center;
                ui_catname.HorizontalAlignment = HorizontalAlignment.Left;
                ui_catname.Margin = new Thickness(120, 30, 0, 60);
                ui_nbjouteurs.HorizontalAlignment = HorizontalAlignment.Right;
                ui_nbjouteurs.Margin = new Thickness(0, 30, 150, 120);
            }
        }

        private async void Start_click(object sender, RoutedEventArgs e)
        {
            if(list_of_categories.SelectedItem is null || timer_selection.SelectedItem is null)
            {
                ContentDialog someNullValuesDialog = new ContentDialog
                {
                    Title = "Attention",
                    Content = "Vous devez remplir les champs vides avant de lancer une manche.",
                    CloseButtonText = "Annuler"
                };
                await someNullValuesDialog.ShowAsync();
            }
            else
            {
                ui_catname.Text = list_of_categories.SelectedItem.ToString();
                page.Children.Remove(new_round);
                source_list_of_categories.Remove(list_of_categories.SelectedValue.ToString());
                sec = 0;
                min = timer_selection.SelectedIndex+1;
                timer.Start();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Matchimpro)
            {
                matchimpro = (Matchimpro)e.Parameter;
                Update_Match();
            }
        }

        private void Update_Match()
        {
            round_value = 1;
            scoreleft = 0;
            scoreright = 0;
            ui_leftname.Text = matchimpro.Team1;
            ui_rightname.Text = matchimpro.Team2;
            ui_startname1.Text = matchimpro.Team1;
            ui_startname2.Text = matchimpro.Team2;
            ui_scoreleft.Text = scoreleft.ToString();
            ui_scoreright.Text = scoreright.ToString();
            source_list_of_categories.Clear();
            ListInitialization();
            page.Children.Remove(ui_endround);
            page.Children.Remove(new_round);
            ui_scroll.Visibility = Visibility.Collapsed;
            ui_showinfo.Visibility = Visibility.Collapsed;
            if (!page.Children.Contains(ui_start))
            {
                page.Children.Add(ui_start);
            }
            notes_text.Document.SetText(Windows.UI.Text.TextSetOptions.None, $"Notes du match {matchimpro.Team1} - {matchimpro.Team2}" + Environment.NewLine);
            round_nb.Text = $"Manche {round_value} / {matchimpro.Rounds}";
        }

        private void LeftCardClick(object sender, PointerRoutedEventArgs e)
        {
            if (page.Children.Contains(ui_endround))
            {
                ui_trans1.Opacity = (ui_trans1.Opacity == 1) ? 0.4 : 1;
                VoteSelection();
            }
        }

        private void RightCardClick(object sender, PointerRoutedEventArgs e)
        {
            if (page.Children.Contains(ui_endround))
            {
                ui_trans2.Opacity = (ui_trans2.Opacity == 1) ? 0.4 : 1;
                VoteSelection();
            }
        }

        private void ShowInfo(object sender, PointerRoutedEventArgs e)
        {
            if (!page.Children.Contains(ui_infocard))
            {
                page.Children.Add(ui_infocard);
            }
            if (!page.Children.Contains(ui_endround))
            {
                ui_trans1.Opacity = 0.4;
                ui_trans2.Opacity = 0.4;
            }
        }

        private void HideInfo(object sender, RoutedEventArgs e)
        {
            page.Children.Remove(ui_infocard);
            if (!page.Children.Contains(ui_endround))
            {
                ui_trans1.Opacity = 1;
                ui_trans2.Opacity = 1;
            }
        }

        private void VoteSelection()
        {
            if (ui_trans1.Opacity == 1 || ui_trans2.Opacity == 1)
            {
                ui_votemessage.Visibility = Visibility.Collapsed;
                ui_votebox.Visibility = Visibility.Visible;
                ui_votecomment.Text = (ui_trans1.Opacity == ui_trans2.Opacity) ? "Égalité" : (ui_trans1.Opacity == 1) ? matchimpro.Team1 : matchimpro.Team2;
            }
            else
            {
                ui_votemessage.Visibility = Visibility.Visible;
                ui_votebox.Visibility = Visibility.Collapsed;
            }
        }
        private void PageLoaded(object sender, RoutedEventArgs e)
        {            
            page.Children.Remove(ui_infocard);
            if (!page.Children.Contains(ui_endround))
            {
                ui_trans1.Opacity = 1;
                ui_trans2.Opacity = 1;
            }
        }

        private void NextRound(object sender, RoutedEventArgs e)
        {
            if (round_value< matchimpro.Rounds) {
                if (!page.Children.Contains(new_round))
                {
                    ui_votemessage.Visibility = Visibility.Visible;
                    ui_votebox.Visibility = Visibility.Collapsed;
                    page.Children.Remove(ui_endround);
                    round_value++;
                    page.Children.Add(new_round);
                    round_nb.Text = $"Manche {round_value} / {matchimpro.Rounds}";
                    if (ui_trans1.Opacity == 1)
                    {
                        scoreleft++;
                    }
                    if (ui_trans2.Opacity == 1)
                    {
                        scoreright++;
                    }
                    ui_scoreleft.Text = scoreleft.ToString();
                    ui_scoreright.Text = scoreright.ToString();
                    ui_trans1.Opacity = 1;
                    ui_trans2.Opacity = 1;
                }
                if (source_list_of_categories.Count() == 0)
                {
                    source_list_of_categories.Add("Libre");
                    list_of_categories.SelectedIndex = 0;
                }
            }
            else
            {
                //TODO Afficher un écran de fin de match avec score final
            }
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            ui_scroll.Visibility = Visibility.Visible;
            ui_showinfo.Visibility = Visibility.Visible;
            page.Children.Remove(ui_start);
            page.Children.Add(new_round);
        }

        private void random_category(object sender, RoutedEventArgs e)
        {
            Random rand_index = new Random();
            if (source_list_of_categories.Count() == 0)
                source_list_of_categories.Add("Libre");
            list_of_categories.SelectedIndex = rand_index.Next(0, source_list_of_categories.Count());    
        }

        private void timerdown(object sender, object e)
        {
            if (min == 0 && sec == 0)
            {
                timer.Stop();
                show();
                return;
            }
        
            if (sec == 0)
            {
                min--;
                sec = 60;
            }
            sec--;
            time_left.Text = min + "min et " + sec + "s";
        }
    }
}
