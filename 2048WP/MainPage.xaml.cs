using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using _2048WP.Resources;
using _2048WP.CustomControls;
using _2048.Core;


namespace _2048WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        const int Side = 4;
        GameLogick _gameLogick;
        Dictionary<Coordinate, Tile> _tiles;

        Dictionary<Coordinate, Tile> Tiles
        {
            get
            {
                if (_tiles == null)
                    _tiles = new Dictionary<Coordinate, Tile>();
                return _tiles;
            }
        }

        private GameLogick GameLogick
        {
            get
            {
                if (_gameLogick == null)
                    _gameLogick = new GameLogick(Side);
                return _gameLogick;
            }
        }
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            PlayingSurface.ManipulationCompleted += PlayingSurface_ManipulationCompleted;
            PlayingSurface.ManipulationStarted += PlayingSurface_ManipulationStarted;
            PlayingSurface.ManipulationDelta += PlayingSurface_ManipulationDelta;
            GameLogick.StateChanged += GameLogick_StateChanged;
            NewGame();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void PlayingSurface_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            
        }

        void PlayingSurface_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
        }

        void PlayingSurface_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            Point velocities = e.FinalVelocities.LinearVelocity;
            if (velocities.X > 0)
                GameLogick.ExecuteMove(Direction.Right);
            else if(velocities.X < 0)
                GameLogick.ExecuteMove(Direction.Left);
            else if(velocities.Y < 0)
                GameLogick.ExecuteMove(Direction.Up);
            else if (velocities.Y > 0)
                GameLogick.ExecuteMove(Direction.Down);
        }

        void GameLogick_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (PlayingSurface.Children.Count == 0)
            {
                foreach (var item in GameLogick.TilesState)
                {
                    Tile tile = new Tile(item.Value);
                    Grid.SetColumn(tile, item.Key.X-1);
                    Grid.SetRow(tile, item.Key.Y-1);
                    PlayingSurface.Children.Add(tile);
                    Tiles.Add(item.Key, tile);
                }
            }
            else
            {
                foreach (var tile in GameLogick.TilesState)
                {
                    Tiles[tile.Key].Value = tile.Value;
                }
            }
            Score.Text = GameLogick.Soccer.ToString();
        }

        private void NewGame()
        {
            GameLogick.NewGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        
    }
}