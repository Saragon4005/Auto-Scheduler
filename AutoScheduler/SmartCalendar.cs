using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScheduler
{
    public static class SmartCalendar
    {

        public static List<Task> TaskList = new List<Task>();
        public static List<HardTask> Schedule;

        public static void Pain(string[] args)
        {
            //JJACKSON HAS ADDED A COMMENTO WOOOOO
            Console.WriteLine("Starting");
            HardTask Badminton = new HardTask("Badminton", "Going to Badminton", 40, null, new DateTime(2023, 4, 9, 14, 30, 0));
            HardTask Hockey = new HardTask("Hockey", "Going to Hockey", 60, null, new DateTime(2023, 4, 9, 15, 30, 0));
            HardTask Meeting = new HardTask("Meeting", "Talk with Mike", 60, null, new DateTime(2023, 4, 9, 17, 30, 0));
            SoftTask Laundry = new SoftTask("Laundry", "One Load of Laundry", 35, null, 3, 1, new DateTime(2023, 4, 9, 14, 0, 0), new DateTime(2023, 4, 24), false);
            TaskList.Add(Badminton);
            TaskList.Add(Laundry);
            TaskList.Add(Hockey);
            TaskList.Add(Meeting);
            Schedule = generateList(TaskList);
            printSchedule(Schedule);
            Console.ReadLine();
        }

        public static void addNewTask(Task task)
        {
            TaskList.Add(task);
			Schedule.Clear();
			Schedule = generateList(TaskList);
		}
        private static void printSchedule(List<HardTask> Schedule)
        {
            foreach (HardTask h in Schedule)
            {
                Console.WriteLine(h.name + " at " + h.time + " for " + h.length + " minutes.");
            }
        }

        public static List<HardTask> generateList(List<Task> taskList)
        {
            List<HardTask> HardTaskList = new List<HardTask>();
            List<SoftTask> SoftTaskList = new List<SoftTask>();
            foreach (Task t in taskList)
            {
                if (t is HardTask)
                {
                    Console.WriteLine("Adding " + t.name + " to it's given time slot.");
                    HardTaskList.Add((HardTask)t);

                }
                else
                { //SoftTask
                    SoftTaskList.Add((SoftTask)t);
                }
            }
            HardTaskList.Sort((x, y) => DateTime.Compare(x.time, y.time));
            SoftTaskList = PrioSort(SoftTaskList);
            foreach (SoftTask t in SoftTaskList)
            {
                //ADD TO THE NEXT AVAILABLE SLOT TODO
                bool finding = true;
                int i = 0;
                while (finding)
                {
                    if (i == 0)
                    {
                        if (t.startTimePref < DateTime.Now)
                        {
                            t.startTimePref = DateTime.Now;
                        }
                        if (HardTaskList[0].time >= t.startTimePref.AddMinutes(t.length))
                        {
                            Console.WriteLine(t.startTimePref + " is the next available time");
                            HardTaskList.Add(new HardTask(t.name, t.description, t.length, t.children, t.startTimePref));
                            finding = false;
                            break;
                        }
                    }
                    if (i + 1 >= HardTaskList.Count)
                    {
                        Console.WriteLine(HardTaskList.Count);
                        break;
                    }
                    HardTask lookingAt = HardTaskList[i];
                    TimeSpan span = HardTaskList[i + 1].time - lookingAt.time.AddMinutes(lookingAt.length);
                    Console.WriteLine("--" + span.TotalMinutes + " long gap");
                    if (span.TotalMinutes >= t.length)
                    {
                        Console.WriteLine(span.TotalMinutes + " must be greater than the activity length of " + t.length);
                        Console.WriteLine(lookingAt.time.AddMinutes(lookingAt.length) + " is the next available time");
                        HardTaskList.Add(new HardTask(t.name, t.description, t.length, t.children, lookingAt.time.AddMinutes(lookingAt.length)));
                        finding = false;
                    }
                    i++;
                }
            }
            HardTaskList.Sort((x, y) => DateTime.Compare(x.time, y.time));
            return HardTaskList;
        }

        public static List<SoftTask> PrioSort(List<SoftTask> s)
        {
            s.Sort((x, y) => x.priority.CompareTo(y.priority));
            return s;
        }
    }
    public class Task
    {

        public string name;
        public string description;
        public float length;
        public List<Task> children;

        public Task(string name, string description, float length, List<Task> children)
        {
            this.name = name;
            this.description = description;
            this.length = length;
            this.children = children;

        }


        public override string ToString()
        {
            string output = "Task";
			output += name + "👍";
			output += description + "👍";
			output += length + "🤞";
            return output;
		}
    }
    public class HardTask : Task
    {
        public DateTime time;

        public HardTask(string name, string description, float length, List<Task> children, DateTime time) : base(name, description, length, children)
        {
            this.time = time;
        }
    }
    public class SoftTask : Task
    {
        public int priority;
        public int day;
        public DateTime startTimePref;
        public DateTime endTimePref;
        public bool recurring;

        public SoftTask(string name, string description, float length, List<Task> children, int priority, int day, DateTime startTimePref, DateTime endTimePref, bool recurring) : base(name, description, length, children)
        {

            this.priority = priority;
            this.day = day;
            this.startTimePref = startTimePref;
            this.endTimePref = endTimePref;
            this.recurring = recurring;
        }
    }
}
