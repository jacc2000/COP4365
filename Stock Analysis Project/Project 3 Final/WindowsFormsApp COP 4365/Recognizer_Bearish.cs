using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizes a simple bearish candlestick pattern.
    /// </summary>
    internal class Recognizer_Bearish : Recognizer
    {
        /// <summary>
        /// Constructor initializes the Recognizer with the pattern name "Bearish" and length 1.
        /// </summary>
        public Recognizer_Bearish() : base("Bearish", 1)
        {
        }

        /// <summary>
        /// Determines if a bearish pattern exists at a specific index in the list of SmartCandlesticks.
        /// </summary>
        /// <param name="scsList">List of SmartCandlesticks.</param>
        /// <param name="index">Index to check for bearish pattern.</param>
        /// <returns>True if the pattern is found, otherwise false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value; // Return existing pattern value if already calculated.
            }
            else
            {
                bool bearish = scs.open > scs.close; // Check if open price is greater than close price.
                scs.Dictionary_Pattern.Add(Pattern_Name, bearish); // Add result to dictionary.
                return bearish; // Return the computed pattern result.
            }
        }
    }
}
