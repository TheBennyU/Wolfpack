using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;


namespace Wolfpack
{
	public partial class GamePage : ContentPage
	{
		ObservableCollection<Player> players;

		public GamePage(ObservableCollection<Player> inPlayers)
		{
			InitializeComponent();
			players = inPlayers;

			// Pour loader le fichier XML
			var assembly = typeof(Challenge).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("Wolfpack.Challenge.xml");

			List<Challenge> challenges;
			using (var reader = new System.IO.StreamReader(stream))
			{
				var serializer = new XmlSerializer(typeof(List<Challenge>), new XmlRootAttribute("ChallengeList"));
				challenges = (List<Challenge>)serializer.Deserialize(reader);
			}

			Random rnd = new Random();
			int randomPlayer = rnd.Next(players.Count);
			Event.Text = players[randomPlayer].DisplayName + " va boire en tabarnak";
		}

		void btnNext_Clicked(object sender, EventArgs e)
		{
			Random rnd = new Random();
			int randomPlayer = rnd.Next(players.Count);
			Event.Text = players[randomPlayer].DisplayName + " va boire en ccriss";
		}
	}

	[XmlRoot(ElementName = "Challenge")]
	public class Challenge
	{
		[XmlElement(ElementName = "type")]
		public string Type { get; set; }
		[XmlElement(ElementName = "textDescription")]
		public string TextDescription { get; set; }
		[XmlElement(ElementName = "who")]
		public string Who { get; set; }
		[XmlElement(ElementName = "cancel")]
		public string Cancel { get; set; }
	}

	[XmlRoot(ElementName = "ChallengeList")]
	public class ChallengeList
	{
		[XmlElement(ElementName = "Challenge")]
		public List<Challenge> Challenge { get; set; }
	}
}

