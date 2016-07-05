using System;
using System.Collections.Generic; // List
using System.Collections.ObjectModel; // ObservableCollection

namespace Wolfpack
{
	/// <summary>
	/// Méthodes d'extensions des listes
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Permet de sélectionner un joueur aléatoirement parmis la liste des joueurs
		/// </summary>
		/// <returns>Le nom du joueur</returns>
		/// <param name="lstPlayers">La liste des joueurs</param>
		public static string RandomPlayer(this ObservableCollection<Player> lstPlayers)
		{
			int iNoPlayer = lstPlayers.Count;
			Random rnd = new Random();
			int iRndNumber = rnd.Next(iNoPlayer);
			if (iNoPlayer > 0)
				return lstPlayers[iRndNumber].DisplayName;
			else
				return String.Empty;
		}

		/// <summary>
		/// Permet de sélectionner un challenge aléatoirement parmis la liste des challenges
		/// </summary>
		/// <returns>The challenge.</returns>
		/// <param name="lstChallenges">Lst challenges.</param>
		public static Challenge RandomChallenge(this List<Challenge> lstChallenges)
		{
			Challenge chRandom;

			// On va obtenir le nombre de challenge et générer un nombre aléatoire en conséquence
			int iNbChallenge = lstChallenges.Count;
			Random rnd = new Random();
			int iRndNumber = rnd.Next(iNbChallenge);

			// On s'assure qu'il y a au moins un challenge
			if (iNbChallenge > 0)
				chRandom = lstChallenges[iRndNumber];
			else
				chRandom = new Challenge();
			
			// On retire de la liste le challenge qui va être fait
			lstChallenges.RemoveAt(iRndNumber);

			return chRandom;
		}
	}
}

