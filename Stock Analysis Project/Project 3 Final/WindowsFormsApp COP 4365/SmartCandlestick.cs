using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Represents an enhanced version of a Candlestick with additional properties
    /// and the ability to identify certain candlestick patterns.
    /// </summary>
    internal class SmartCandlestick : Candlestick
    {
        // Properties specific to SmartCandlestick, extending the base Candlestick class.
        public decimal range { get; set; }
        public decimal topPrice { get; set; }
        public decimal bottomPrice { get; set; }
        public decimal bodyRange { get; set; }
        public decimal upperTail { get; set; }
        public decimal lowerTail { get; set; }

        /// <summary>
        /// Stores the truth values of different candlestick patterns for this instance.
        /// </summary>
        public Dictionary<string, bool> Dictionary_Pattern = new Dictionary<string, bool>();

        /// <summary>
        /// Constructor that initializes a SmartCandlestick from a CSV line.
        /// </summary>
        /// <param name="rowOfData">A single line from a CSV file representing candlestick data.</param>
        public SmartCandlestick(string rowOfData) : base(rowOfData)
        {
            // Computes additional properties specific to a SmartCandlestick.
            ComputeCandlestickProperties();
            // Analyzes and records candlestick patterns.
            ComputePatternProperties();
        }

        /// <summary>
        /// Constructor that converts a base Candlestick to a SmartCandlestick.
        /// </summary>
        /// <param name="cs">The Candlestick instance to convert.</param>
        public SmartCandlestick(Candlestick cs)
        {
            // Copying properties from the base Candlestick instance.
            date = cs.date;
            open = cs.open;
            close = cs.close;
            high = cs.high;
            low = cs.low;
            volume = cs.volume;

            // Compute additional properties for the SmartCandlestick.
            ComputeCandlestickProperties();
        }

        /// <summary>
        /// Computes additional properties related to the candlestick's dimensions.
        /// </summary>
        private void ComputeCandlestickProperties()
        {
            // The total price range of the candlestick for the period.
            range = high - low;
            // The highest price between the open and close prices.
            topPrice = Math.Max(open, close);
            // The lowest price between the open and close prices.
            bottomPrice = Math.Min(open, close);
            // The price range between the opening and closing prices.
            bodyRange = topPrice - bottomPrice;
            // The distance from the high price to the top of the body.
            upperTail = high - topPrice;
            // The distance from the low price to the bottom of the body.
            lowerTail = bottomPrice - low;
        }

        /// <summary>
        /// Determines the presence of various candlestick patterns and records their status.
        /// </summary>
        private void ComputePatternProperties()
        {
            // Bullish: Close price is higher than the open price.
            bool bullish = close > open;
            Dictionary_Pattern.Add("Bullish", bullish);

            // Bearish: Open price is higher than the close price.
            bool bearish = open > close;
            Dictionary_Pattern.Add("Bearish", bearish);

            // Neutral: The body range is very small compared to the overall range.
            // Adjusted to consider a smaller body relative to the candlestick's range.
            bool neutral = bodyRange <= (range * 0.03m);
            Dictionary_Pattern.Add("Neutral", neutral);

            // Marubozu: No or very small tails.
            bool marubozu = bodyRange > (range * 0.96m);
            Dictionary_Pattern.Add("Marubozu", marubozu);

            // Hammer: A small body at the upper end with a long lower tail.
            // The body should be in the upper third of the candlestick, and the lower tail should be at least twice the size of the body.
            bool hammer = ((range * 0.20m) < bodyRange) & (bodyRange < (range * 0.33m)) & (lowerTail > range * 0.66m);
            Dictionary_Pattern.Add("Hammer", hammer);

            // Doji: Very small body.
            bool doji = bodyRange < (range * 0.03m);
            Dictionary_Pattern.Add("Doji", doji);

            // Dragonfly doji: The body is at the top, and there's a significant lower tail.
            bool dragonfly_doji = doji & (lowerTail > range * 0.66m);
            Dictionary_Pattern.Add("Dragonfly Doji", dragonfly_doji);

            // Gravestone doji: The body is at the bottom, and there's a significant upper tail.
            bool gravestone_doji = doji & (upperTail > range * 0.66m);
            Dictionary_Pattern.Add("Gravestone Doji", gravestone_doji);
        }
    }
}
