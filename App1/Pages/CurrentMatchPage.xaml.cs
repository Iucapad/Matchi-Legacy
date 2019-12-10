using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private Matchimpro matchimpro;
        private ObservableCollection<string> source_list_of_categories = new ObservableCollection<string>();
        private MatchStorage store = new MatchStorage();//objet relatif au stockage
        private DispatcherTimer timer = new DispatcherTimer();
        private ObservableCollection<string> source_time_choice = new ObservableCollection<string>();
        private List<string> source_nbre_times = new List<string>();
        private MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content; //prend la mainpage actuelle comme mainpage
        private ContentDialog ErrorDialog = new ContentDialog //Squelette de base d'un message d'erreur, à compléter en cas d'erreur
        {
            Title = "Attention",
            Content = "",
            CloseButtonText = "Ok"
        };
        private int min;
        private int sec;
        int maxtime;
        int curtime=0;
        float comptime=0;
        bool in_pause = false;
        bool is_bigpicture = false;
        int round_value = 1;
        int scoreleft = 0;
        int scoreright = 0;
        int times;        

        public CurrentMatchPage()
        {
            this.InitializeComponent();
            new_round.Visibility = Visibility.Visible;
            ui_infocard.Visibility = Visibility.Visible;
            SharedShadow.Receivers.Add(Receiver);
            if (mainFrame.navigationView.MenuItems.Count < 6)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                mainFrame.navigationView.MenuItems.Insert(0, new NavigationViewItem
                {
                    Name = "CURRENT",
                    IsSelected = true,
                    Content = resourceLoader.GetString("CurrentMatch"),
                    Icon = new SymbolIcon((Symbol)0xE945),
                    Tag = "currentNav"                    
                });
            }
            for (int i = 1; i <= 10; i++)
            {
                source_time_choice.Add(i + " min");
                source_nbre_times.Add(i.ToString());
            }         
            timer_selection.ItemsSource = source_time_choice;
            times_selection.ItemsSource = source_nbre_times;
            timer_selection.SelectedIndex = 0;
            times_selection.SelectedIndex = 0;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timerdown;
            ui_progressbar.Foreground = new SolidColorBrush(Colors.RoyalBlue);
        }
        private async void ListInitialization()
        {
            StorageFolder storageFolder = store.Folder;
            IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync();
            if (!File.Exists(storageFolder.Path + Path.DirectorySeparatorChar + "Categories.catei"))
                return;
            StorageFile cate_file = await storageFolder.GetFileAsync("Categories.catei");
            source_list_of_categories = new ObservableCollection<string>((await FileIO.ReadLinesAsync(cate_file)).Distinct());
            list_of_categories.ItemsSource = source_list_of_categories;
        }
        private void EndRound()
        {
            if (!page.Children.Contains(ui_endround))
            {
                page.Children.Remove(ui_infocard);
                ui_scroll.Opacity = 1;
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
            if (((mainFrame)).ActualWidth < 750)
            {
                //ui_nbjouteurs.Margin = new Thickness(0, 0, 30, 55);
                if (ui_container.Orientation == Orientation.Horizontal)
                {
                    match_name.Margin = new Thickness(50, 0, 0, 0);
                    ui_scroll.Margin = new Thickness(0, 60, 0, 60);
                    ui_container.Orientation = Orientation.Vertical;
                    ui_container.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ui_container.Padding = new Thickness(30, 5, 30, 5);
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
                    ui_catname.Margin = new Thickness(30, 10, 0, 60);
                    
                    ui_progressinfo.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ui_progressbar.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ui_progressbar.Width = double.NaN;
                    ui_progressinfo.Margin = new Thickness(35, 30, 35, 0);
                    ui_showinfo.Margin = new Thickness(0);
                    ui_showinfo.BorderThickness = new Thickness(0, 1, 0, 0);
                    ui_infocard.Margin = new Thickness(0);
                    ui_infocard.BorderThickness = new Thickness(0, 1, 0, 0);
                }
            }
            else if (ui_container.Orientation == Orientation.Vertical)
            { 
                match_name.Margin = new Thickness(20, 10, 0, 0);
                ui_scroll.Margin = new Thickness(0, 80, 0, 60);
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
                ui_catname.Margin = new Thickness(120, 30, 0, 0);
                //ui_nbjouteurs.Margin = new Thickness(0, 0, 150, 0);
                ui_progressinfo.HorizontalAlignment = HorizontalAlignment.Center;
                ui_progressbar.HorizontalAlignment = HorizontalAlignment.Center;
                ui_progressbar.Width = 300;
                ui_progressinfo.Margin = new Thickness(0, 30, 0, 60);
                ui_showinfo.Margin = new Thickness(30, 0, 30, 0);
                ui_showinfo.BorderThickness = new Thickness(1, 1, 1, 0);
                ui_infocard.Margin = new Thickness(30, 0, 30, 0);
                ui_infocard.BorderThickness = new Thickness(1, 1, 1, 0);                
            }
        }

        private async void Start_click(object sender, RoutedEventArgs e)
        {
            if (list_of_categories.SelectedItem is null || timer_selection.SelectedItem is null || times_selection.SelectedItem is null)
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
                ui_penalty.Visibility = Visibility.Visible;

                if(list_of_categories.SelectedValue.ToString() != "Libre")
                    source_list_of_categories.Remove(list_of_categories.SelectedValue.ToString());

                if (times_selection.SelectedIndex>0)
                {
                    ui_matchlength.Text = $"{times_selection.SelectedIndex + 1} x {timer_selection.SelectedIndex + 1}:00";
                }
                else
                {
                    ui_matchlength.Text = $"{timer_selection.SelectedIndex + 1}:00";
                }                
                ui_progressbar.Foreground = new SolidColorBrush(Colors.RoyalBlue);                
                sec = 0;
                min = timer_selection.SelectedIndex+1;
                maxtime = 60 * min;
                curtime = 0;
                times = times_selection.SelectedIndex + 1;
                timer.Start();
                ui_progressinfo.Visibility = Visibility.Visible;
                ui_controlstimer.Visibility = Visibility.Visible;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Matchimpro)
            {
                matchimpro = (Matchimpro)e.Parameter;
                Update_Match();
            }
            //Charge le paramètre de thème et le définit
            String app_setting = localSettings.Values["theme_setting"] as string;
            ApplyTheme(app_setting);
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
            page.Children.Remove(ui_interlude);
            page.Children.Remove(ui_endround);
            page.Children.Remove(new_round);
            page.Children.Remove(ui_victory);
            page.Children.Remove(anim_givepoint);
            ui_scroll.Visibility = Visibility.Collapsed;
            ui_showinfo.Visibility = Visibility.Collapsed;
            ui_endround.Visibility = Visibility.Visible;
            if (!page.Children.Contains(ui_start))
            {
                page.Children.Add(ui_start);
            }
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            notes_text.Document.SetText(Windows.UI.Text.TextSetOptions.None, resourceLoader.GetString("NotesDefault") + $" {matchimpro.Team1} - {matchimpro.Team2}" + Environment.NewLine);            
            round_nb.Text = resourceLoader.GetString("RoundNb") + $" {round_value} / {matchimpro.Rounds}";
            timer_selection.SelectedIndex = 0;
            times_selection.SelectedIndex = 0;
            //Masque le bouton de projection si pas de second écran connecté
            bool available = ProjectionManager.ProjectionDisplayAvailable;            
            if (true)//!available)
            {
                ui_bigpicture.Visibility = Visibility.Visible;
            }
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
                ui_notesbackdrop.Visibility = Visibility.Visible;
                ui_notesbackdrop.Opacity = 1;
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
            ui_notesbackdrop.Visibility = Visibility.Collapsed;
            ui_notesbackdrop.Opacity = 0;
            if (!page.Children.Contains(ui_endround) && !page.Children.Contains(ui_interlude))
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
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                ui_votecomment.Text = (ui_trans1.Opacity == ui_trans2.Opacity) ? resourceLoader.GetString("Equality") : (ui_trans1.Opacity == 1) ? matchimpro.Team1 : matchimpro.Team2;
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
            ui_notesbackdrop.Opacity = 0;
            ui_notesbackdrop.Visibility = Visibility.Collapsed;
            if (!page.Children.Contains(ui_endround))
            {
                ui_trans1.Opacity = 1;
                ui_trans2.Opacity = 1;
            }
        }

        private async void NextRound(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            //animation de fin de manche et distribution des points
            if (!page.Children.Contains(anim_givepoint))
            {
                page.Children.Add(anim_givepoint);                
            }
            ui_scroll.Opacity = 0.2;
            ui_penalty.Visibility = Visibility.Collapsed;
            if ((ui_trans1.Opacity == 1)&& (ui_trans2.Opacity != 1))
            {
                scoreleft++;
                ui_leftcard.Rotation = -3;
                anim_text.Text = resourceLoader.GetString("PointMessage") + matchimpro.Team1;
            }
            else if ((ui_trans1.Opacity != 1) && (ui_trans2.Opacity == 1))
            {
                scoreright++;
                ui_rightcard.Rotation = 3;
                anim_text.Text = resourceLoader.GetString("PointMessage") + matchimpro.Team2;
            }
            else if ((ui_trans1.Opacity == 1)&& (ui_trans2.Opacity == 1))
            {
                scoreleft++;
                scoreright++;
                ui_leftcard.Rotation = -3;
                ui_rightcard.Rotation = 3;
                anim_text.Text = resourceLoader.GetString("EqualityMessage");
            }
            ui_scoreleft.Text = scoreleft.ToString();
            ui_scoreright.Text = scoreright.ToString();
            ui_trans1.Opacity = 1;
            ui_trans2.Opacity = 1;
            await Task.Delay(TimeSpan.FromSeconds(2));
            ui_leftcard.Rotation = 0;
            ui_rightcard.Rotation = 0;
            page.Children.Remove(anim_givepoint);
            ui_scroll.Opacity = 1;
            if (round_value< matchimpro.Rounds) {
                //Affiche l'interface de la nouvelle manche
                if (!page.Children.Contains(new_round))
                {
                    ui_votemessage.Visibility = Visibility.Visible;
                    ui_votebox.Visibility = Visibility.Collapsed;
                    page.Children.Remove(ui_endround);
                    round_value++;
                    page.Children.Add(new_round);                    
                    round_nb.Text = $"Manche {round_value} / {matchimpro.Rounds}";                    
                }
                if (source_list_of_categories.Count() == 0)
                {
                    source_list_of_categories.Add("Libre");
                    list_of_categories.SelectedIndex = 0;
                }
            }
            else
            {
                //Affiche un écran de fin de match avec score final
                if (!page.Children.Contains(ui_victory))
                {
                    page.Children.Add(ui_victory);
                    ui_endround.Visibility = Visibility.Collapsed;
                    page.Children.Remove(ui_endround);                    
                    ui_scroll.Visibility = Visibility.Collapsed;
                    if (scoreleft > scoreright)
                    {
                        //Victoire de équipe 1
                        ui_victorymessage.Text = resourceLoader.GetString("VictoryMessage") + matchimpro.Team1;
                    }
                    else if (scoreleft < scoreright)
                    {
                        //Victoire de équipe 2
                        ui_victorymessage.Text = resourceLoader.GetString("VictoryMessage") + matchimpro.Team2;
                    }
                    else
                    {
                        //Egalité
                        ui_victorymessage.Text = resourceLoader.GetString("Equality");
                    }
                }
            }
        }
        private void ApplyTheme(string app_setting)
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
            ui_trans1.Fill = leftcardBrush;
            ui_startleftcard.Fill = leftcardBrush;

            LinearGradientBrush rightcardBrush = new LinearGradientBrush();
            rightcardBrush.StartPoint = new Point(0.1, 0);
            rightcardBrush.EndPoint = new Point(0.25, 1);
            rightcardBrush.GradientStops.Add(new GradientStop { Color = color2a, Offset = 0.2 });
            rightcardBrush.GradientStops.Add(new GradientStop { Color = color2b, Offset = 0.9 });
            ui_trans2.Fill = rightcardBrush;
            ui_startrightcard.Fill = rightcardBrush;
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

        private async void timerdown(object sender, object e)
        {
            if (min == 0 && sec == 0)
            {
                times--;
                ui_progressbar.Value = 0;
                ui_controlstimer.Visibility = Visibility.Collapsed;
                ui_progressinfo.Visibility = Visibility.Collapsed;
                timer.Stop();
                if(times == 0)
                {
                    EndRound();                    
                    return;
                }
                else
                {
                    page.Children.Remove(ui_infocard);
                    ui_scroll.Opacity = 1;
                    page.Children.Add(ui_interlude);
                    ui_trans1.Opacity = 0.4;
                    ui_trans2.Opacity = 0.4;
                    ui_intermessage.Text = "La prochaine partie de " + (timer_selection.SelectedValue) + " va commencer. Il reste " + times + " passage(s) avant la fin de la manche.";
                }
            }
            else
            {
                curtime++;
                comptime = (float)curtime / (float)maxtime;
                ui_progressbar.Value = comptime * 100;
            }
        
            if (sec == 0)
            {
                min--;
                sec = 60;
            }
            sec--;            
            
            if (sec >= 10)
                time_left.Text = min + ":" + sec;
            else
                time_left.Text = min + ":0" + sec;

            if (min == 0 && sec <= 10)
                ui_progressbar.Foreground = new SolidColorBrush (Colors.Firebrick);
        }

        private void pause_timer(object sender, RoutedEventArgs e)
        {
            if (!in_pause)
            {
                timer.Stop();
                ui_progressbar.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
                ui_pausetimer.Content = "\uE768";
                in_pause = true;
            }
            else
            {
                timer.Start();
                ui_progressbar.Foreground = new SolidColorBrush(Colors.RoyalBlue);
                ui_pausetimer.Content = "\uE769";
                in_pause = false;
            }
        }

        private async void stop_timer(object sender, RoutedEventArgs e)
        {
            ErrorDialog.Content = "Êtes-vous sûr(e) de vouloir mettre fin à cette manche ?";
            ErrorDialog.PrimaryButtonText = "Oui";
            ErrorDialog.CloseButtonText = "Annuler";
            ui_progressbar.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
            timer.Stop();
            in_pause = false;
            ui_pausetimer.Content = "\uE769";

            ContentDialogResult result = await ErrorDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ui_controlstimer.Visibility = Visibility.Collapsed;
                ui_progressinfo.Visibility = Visibility.Collapsed;
                ui_progressbar.Value = 100;
                EndRound();
            }
            else
            {
                ui_progressbar.Foreground = new SolidColorBrush(Colors.RoyalBlue);
                timer.Start();
            }
        }

        private void ContinueRound(object sender, RoutedEventArgs e)
        {           
            min = timer_selection.SelectedIndex + 1;
            sec = 0;
            curtime = 0;
            ui_pausetimer.Content = "\uE769";
            ui_progressbar.Foreground = new SolidColorBrush(Colors.RoyalBlue);
            timer.Start();
            ui_trans1.Opacity = 1;
            ui_trans2.Opacity = 1;
            time_left.Text ="";
            page.Children.Remove(ui_interlude);
            ui_controlstimer.Visibility = Visibility.Visible;
            ui_progressinfo.Visibility = Visibility.Visible;
        }

        private async void OpenBigPicture(object sender, RoutedEventArgs e)
        {
            var NewWindow = CoreApplication.CreateNewView();
            //Initialise le paramètre contenant les infos du match
            var currentMatchInfo = new RoundInfo();
            currentMatchInfo.Team1 = matchimpro.Team1;
            currentMatchInfo.Team2 = matchimpro.Team2;
            currentMatchInfo.Score1 = scoreleft.ToString();
            currentMatchInfo.Score2 = scoreright.ToString();
            currentMatchInfo.Category = ui_catname.Text;
            currentMatchInfo.Length = ui_matchlength.Text;

            //Crée une seconde vue et crée une deuxième fenêtre
            int NewWindowid = 0;
            int Windowid = ApplicationView.GetForCurrentView().Id;
            String app_setting = localSettings.Values["theme_setting"] as string;
            await NewWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame newframe = new Frame();
                newframe.Navigate(typeof(BigPicturePage), currentMatchInfo);

                Window.Current.Content = newframe;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = matchimpro.Name;

                NewWindowid = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(NewWindowid);
           

            //Ouvre la nouvelle fenêtre en plein écran si second écran connecté
            bool available = ProjectionManager.ProjectionDisplayAvailable;
            if (true)//!available)
            {
                await ProjectionManager.StartProjectingAsync(NewWindowid, Windowid);
            }
        }
    }
}
