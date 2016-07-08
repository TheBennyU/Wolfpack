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
		List<Tuple<Challenge, DateTime, string>> lstCancel;

		/// <summary>
		/// Permet d'afficher la page et d'obtenir une liste de challenges
		/// </summary>
		/// <param name="inPlayers">Liste des joueurs</param>
		public GamePage(ObservableCollection<Player> inPlayers)
		{
			InitializeComponent();
			players = inPlayers;
			lstCancel = new List<Tuple<Challenge, DateTime, string>>();
			StartGame();
		}

		public void StartGame()
		{
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
				GameEvent.Text = "Partie terminée !";
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
			 btnNewGame.IsVisible = false;
			 Navigation.PushModalAsync(new AddPlayersPage(players));

			// Navigation.PushModalAsync(new AddPlayersPage());

		}

		/// <summary>
		/// Permet de trouver le challenge s'applique à qui
		/// </summary>
		/// <returns>Who</returns>
		public string FindWho(Challenge CurCha)
		{
			string who = "";

			// On formate le text du challenge selon la valeur de Who
			if (CurCha.Who == Constantes.WHO_ALL ||
				CurCha.Who == Constantes.WHO_BOYS ||
				CurCha.Who == Constantes.WHO_GIRLS)
				who = CurCha.TextDescription;
			else if (CurCha.Who == Constantes.WHO_SOLO)
				who = players.RandomPlayer();
			else
			{
				// Erreur
			}
			return who;
		}


		/// <summary>
		/// Permet de générer un challenge
		/// </summary>
		/// <returns>The challenge.</returns>
		private void GenerateChallenge()
		{
			bool ahah = true;
			string sText = String.Empty;
			Color cBackground;
			Color cText;
			string tText = String.Empty;
			string who = "";
			Challenge chCurrentChallenge = new Challenge();

			// on vérifie si un challenge est à canceller
			if (lstCancel != null)
			{
				foreach (Tuple<Challenge, DateTime, string> tup in lstCancel)
				{
					if (tup.Item2 < DateTime.Now)
					{
						chCurrentChallenge = tup.Item1;
						sText = String.Format(chCurrentChallenge.Cancel.Text, tup.Item3);
						ahah = false;
						lstCancel.Remove(tup);
						break;
					}
				}
			}

			if (ahah)
			{
				chCurrentChallenge = lstChallenges.RandomChallenge();

				// On ajoute les challenges ayant un temps limite dans la liste
				if (chCurrentChallenge.Cancel.Time != "0")
				{
					int time = Int32.Parse(chCurrentChallenge.Cancel.Time);
					who = FindWho(chCurrentChallenge);
					lstCancel.Add(Tuple.Create(chCurrentChallenge, DateTime.Now.AddMinutes(time), who));
				}


				// On formate le text du challenge selon la valeur de Who
				if (chCurrentChallenge.Who == Constantes.WHO_ALL ||
					chCurrentChallenge.Who == Constantes.WHO_BOYS ||
					chCurrentChallenge.Who == Constantes.WHO_GIRLS)
					sText = chCurrentChallenge.TextDescription;
				else if (chCurrentChallenge.Who == Constantes.WHO_SOLO)
				{
					who = players.RandomPlayer();
					sText = String.Format(chCurrentChallenge.TextDescription, who);
				}
				else
				{
					// Erreur
				}
			}

			// On change l'arrière plan selon le type de challenge
			switch (chCurrentChallenge.Type)
			{
				case Constantes.TYPE_CONSEQUENCE:
					cText = Color.White;
					cBackground = Color.FromHex("2D3047");
					break;
				case Constantes.TYPE_GAME:
					cText = Color.White;
					cBackground = Color.FromHex("1B998B");
					break;
				case Constantes.TYPE_RULE:
					cText = Color.White;
					cBackground = Color.FromHex("E84855");
					break;
				default:
					cText = Color.Lime;
					cBackground = Color.White;
					// Erreur
					break;
			}

			if (chCurrentChallenge.Type == "Game")
			{
				tText = "Game";
			}

			// On actualise le View
			GameEvent.Text = sText;
			GameEvent.TextColor = cText;
			slChallenge.BackgroundColor = cBackground;
		}
	}


	// la classe qui représente le xml
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

