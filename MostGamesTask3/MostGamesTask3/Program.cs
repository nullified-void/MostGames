using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace MostGamesTask3
{
	public class Program
	{
		public static void Main()
		{
			string[] russian = new string[] { "aqwe", "ba", "c" };
			string[] english = new string[] { "a", "|a", "ab" };
			float[] russianRes = new float[russian.Length];
			float[] englishRes = new float[english.Length];
			int[,] finalResults = new int[russian.Length, english.Length];
			Dictionary<string, string[]> resultsDictionary = new Dictionary<string, string[]>();
			for (int i = 0; i < english.Length; i++)
			{
				englishRes[i] = CalculateIndex(english[i]);
			}

			for (int i = 0; i < russian.Length; i++)
			{
				russianRes[i] = CalculateIndex(russian[i]);
				int[] temp = englishRes.Select((s, x) => new { x, s }).Where(t => t.s == russianRes[i]).Select(t => t.x).ToArray();
				for (int j = 0; j < temp.Length; j++)
				{
					finalResults[i, j] = temp[j];
				}

				resultsDictionary.Add(russian[i], MakeArray(english, temp));
			}

			foreach (var pair in resultsDictionary)
			{
				for (int i = 0; i < pair.Value.Length; i++)
				{
				}
			}
			Console.Read();
		}

		private static string[] MakeArray(string[] english, int[] englishIndexes)
		{
			string[] array = new string[englishIndexes.Length];
			for (int i = 0; i < englishIndexes.Length; i++)
			{
				array[i] = english[englishIndexes[i]];
			}

			return array;
		}

		private static float CalculateIndex(string text)
		{
			float index = 0.0f;
			float additiveIndex = 0.5f;
			float commentIndex = 0.0f;
			string data = String.Empty;
			string comment = String.Empty;
			if (text.IndexOf('|') != -1)
			{
				data = text.Substring(0, text.IndexOf('|'));
				comment = text.Substring(text.IndexOf('|') + 1);
			}
			else
			{
				data = text;
			}

			data = RemoveChars(data);
			if (comment != string.Empty)
			{
				comment = RemoveChars(comment);
				for (int i = 0; i < comment.Length; i++)
				{
					commentIndex += additiveIndex;
					additiveIndex += 1f;
				}

				additiveIndex = 0.5f;
				commentIndex = commentIndex * comment.Length;
			}

			for (int i = 0; i < data.Length; i++)
			{
				index += additiveIndex;
				additiveIndex += 1f;
			}

			index = index * data.Length;
			return index + commentIndex;
		}

		private static string RemoveChars(string text)
		{
			string charsToRemove = '[' + Regex.Escape("\"\' @;:.,!?#%№$^&*\\/()-") + ']';
			text = Regex.Replace(text, charsToRemove, "");
			return text;
		}
	}
}
