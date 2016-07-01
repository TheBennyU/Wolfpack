using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Wolfpack
{
	public partial class GamePage : ContentPage
	{
		ObservableCollection<Player> players;

		public GamePage(ObservableCollection<Player> inPlayers)
		{
			InitializeComponent();
			players = inPlayers;

			Random rnd = new Random();
			int randomPlayer = rnd.Next(players.Count);
			Event.Text = players[randomPlayer].DisplayName + " va boire en tabarnak";

		}

		void btnNext_Clicked(object sender, System.EventArgs e)
		{
			Random rnd = new Random();
			int randomPlayer = rnd.Next(players.Count);
			Event.Text = players[randomPlayer].DisplayName + " va boire en ccriss";
		}
	}
}

