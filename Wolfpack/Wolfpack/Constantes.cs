using System;
using System.Collections.ObjectModel;

namespace Wolfpack
{
	public class Constantes
	{
		// Les types de challenge
		public const string TYPE_CONSEQUENCE = "Consequence";
		public const string TYPE_GAME = "Game";
		public const string TYPE_RULE = "Rule";

		// Les valeurs possibles de "who"
		public const string WHO_ALL = "All";
		public const string WHO_GIRLS = "Girls";
		public const string WHO_BOYS = "Boys";
		public const string WHO_SOLO = "Solo";
		public const string WHO_DOUBLE = "Double";

		// Nombre de challenge dans une partie
		public const int GAME_LENGTH = 40;
	}
}

