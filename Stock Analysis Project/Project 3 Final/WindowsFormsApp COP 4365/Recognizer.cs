using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Serves as an abstract base class for candlestick pattern recognizers.
    /// </summary>
    internal abstract class Recognizer
    {
        // Holds the name of the pattern this recognizer identifies.
        public string Pattern_Name;

        // Holds the number of candlesticks involved in the pattern.
        public int Pattern_Length;

        /// <summary>
        /// Initializes a new instance of the Recognizer class.
        /// </summary>
        /// <param name="pN">The name of the pattern.</param>
        /// <param name="pL">The length of the pattern in terms of number of candlesticks.</param>
        protected Recognizer(string pN, int pL)
        {
            Pattern_Name = pN; // Assign the pattern name.
            Pattern_Length = pL; // Assign the number of candlesticks this pattern spans.
        }

        /// <summary>
        /// Abstract method to recognize the pattern at a specific index in the list of candlesticks.
        /// </summary>
        /// <param name="scsList">List of SmartCandlesticks to analyze.</param>
        /// <param name="index">The index at which to start analysis in the list.</param>
        /// <returns>True if the pattern is recognized at the index, otherwise false.</returns>
        public abstract bool Recognize(List<SmartCandlestick> scsList, int index);

        /// <summary>
        /// Applies the Recognize method to every candlestick in the list.
        /// </summary>
        /// <param name="scsList">List of all SmartCandlesticks to analyze.</param>
        public void Recognize_All(List<SmartCandlestick> scsList)
        {
            for (int i = 0; i < scsList.Count; i++)
            {
                // Apply the Recognize method to each candlestick in the list.
                Recognize(scsList, i);
            }
        }
    }
}
