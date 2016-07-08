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
		List<Tuple<Challenge, DateTime, string, string, string>> lstCancel = new List<Tuple<Challenge, DateTime, string, string, string>>();
		int turnCount = 0;

		/// <summary>
		/// Permet d'afficher la page et d'obtenir une liste de challenges
		/// </summary>
		/// <param name="inPlayers">Liste des joueurs</param>
		public GamePage(ObservableCollection<Player> inPlayers)
		{
			InitializeComponent();
			players = inPlayers;
			StartGame();
		}

		/// <summary>
		/// Permet d'initialiser tout les composants du jeu
		/// </summary>
		public void StartGame()
		{
			// Pour loader le fichier XML, permet de repartir la partie 
			var assembly = typeof(Challenge).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("Wolfpack.Challenge.xml");

			using (var reader = new System.IO.StreamReader(stream))
			{
				var serializer = new XmlSerializer(typeof(List<Challenge>), new XmlRootAttribute("ChallengeList"));
				lstChallenges = (List<Challenge>)serializer.Deserialize(reader);
			}

			// On vide la liste de challenge à canceller
			lstCancel.Clear();
			turnCount = 0;
			GenerateChallenge();
		}

		/// <summary>
		/// Action effectuée lorsque le bouton Next est cliqué
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void btnNext_Clicked(object sender, EventArgs e)
		{
			if (turnCount <= Constantes.GAME_LENGTH && lstChallenges.Count > 0)
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
			 //StartGame();
			Navigation.PushModalAsync(new AddPlayersPage(players));

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
		/// Permet de générer un challenge. OSTI DE CANCER COMME CODE DÉSOLÉ!
		/// </summary>
		/// <returns>The challenge.</returns>
		private void GenerateChallenge()
		{
			bool generateNewCh = true;
			string sText = String.Empty;
			Color cBackground;
			Color cText;
			string who = "";
			string who2 = "";
			string who3 = "";
			Challenge chCurrentChallenge = new Challenge();

			// On vérifie si un challenge est à canceller
			if (lstCancel != null)
			{
				foreach (Tuple<Challenge, DateTime, string, string, string> tup in lstCancel)
				{
					if (tup.Item2 < DateTime.Now)
					{
						chCurrentChallenge = tup.Item1;
						sText = String.Format(chCurrentChallenge.Cancel.Text, tup.Item3, tup.Item4, tup.Item5);
						// Puisqu'on cancel un challenge, on ne veut pas en générer un nouveau
						generateNewCh = false;
						lstCancel.Remove(tup);
						break;
					}
				}
			}

			// Si aucun challenge à canceller on veut générer un nouveau challenge
			if (generateNewCh)
			{
				// On génère un nouveau challenge
				chCurrentChallenge = lstChallenges.RandomChallenge();

				// On trouve aléatoirement 3 joueurs différents
				who = players.RandomPlayer();
				while (who2 == "" || who2 == who)
				{
					who2 = players.RandomPlayer();
				}

				while (who3 == "" || who3 == who || who3 == who2)
				{
					who3 = players.RandomPlayer();
				}

				// On ajoute les challenges ayant un temps limite dans la liste à vérifier
				if (chCurrentChallenge.Cancel.Time != "0")
				{
					int time = Int32.Parse(chCurrentChallenge.Cancel.Time);
					lstCancel.Add(Tuple.Create(chCurrentChallenge, DateTime.Now.AddMinutes(time), who, who2, who3));
				}

				// On formate le text du challenge selon la valeur de Who
				if (chCurrentChallenge.Who == Constantes.WHO_ALL ||
					chCurrentChallenge.Who == Constantes.WHO_BOYS ||
					chCurrentChallenge.Who == Constantes.WHO_GIRLS)
					sText = chCurrentChallenge.TextDescription;
				else if (chCurrentChallenge.Who == Constantes.WHO_SOLO || 
				         chCurrentChallenge.Who == Constantes.WHO_DOUBLE)
					sText = String.Format(chCurrentChallenge.TextDescription, who, who2, who3);
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

			turnCount ++;

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

