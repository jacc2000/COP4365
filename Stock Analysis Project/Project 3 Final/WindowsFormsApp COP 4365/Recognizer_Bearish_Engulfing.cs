using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizes the bearish engulfing pattern over two candlesticks.
    /// </summary>
    internal class Recognizer_Bearish_Engulfing : Recognizer
    {
        /// <summary>
        /// Constructor initializes the Recognizer with the pattern name "Bearish Engulfing" and length 2.
        /// </summary>
        public Recognizer_Bearish_Engulfing() : base("Bearish Engulfing", 2)
        {
        }

        /// <summary>
        /// Determines if a bearish engulfing pattern exists at a specific index in the list of SmartCandlesticks.
        /// </summary>
        /// <param name="scsList">List of SmartCandlesticks.</param>
        /// <param name="index">Index to check for the pattern.</param>
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
                if (index < 1) // Check if index is valid for a two candlestick pattern.
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false; // Return false if the pattern cannot exist due to index constraints.
                }
                SmartCandlestick prev = scsList[index - 1];
                bool bearish = (prev.open < prev.close) && (scs.close < scs.open); // Check for bearish conditions.
                bool engulfing = (scs.topPrice > prev.topPrice) && (scs.bottomPrice < prev.bottomPrice); // Check if current candlestick engulfs the previous one.
                bool bearish_engulfing = bearish && engulfing; // Combine conditions for the pattern.
                scs.Dictionary_Pattern.Add(Pattern_Name, bearish_engulfing); // Add result to dictionary.
                return bearish_engulfing; // Return the computed pattern result.
            }
        }
    }
}
