using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    internal class Candlestick
    {
        // Properties representing the key data points for a candlestick
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal adjClose { get; set; }
        public ulong volume { get; set; }
        public DateTime date { get; set; }

        // Default Candlestick Constructor
        public Candlestick()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Candlestick class using a row of data from a CSV file.
        /// </summary>
        /// <param name="rowOfData">A string containing the comma-separated values for date, open, high, low, close, adjusted close, and volume.</param>
        public Candlestick(string rowOfData)
        {
            // Delimiters used to split the CSV row into individual data points
            char[] seperators = new char[] { ',', ' ', '"' };
            string[] subs = rowOfData.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            // Delimiters used to split the CSV row into individual data points
            string dateString = subs[0];
            date = DateTime.Parse(dateString);

            // Parsing numerical data from the split data. Each 'TryParse' checks if the conversion is successful before assigning the value to the property
            decimal temp;
            bool success = decimal.TryParse(subs[1], out temp);
            if (success) open = temp;

            success = decimal.TryParse(subs[2], out temp);
            if (success) high = temp;

            success = decimal.TryParse(subs[3], out temp);
            if (success) low = temp;

            success = decimal.TryParse(subs[4], out temp);
            if (success) close = temp;

            success = decimal.TryParse(subs[4], out temp);
            if (success) adjClose = temp;

            ulong tempVolume;
            success = ulong.TryParse(subs[6], out tempVolume);
            if (success) volume = tempVolume;
        }
    }
}
