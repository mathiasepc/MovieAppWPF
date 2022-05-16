using System;

namespace MovieApp
{
    public partial class Validation : MainWindow
    {
        #region methods
        /// <summary>
        /// laver en checker på textbox rating om den er længde 2 eller mindre.
        /// </summary>
        /// <param name="textboxRating"></param>
        /// <returns></returns>
        public static bool CheckRatingTextBox(string ratingText)
        {
            //laver min string om til en int
            bool succes = Int32.TryParse(ratingText, out int rating);

            //laver if statement for at den skal parses og rating er <= 10
            if (succes && rating <= 10)
                return true;
            else
                return false;
        }
        /// <summary>
        /// laver en checker på textbox Year om den er længde 4.
        /// </summary>
        /// <param name="textboxRating"></param>
        /// <returns></returns>
        public static bool CheckYearTextBox(string yearText)
        {
            //laver min string om til en int
            bool succes = Int32.TryParse(yearText, out int year);

            //laver if statement for at der er tastet 4
            if (succes && year <= 2022 && year >= 1878)
                return true;
            else
                return false;
        }
        /// <summary>
        /// laver en metode som checker at der er tekst i feltet titel
        /// </summary>
        /// <param name="titleText"></param>
        /// <returns></returns>
        public static bool CheackTitleTextBox(string titleText)
        {
            if (!string.IsNullOrEmpty(titleText))
                return true;
            else
                return true;
        }
        #endregion
    }
}
