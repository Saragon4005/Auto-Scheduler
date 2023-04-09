using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


namespace AutoScheduler
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Grid calendar;
		public MainWindow()
		{
			InitializeComponent();
			calendar = (Grid)((ScrollViewer)((Grid)((Grid)((Grid)((Grid)Application.Current.MainWindow.Content).Children[1]).Children[0]).Children[1]).Children[0]).Content;

			for(int i = 0; i < 24; i++)
			{
				TextBlock t = new TextBlock() { Text = i.ToString() + ":00" , FontFamily = new FontFamily("Tahoma") ,
												HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch};
                calendar.Children.Add(t);
				Grid.SetRow(t, i+1);
			}

			SmartCalendar.Pain(new string[0]);

			foreach(HardTask task in SmartCalendar.Schedule)
			{
				drawTask(task.name, task.time, task.length);
			}
			//drawTask("Dinner", DateTime.Now, 90);
			
        }

        private void sideBar_CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have clicked the button");
        }

		void drawTask(string name, DateTime start, float length) 
		{
			Random r = new Random();
			Rectangle rect = new Rectangle()
			{
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Top,
				Height = calendar.RowDefinitions[1].MinHeight * length / 60,
				Margin = new Thickness(0, start.Minute / 60f * calendar.RowDefinitions[1].MinHeight, 0, 0),
				Fill = new SolidColorBrush(Color.FromRgb((byte)r.Next(), (byte)r.Next(), (byte)r.Next()))
			};
            calendar.Children.Add(rect);
			Grid.SetColumn(rect, (int)start.DayOfWeek + 1);
			Grid.SetRow(rect, start.Hour + 1);
			Grid.SetRowSpan(rect, (int)(length / 60) + 2);
		}
    }
}
