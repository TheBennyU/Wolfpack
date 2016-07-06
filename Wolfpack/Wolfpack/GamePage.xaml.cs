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
		List<Challenge> lstChallenges;

		/// <summary>
		/// Permet d'afficher la page et d'obtenir une liste de challenges
		/// </summary>
		/// <param name="inPlayers">Liste des joueurs</param>
		public GamePage(ObservableCollection<Player> inPlayers)
		{
			InitializeComponent();
			players = inPlayers;

			// Pour loader le fichier XML
			var assembly = typeof(Challenge).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("Wolfpack.Challenge.xml");
			                        
			using (var reader = new System.IO.StreamReader(stream))
			{
				var serializer = new XmlSerializer(typeof(List<Challenge>), new XmlRootAttribute("ChallengeList"));
				lstChallenges = (List<Challenge>)serializer.Deserialize(reader);
			}

			GenerateChallenge();
		}

		/// <summary>
		/// Action effectuée lorsque le bouton Next est cliqué
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void btnNext_Clicked(object sender, EventArgs e)
		{
			if (lstChallenges.Count > 0)
				GenerateChallenge();
			else
			{
				Event.Text = "Partie terminée !";
				btnNext.IsVisible = false;
				btnNewGame.IsVisible = true;
			}
		}

		/// <summary>
		/// Action effectuée lorsque le bouton New Game est cliqué
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void btnNewGame_Clicked(object sender, EventArgs e)
		{
			
		}

		/// <summary>
		/// Permet de générer un challenge
		/// </summary>
		/// <returns>The challenge.</returns>
		private void GenerateChallenge()
		{
			Challenge chCurrentChallenge = lstChallenges.RandomChallenge();

			string sText = String.Empty;
			Color cBackground;
			Color cText;

			// On formate le text du challenge selon la valeur de Who
			if (chCurrentChallenge.Who == Constantes.WHO_ALL ||
				chCurrentChallenge.Who == Constantes.WHO_BOYS ||
				chCurrentChallenge.Who == Constantes.WHO_GIRLS)
				sText = chCurrentChallenge.TextDescription;
			else if (chCurrentChallenge.Who == Constantes.WHO_SOLO)
				sText = String.Format(chCurrentChallenge.TextDescription, players.RandomPlayer());
			else
			{
				// Erreur
			}



			if (chCurrentChallenge.Cancel.Time != "0")
			{
				int numVal = Int32.Parse(chCurrentChallenge.Cancel.Time);
				DateTime time = DateTime.Now.AddMinutes(numVal);
				// sText = chCurrentChallenge.Cancel.Text;
			}

			// On change l'arrière plan selon le type de challenge
			switch (chCurrentChallenge.Type)
			{
				case Constantes.TYPE_CONSEQUENCE:
					cText = Color.White;
					cBackground = Color.Navy;
					break;
				case Constantes.TYPE_GAME:
					cText = Color.White;
					cBackground = Color.Green;
					break;
				case Constantes.TYPE_RULE:
					cText = Color.Pink;
					cBackground = Color.Purple;
					break;
				default:
					cText = Color.Lime;
					cBackground = Color.Black;
					// Erreur
					break;
			}

			// On actualise le View
			Event.Text = sText;
			Event.TextColor = cText;
			slChallenge.BackgroundColor = cBackground;
		}
	}

	[XmlRoot(ElementName = "cancel")]
	public class Cancel
	{
		[XmlElement(ElementName = "time")]
		public string Time { get; set; }
		[XmlElement(ElementName = "text")]
		public string Text { get; set; }
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
		public Cancel Cancel { get; set; }
	}

	[XmlRoot(ElementName = "ChallengeList")]
	public class ChallengeList
	{
		[XmlElement(ElementName = "Challenge")]
		public List<Challenge> Challenge { get; set; }
	}

}

