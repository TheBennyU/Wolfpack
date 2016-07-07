using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;


public class Player
{
	public string DisplayName { get; set; }
}

namespace Wolfpack
{
	public partial class AddPlayersPage : ContentPage
	{

		ObservableCollection<Player> players = new ObservableCollection<Player>();

		public AddPlayersPage()
		{
			InitializeComponent();
			PlayerView.ItemsSource = players;

			// S'il n'y a aucun player (en début de partie), on en créé 1
			if (players.Count == 0)
			{
				players.Add(new Player { DisplayName = "Joueur 1" });
			}

		}

		void OnSelection(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

		}

		void btnAddPlayer_Clicked(object sender, System.EventArgs e)
		{
			string playerName = String.Format("Joueur {0}", players.Count + 1);
			players.Add(new Player { DisplayName = playerName });
			PlayerView.ItemsSource = players;
		}

		void btnStart_Clicked(object sender, System.EventArgs e)
		{
			Navigation.PushModalAsync(new GamePage(players));
		}
	}
}

