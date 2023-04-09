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
		bool hard = true;
		public MainWindow()
		{
			InitializeComponent();
			calendar = (Grid)((ScrollViewer)((Grid)((Grid)((Grid)((Grid)Application.Current.MainWindow.Content).Children[1]).Children[0]).Children[1]).Children[0]).Content;

			for(int i = 1; i <= 2; i++) ((Grid)((Grid)((Grid)((Grid)((ScrollViewer)calendar.Parent).Parent).Parent).Parent).Children[i]).Visibility = Visibility.Collapsed;

			SmartCalendar.Pain(new string[0]);

			//drawTask("Dinner", DateTime.Now, 90);
			draw();
        }

		void draw()
		{
			calendar.Children.Clear();
			
			for(int i = 0; i < 24; i++)
			{
				TextBlock t = new TextBlock()
				{
					Text = i.ToString() + ":00",
					FontFamily = new FontFamily("Tahoma"),
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Stretch
				};
				calendar.Children.Add(t);
				Grid.SetRow(t, i+1);
			}
			foreach(HardTask task in SmartCalendar.Schedule)
			{
				drawTask(task);
			}
			for(int x = 0; x < calendar.ColumnDefinitions.Count; x++)
			{
				for(int y = 0; y < calendar.RowDefinitions.Count; y++)
				{
					Rectangle rect = new Rectangle()
					{
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch,
						Stroke = new SolidColorBrush(Color.FromRgb(20, 20, 20))
					};
					calendar.Children.Add(rect);
					Grid.SetRow(rect, y);
					Grid.SetColumn(rect, x);
				}
			}
		}



        private void sideBar_CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have clicked the button");
        }

		void drawTask(HardTask task) 
		{
			Random r = new Random();
			Rectangle rect = new Rectangle()
			{
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Top,
				Height = calendar.RowDefinitions[1].MinHeight * task.length / 60,
				Margin = new Thickness(0, task.time.Minute / 60f * calendar.RowDefinitions[1].MinHeight, 0, 0),
				Fill = new SolidColorBrush(Color.FromRgb((byte)r.Next(), (byte)r.Next(), (byte)r.Next())),
				RadiusX = 5, RadiusY = 5,
				ToolTip = task.name + "\n" + task.description
			};
            calendar.Children.Add(rect);
			Grid.SetColumn(rect, (int)task.time.DayOfWeek + 1);
			Grid.SetRow(rect, task.time.Hour + 1);
			Grid.SetRowSpan(rect, (int)(task.length / 60) + 2);

			if(calendar.RowDefinitions[1].MinHeight * task.length / 60 >= 20)
			{
				TextBlock text = new TextBlock()
				{
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Top,
					Height = calendar.RowDefinitions[1].MinHeight * task.length / 60,
					Margin = new Thickness(0, task.time.Minute / 60f * calendar.RowDefinitions[1].MinHeight, 0, 0),
					Text = task.name,
					FontFamily = new FontFamily("Tahoma"),
					FontSize = 10,
					ToolTip = task.name + "\n" + task.description
				};
				calendar.Children.Add(text);
				Grid.SetColumn(text, (int)task.time.DayOfWeek + 1);
				Grid.SetRow(text, task.time.Hour + 1);
				Grid.SetRowSpan(text, (int)(task.length / 60) + 2);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try{
				Grid newTask = ((Grid)((Button)sender).Parent);
				string name = ((TextBox)newTask.Children[0]).Text;
				string desc = ((TextBox)newTask.Children[1]).Text;
				string startS = ((TextBox)newTask.Children[2]).Text;
				string lengthS = ((TextBox)newTask.Children[3]).Text;
				string dateS = ((TextBox)newTask.Children[4]).Text;

				float length = float.Parse(lengthS.Split(':')[0])*60 + float.Parse(lengthS.Split(':')[1]);

				if(hard)
				{
					DateTime dateTime = new DateTime(int.Parse(dateS.Split('/')[2]), int.Parse(dateS.Split('/')[0]), int.Parse(dateS.Split('/')[1]), int.Parse(startS.Split(':')[0]), int.Parse(startS.Split(':')[1]), 0);

					SmartCalendar.addNewTask(new HardTask(name, desc, length, new List<Task>(), dateTime));
				}
				else
				{
					DateTime dateTime = new DateTime(int.Parse(dateS.Split('/')[2]), int.Parse(dateS.Split('/')[0]), int.Parse(dateS.Split('/')[1]));
					DateTime starting = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, int.Parse(((TextBox)((Grid)newTask.Children[5]).Children[0]).Text.Split(':')[0]), int.Parse(((TextBox)((Grid)newTask.Children[5]).Children[0]).Text.Split(':')[1]), 0);
					DateTime ending = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, int.Parse(((TextBox)((Grid)newTask.Children[5]).Children[1]).Text.Split(':')[0]), int.Parse(((TextBox)((Grid)newTask.Children[5]).Children[1]).Text.Split(':')[1]), 0);
					

					//SmartCalendar.addNewTask(new SoftTask(name, desc, length, new List<Task>(), int.Parse(startS), (int)dateTime.DayOfWeek + 1, starting, ending, false));
				}
				draw();
				newTask.Visibility = Visibility.Collapsed;
			}catch(Exception ex)
			{

			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			((Grid)((Grid)((Grid)((Grid)((ScrollViewer)calendar.Parent).Parent).Parent).Parent).Children[hard?1:2]).Visibility = Visibility.Visible;
		}

		private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
		{
			TextBox t = (TextBox)sender;
			if(float.TryParse(t.Text, out float value))
			{
				if(value < 0) t.Text = "0";
				if(value > 5) t.Text = "5";
				return;
			}
			t.Text = t.Name;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			((Grid)((Button)sender).Parent).Visibility = Visibility.Collapsed;
			hard = !hard;
			((Grid)((Grid)((Grid)((Grid)((ScrollViewer)calendar.Parent).Parent).Parent).Parent).Children[hard ? 1 : 2]).Visibility = Visibility.Visible;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}
}
