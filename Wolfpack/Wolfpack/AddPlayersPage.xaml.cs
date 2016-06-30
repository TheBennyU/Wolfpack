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
			players.Add(new Player { DisplayName = "Test" });
		}

		void OnSelection(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			players.Add(new Player { DisplayName = "Joueur" });
			PlayerView.ItemsSource = players;

		}
	}
}

