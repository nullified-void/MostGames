using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MostGames
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary> Contains all Vowels known to me. Used for counting vowels in texts.</summary>
        readonly HashSet<char> Vowels = new HashSet<char> { 'e', 'u', 'i', 'o', 'a', 'æ', 'ø', 'y', 'а', 'о', 'у', 'э', 'ы', 'я', 'ё', 'е', 'ю', 'и' };
        /// <summary> Regex for removing Diacritics. </summary>
        private readonly static Regex nonSpacingMarkRegex = new Regex(@"\p{Mn}", RegexOptions.Compiled);
        /// <summary> Contains all earlier created components. Can contain TextBlock's or Label's, used for tidying up when "Calculate" button is pressed. </summary>
        List<dynamic> Createdcomponents = new List<dynamic>();
        /// <summary> Contains all earlier created rows. used for tidying up when "Calculate" button is pressed. </summary>
        List<RowDefinition> CreatedRows = new List<RowDefinition>();

        List<int> incorrectIndexes = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// Called upon "Calculate" button click. Resets row count to its initial number (3 at the moment of writing this), removes all previosly created components. Parses text from textbox(removes all whitespaces, unnesessary zeroes,
        /// splits it(accepted separators: ';', ',') makes GET request for every distinct index, and forms a new table from this content.(creates a row for every valid index, fills row with 1 TextBlock and 2 Labels)).
        /// </summary>
        public void buttonClick(object sender, RoutedEventArgs e)
        {
            if (textbox.Text == String.Empty)
            {
                return;
            }

            ResetControls();

            string[] ids = MakeArray();
            foreach (string s in ids)
            {
                if (int.Parse(s) > 0 && int.Parse(s) <= 20)
                {
                    GenerateRows(s);
                }
                else
                {
                    incorrectIndexes.Add(int.Parse(s));
                }
            }
            //<Label Content="incorrect indexes: 1" Grid.Row="0" Grid.Column="1" Margin="200,30,4,40"/>
            if (incorrectIndexes.Count > 0)
            {
                Label incorrectIndexesLabel = new Label();
                incorrectIndexesLabel.Content = "incorrect indexes:";
                foreach (int i in incorrectIndexes)
                {
                    incorrectIndexesLabel.Content += " " + i;
                }
                Grid.SetRow(incorrectIndexesLabel, 0);
                Grid.SetColumn(incorrectIndexesLabel, 1);
                incorrectIndexesLabel.Foreground = Brushes.Red;
                Grid.Children.Add(incorrectIndexesLabel);
                incorrectIndexes.Clear();
                Createdcomponents.Add(incorrectIndexesLabel);
            }
           


        }
        /// <summary>
        /// Called from "Calculate" button click. creates a row, fitting 1 TextBlock and 2 Lables inside of it. TextBlock contains text from GET request, Labels contain vowel and word count from this text.
        /// </summary>
        /// <param name="s">ID of text for the GET request</param>
        public void GenerateRows(string s)
        {
            //GET request with specified ID
            string text = Get(ConfigurationManager.AppSettings["adress"] + s);
            text = text.Substring(9, text.Length - 11);

            //row creation
            var rowDefinition = new RowDefinition();
            Grid.RowDefinitions.Add(rowDefinition);
            Grid.SetRowSpan(borderTest, Grid.GetRowSpan(borderTest) + 1);
            CreatedRows.Add(rowDefinition);

            //TextBlock creation with text from GET request
            TextBlock labelText = new TextBlock();
            labelText.Text = text;
            Grid.SetRow(labelText, Grid.RowDefinitions.Count - 2);
            Grid.SetColumn(labelText, 0);
            labelText.TextWrapping = TextWrapping.Wrap;
            labelText.Margin = new Thickness(15, 5, 0, 0);
            Grid.Children.Add(labelText);
            Createdcomponents.Add(labelText);

            //Label creation with word count IMPORTANT : WORD COUNT IS JUST SPLITTING STRING BY ' '(WHITE SPACE) SEPARATOR, SO MAKE SURE WORDS ARE SEPARATED CORRECTLY!
            Label labelWords = new Label();
            labelWords.Content = text.Split(' ').Length.ToString();
            Grid.SetRow(labelWords, Grid.RowDefinitions.Count - 2);
            Grid.SetColumn(labelWords, 1);
            labelWords.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.Children.Add(labelWords);
            Createdcomponents.Add(labelWords);

            //Label creation with vowel count.
            Label labelVowels = new Label();
            labelVowels.Content = CountVowels(text);
            Grid.SetRow(labelVowels, Grid.RowDefinitions.Count - 2);
            Grid.SetColumn(labelVowels, 2);
            labelVowels.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.Children.Add(labelVowels);
            Createdcomponents.Add(labelVowels);
        }

        /// <summary>
        /// Removes all previosly created rows and controls.
        /// </summary>
        public void ResetControls()
        {
            Grid.Children.RemoveRange(Grid.Children.Count - Createdcomponents.Count, Createdcomponents.Count);
            Createdcomponents.Clear();
            foreach (var row in CreatedRows)
            {
                Grid.RowDefinitions.Remove(row);
            }
            CreatedRows.Clear();
        }
        /// <summary>
        /// Takes text from textbox, removes all whitespaces and unnesessary zero's from it and splits it by 1 of two valid separators(';' or ',').
        /// </summary>
        /// <returns>Splitted array of distinct indexes</returns>
        public string[] MakeArray()
        {
            string idsString = Regex.Replace(textbox.Text, @"\s+", "");
            string[] ids;
            if (textbox.Text.Contains(';'))
            {
                ids = idsString.Split(';');
            }
            else
            {
                ids = idsString.Split(',');
            }
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = ids[i].TrimStart('0');
            }
            return ids.Distinct().ToArray();
        }

        /// <summary>
        /// Makes GET request to a specified URI
        /// </summary>
        /// <param name="uri">URI for GET request</param>
        /// <returns>response from GET request, or exception message</returns>
        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Add(ConfigurationManager.AppSettings["header"], ConfigurationManager.AppSettings["headerValue"]);
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine();
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                return String.Format("         Exception has occured! {0}  ",e.Message);
            }
            
        }
        /// <summary>
        /// Counts vowels in specified string
        /// </summary>
        /// <param name="s">string to count vowels in</param>
        /// <returns>Count of vowels</returns>
        public int CountVowels(string s)
        {
            s = RemoveDiacritics(s);
            return s.Count(c => Vowels.Contains(Char.ToLower(c)));
        }
        /// <summary>
        /// removes all diacritics from specified string using Regex
        /// </summary>
        /// <param name="text">string from which to remove</param>
        /// <returns>string with removes diacritics</returns>
        private static string RemoveDiacritics(string text)
        {
            if (text == null)
                return string.Empty;

            var normalizedText = text.Normalize(NormalizationForm.FormD);

            return nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }
    }
}
