using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp_COP_4365
{
    public partial class Form_StockViewer : Form
    {
        // Stores all candlestick data read from the selected file
        private List<SmartCandlestick> candlesticks = null;
        // Used for data binding with a DataGridView to display candlestick data 
        private BindingList<SmartCandlestick> boundCandlesticks = null;
        // Stores the starting date for filtering candlestick data
        private DateTime startDate = new DateTime(2022, 1, 1);
        // Stores the ending date for filtering candlestick data
        private DateTime endDate = DateTime.Now;
        // Stores instances of all pattern recognizers
        private Dictionary<string, Recognizer> Dictionary_Recognizer;
        // Holds the maximum chart value for scaling annotations
        private double chartMax;
        // Holds the minimum chart value for scaling annotations
        private double chartMin;  

        /// <summary>
        /// Initializes the main form for the application
        /// Sets up the initial list of candlesticks with a predefined capacity to optimize memory usage
        /// </summary>
        public Form_StockViewer()
        {
            // Initializes the components defined in the designer file
            InitializeComponent();
            // Initializes the dictionary of recognizer classes.
            InitializeRecognizer();
            // Preallocates space for up to 1024 candlestick objects
            candlesticks = new List<SmartCandlestick>(1024);
            // Date time picker is pre-set to specified start and end dates
            dateTimePicker_startDate.Value = startDate;
            dateTimePicker_endDate.Value = endDate;
        }

        /// <summary>
        /// Constructor for child forms to display additional stocks.
        /// </summary>
        /// <param name="stockPath">"File path of the child form (2 - n file selection)"</param>
        /// <param name="start">Start date inherited from the parent form.</param>
        /// <param name="end">End date inherited from the parent form.</param>
        public Form_StockViewer(string stockPath, DateTime start, DateTime end)
        {
            InitializeComponent();  // Initializes the form components from the designer file.
            InitializeRecognizer();  // Initializes the dictionary of recognizer classes.
            dateTimePicker_startDate.Value = startDate = start; // Set inherited start date.
            dateTimePicker_endDate.Value = endDate = end; // Set inherited end date.
            candlesticks = goReadFile(stockPath); // Read stock data from the file.
            filterList(); // Apply date filtering to the stock data.
            displayCandlesticks(); // Display the filtered stock data on the chart.
        }

        /// <summary>
        /// Handler for the "Open File" button click event. Opens the file dialog to select stock files.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void button_openFile_Click(object sender, EventArgs e)
        {
            Text = "Opening File..."; // Update form title to indicate file opening.
            openFileDialog_stockPick.ShowDialog(); // Open the file dialog.
        }

        /// <summary>
        /// Handler for the "Update" button click event. Updates the chart display based on the selected dates.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void button_Update_Click(object sender, EventArgs e)
        {
            if ((candlesticks.Count != 0) && (startDate <= endDate))
            {
                filterList(); // Filter the candlestick data based on the selected dates.
                displayCandlesticks(); // Update the chart display.
            }
        }

        /// <summary>
        /// Handler for the file dialog's FileOk event. Reads the selected stock files and updates the chart.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void openFileDialog_stockPick_FileOk(object sender, CancelEventArgs e)
        {
            int numberOfFiles = openFileDialog_stockPick.FileNames.Count(); // Count the selected files.
            for (int i = 0; i < numberOfFiles; ++i)
            {
                string pathName = openFileDialog_stockPick.FileNames[i]; // Path of the current file.
                string ticker = Path.GetFileNameWithoutExtension(pathName); // Ticker symbol derived from file name.

                Form_StockViewer form_StockViewer; // Declare a form variable for stock viewing.
                if (i == 0) // For the first file, use the existing form.
                {
                    form_StockViewer = this; // Use the current form for the first file.
                    readAndDisplayStock(); // Read and display the stock data in the current form.
                    form_StockViewer.Text = "Parent: " + ticker; // Update the form's title.
                }
                else // For subsequent files, create new child forms.
                {
                    // Instantiate new form using the parameterized constructor to handle child stock files
                    form_StockViewer = new Form_StockViewer(pathName, startDate, endDate);
                    // Update the form's title to indicate it's a child and show the ticker symbol
                    form_StockViewer.Text = "Child: " + ticker;
                }

                // Show the new form, either the updated current form or a new child form
                form_StockViewer.Show();
                // Bring the newly displayed form to the front for visibility
                form_StockViewer.BringToFront();
            }
        }

        /// <summary>
        /// Reads candlestick data from a specified CSV file and stores it in a list
        /// Also sets the start and end dates based on the data
        /// </summary>
        /// <param name="filename">The path to the CSV file to be read</param>
        /// <returns>A list of Candlestick objects populated with data from the file</returns>
        private List<SmartCandlestick> goReadFile(string filename)
        {
            // Display the name of the opened file
            this.Text = Path.GetFileName(filename);
            // Expected header format in the CSV file
            const string referenceString = "Date,Open,High,Low,Close,Adj Close,Volume";

            // Initializes a new list to store the candlestick data
            List<SmartCandlestick> list = new List<SmartCandlestick>();
            // Pass file path and filename to StreamReader constructor
            using (StreamReader sr = new StreamReader(filename))
            {
                // Reads the first line
                string line = sr.ReadLine();
                // Verifies the header format
                if (line == referenceString)
                {
                    // Reads each line until the end of the file
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Parses the line into a Candlestick object
                        SmartCandlestick cs = new SmartCandlestick(line);
                        // Adds the Candlestick object to the list
                        list.Add(cs);
                    }
                }

                else
                { Text = "Bad File: " + Path.GetFileName(filename); } // Updates the form's title to indicate a file format error
            }

            //Run all Recognizers on list
            foreach (Recognizer r in Dictionary_Recognizer.Values)
            {
                //Adds dictionary entries for every pattern on every candlestick
                r.Recognize_All(list);
            }
            // Returns the populated list of candlesticks
            return list; 
        }

        /// <summary>
        /// Overload of goReadFile
        /// </summary>
        private void goReadFile()
        {
            // Read data from file into candlesticks list
            candlesticks = goReadFile(openFileDialog_stockPick.FileName);
            // Bind list to binding list
            boundCandlesticks = new BindingList<SmartCandlestick>(candlesticks);
        }

        /// <summary>
        /// Filters candlestick data based on specified start and end dates.
        /// </summary>
        /// <param name="list">The list of candlesticks to be filtered.</param>
        /// <param name="start">The start date for filtering.</param>
        /// <param name="end">The end date for filtering.</param>
        /// <returns>A filtered list of candlesticks within the specified date range.</returns>
        private List<SmartCandlestick> filterList(List<SmartCandlestick> list, DateTime start, DateTime end)
        {
            List<SmartCandlestick> filter = new List<SmartCandlestick>(list.Count); // Initialize a list for filtered data.
            foreach (SmartCandlestick cs in list) // Iterate through each candlestick in the original list.
            {
                if ((cs.date >= start) & (cs.date <= end)) // Check if the candlestick date falls within the selected range.
                {
                    filter.Add(cs); // Add the candlestick to the filtered list if it matches the criteria.
                }
            }
            return filter; // Return the filtered list.
        }

        /// <summary>
        /// Filters the candlestick data using the class-level list and updates the bound list for display.
        /// </summary>
        private void filterList()
        {
            // Filter the candlestick data using the class-level list and date range
            List<SmartCandlestick> filteredCandlesticks = filterList(candlesticks, startDate, endDate);
            // Update the binding list with the filtered data
            boundCandlesticks = new BindingList<SmartCandlestick>(filteredCandlesticks);
        }

        /// <summary>
        /// Displays the candlestick data on the chart after normalization.
        /// </summary>
        /// <param name="bindList">The BindingList of Candlestick objects to be displayed on the chart.</param>
        private void displayCandlesticks(BindingList<SmartCandlestick> bindList)
        {
            normalizeChart(bindList); // Normalize the chart's Y-axis based on the candlestick data.
            chart_OHLCV.Annotations.Clear(); // Clear any existing annotations on the chart.
            chart_OHLCV.DataSource = bindList; // Bind the candlestick data to the chart.
            chart_OHLCV.DataBind(); // Bind the data to the chart elements, refreshing the display.
        }

        /// <summary>
        /// Overload of displayCandlesticks without arguments, using the bound candlesticks list.
        /// </summary>
        private void displayCandlesticks()
        {
            // Display the candlesticks using the bound list
            displayCandlesticks(boundCandlesticks);
        }

        /// <summary>
        /// Normalizes the chart by finding the lowest and highest value in total data
        /// Sets the Y axis to 2% more and less than the highest and lowest value respectively
        /// </summary>
        /// <param name="bindList">"Binding list containing the data that needs to be displayed"</param>
        private void normalizeChart(BindingList<SmartCandlestick> bindList)
        {
            // Starting conditions for min and max
            decimal min = 1000000000, max = 0;
            // Iterates through each candlestick
            foreach (SmartCandlestick c in bindList)
            {
                // Finds the minimum low value in the list
                if (c.low < min) { min = c.low; }
                // Finds the maximum high value in the list
                if (c.high > max) { max = c.high; }
            }
            // Adjusts the Y-axis minimum and maximum to include a 2% margin around the data
            chartMin = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = Math.Floor(Decimal.ToDouble(min) * 0.98);
            chartMax = chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = Math.Ceiling(Decimal.ToDouble(max) * 1.02);

            // Format the primary Y-axis (prices in the candlestick chart) as a number with two decimal places
            chart_OHLCV.ChartAreas["ChartArea_OHLC"].AxisY.LabelStyle.Format = "N2";
        }

        /// <summary>
        /// Overload of normalizeChart
        /// </summary>
        private void normalizeChart()
        {
            // Find the min and max low and high values from candlesticks
            normalizeChart(boundCandlesticks);
        }

        /// <summary>
        /// Reads stock data from the selected file, filters it, and then displays it on the chart.
        /// </summary>
        private void readAndDisplayStock()
        {
            goReadFile();  // Read stock data from the selected file.
            filterList();  // Filter the stock data based on the selected date range.
            displayCandlesticks();  // Display the filtered stock data on the chart.
        }

        /// <summary>
        /// Initializes recognizers for various candlestick patterns and populates a dictionary to manage them.
        /// Also populates a ComboBox with these pattern names for user selection.
        /// </summary>
        private void InitializeRecognizer()
        {
            // Create a new dictionary to hold the pattern recognizers keyed by their names.
            Dictionary_Recognizer = new Dictionary<string, Recognizer>();

            // Instantiate a bullish pattern recognizer and add it to the dictionary.
            Recognizer r = new Recognizer_Bullish();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a bearish pattern recognizer and add it to the dictionary.
            r = new Recognizer_Bearish();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a neutral pattern recognizer and add it to the dictionary.
            r = new Recognizer_Neutral();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Marubozu pattern recognizer and add it to the dictionary.
            r = new Recognizer_Marubozu();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Hammer pattern recognizer and add it to the dictionary.
            r = new Recognizer_Hammer();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Doji pattern recognizer and add it to the dictionary.
            r = new Recognizer_Doji();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Dragonfly Doji pattern recognizer and add it to the dictionary.
            r = new Recognizer_Dragonfly_Doji();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Gravestone Doji pattern recognizer and add it to the dictionary.
            r = new Recognizer_Gravestone_Doji();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Bullish Engulfing pattern recognizer and add it to the dictionary.
            r = new Recognizer_Bullish_Engulfing();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Bearish Engulfing pattern recognizer and add it to the dictionary.
            r = new Recognizer_Bearish_Engulfing();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Bullish Harami pattern recognizer and add it to the dictionary.
            r = new Recognizer_Bullish_Harami();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Bearish Harami pattern recognizer and add it to the dictionary.
            r = new Recognizer_Bearish_Harami();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Peak pattern recognizer and add it to the dictionary.
            r = new Recognizer_Peak();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Instantiate a Valley pattern recognizer and add it to the dictionary.
            r = new Recognizer_Valley();
            Dictionary_Recognizer.Add(r.Pattern_Name, r);

            // Populate the ComboBox with the names of all recognized patterns for user interaction.
            comboBox_Patterns.Items.AddRange(Dictionary_Recognizer.Keys.ToArray());
        }


        /// <summary>
        /// Event handler for changing the start date in the DateTimePicker.
        /// </summary>
        /// <param name="sender">The sender object (DateTimePicker).</param>
        /// <param name="e">Event arguments.</param>
        private void dateTimePicker_startDate_ValueChanged(object sender, EventArgs e)
        {
            startDate = dateTimePicker_startDate.Value;  // Update the start date based on the picker's value.
        }

        /// <summary>
        /// Event handler for changing the end date in the DateTimePicker.
        /// </summary>
        /// <param name="sender">The sender object (DateTimePicker).</param>
        /// <param name="e">Event arguments.</param>
        private void dateTimePicker_endDate_ValueChanged(object sender, EventArgs e)
        {
            endDate = dateTimePicker_endDate.Value;  // Update the end date based on the picker's value.
        }

        /// <summary>
        /// Handles changes in the selected item in the combo box, updating annotations on the chart accordingly.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event data.</param>
        private void comboBox_Patterns_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear any existing annotations on the chart whenever the selected pattern changes.
            chart_OHLCV.Annotations.Clear();

            // Ensure there are candlesticks bound to the chart before proceeding.
            if (boundCandlesticks != null)
            {
                // Loop through each candlestick currently displayed on the chart.
                for (int i = 0; i < boundCandlesticks.Count; i++)
                {
                    // Access the SmartCandlestick at the current index for pattern checking.
                    SmartCandlestick scs = boundCandlesticks[i];
                    // Retrieve the corresponding chart data point for the current candlestick.
                    DataPoint point = chart_OHLCV.Series[0].Points[i];

                    // Get the currently selected pattern name from the combo box.
                    string selected = comboBox_Patterns.SelectedItem.ToString();

                    // Check if the current candlestick has the selected pattern marked as true.
                    if (scs.Dictionary_Pattern[selected])
                    {
                        // Get the pattern length defined in the recognizer to determine annotation type.
                        int length = Dictionary_Recognizer[selected].Pattern_Length;

                        // Handle multi-candlestick patterns differently from single candlestick patterns.
                        if (length > 1)
                        {
                            // Skip annotation at boundaries where multi-candlestick patterns cannot be formed.
                            if (i == 0 || ((i == boundCandlesticks.Count() - 1) && length == 3))
                            {
                                continue;
                            }
                            // Initialize a rectangle annotation to visually group candlesticks involved in the pattern.
                            RectangleAnnotation rectangle = new RectangleAnnotation();
                            rectangle.SetAnchor(point);

                            double Ymax, Ymin;
                            // Calculate the width of the rectangle based on the number of candlesticks involved in the pattern.
                            double width = (100.0 / boundCandlesticks.Count()) * length;

                            // Calculate the highest and lowest prices involved in the pattern to set the rectangle dimensions.
                            if (length == 2)  // Handle two-candlestick patterns.
                            {
                                Ymax = (int)(Math.Max(scs.high, boundCandlesticks[i - 1].high));
                                Ymin = (int)(Math.Min(scs.low, boundCandlesticks[i - 1].low));
                                // Offset the rectangle's anchor for even-length patterns.
                                rectangle.AnchorOffsetX = ((width / length) / 2 - 0.25) * (-1);
                            }
                            else  // Handle three-candlestick patterns.
                            {
                                Ymax = (int)(Math.Max(scs.high, Math.Max(boundCandlesticks[i + 1].high, boundCandlesticks[i - 1].high)));
                                Ymin = (int)(Math.Min(scs.low, Math.Min(boundCandlesticks[i + 1].low, boundCandlesticks[i - 1].low)));
                            }

                            // Set the rectangle's dimensions based on the calculated height and width.
                            double height = 50.0 * (Ymax - Ymin) / (chartMax - chartMin);
                            rectangle.Y = Ymax;  // Set Y to highest Y value for candlesticks
                            rectangle.Height = height;
                            rectangle.Width = width;
                            rectangle.LineDashStyle = ChartDashStyle.Dot;  // Set line style to dot
                            rectangle.BackColor = Color.Transparent;  // Set background color to transparent
                            rectangle.LineWidth = 2;
                            

                            // Add the rectangle annotation to the chart to highlight the pattern.
                            chart_OHLCV.Annotations.Add(rectangle);
                        }


                        ArrowAnnotation arrow = new ArrowAnnotation  // Create a new arrow annotation.
                        {
                            ArrowSize = 4,  // Set the arrow size.
                            ArrowStyle = ArrowStyle.Simple,  // Choose a simple arrow style.
                            LineColor = Color.Black, // Set the line color of the arrow to black
                            BackColor = Color.Yellow, // Set the background color of the arrow for contrast.
                            LineWidth = 1, // Set the line width of the arrow.
                            AnchorDataPoint = point, // Anchor the arrow to the corresponding data point on the chart.
                            Height = -4, // Set the height of the arrow to ensure it's visible above the candlestick.
                            Width = 0, // Set the width of the arrow. A value of 0 keeps it slim.
                            AnchorOffsetY = -3 // Offset the arrow on the Y-axis to avoid overlapping with the candlestick.
                        };

                        chart_OHLCV.Annotations.Add(arrow); // Add the configured arrow annotation to the chart.
                    }
                }
            }
        }

        private void Form_Project_2_Load(object sender, EventArgs e)
        {
            // This method is called when the form loads. You can initialize form components or variables here if needed.
        }
    }
}