using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TODOList.Logic;

namespace TODOList.Drawing.Navigation
{
    public class Frames
    {
        public Frame frame { get; private set; }
        public List<Logic.Task> TasksWithoutTree { get; private set; }
        public int NumberOfYears { get; private set; }

        public Frames()
        {
            frame = new Frame();
            TasksWithoutTree = new List<Logic.Task>();
        }

        public void CreateFrame(Grid grid)
        {
            frame.Background = Brushes.Azure;
            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            Thickness margin = frame.Margin;
            margin.Left = 200;
            margin.Top = 10;
            margin.Right = 10;
            margin.Bottom = 40;
            frame.Margin = margin;

            grid.Children.Add(frame);
        }

        #region convert
        public void RefreshTaskList()
        {
            TasksWithoutTree = null;
            TasksWithoutTree = new List<Logic.Task>();
        }

        public void ConvertToList(Project project)
        {
            foreach (var child in project.Root)
            {
                if (child.Children == null)
                {
                    TasksWithoutTree.Add(child);
                }
                else
                {
                    TasksWithoutTree.Add(child);
                    ConvertToListChild(child);
                }
            }
        }

        private void ConvertToListChild(Logic.Task child)
        {
            foreach (var child_ in child.Children)
            {
                if (child_.Children == null)
                {
                    TasksWithoutTree.Add(child_);
                }
                else
                {
                    TasksWithoutTree.Add(child_);
                    ConvertToListChild(child_);
                }
            }
        }
        #endregion

        public void RefreshYears()
        {
            NumberOfYears = 0;
        }

        public void CheckYears()
        {
            List<int> tmpYear = new List<int>();
            foreach(var task in TasksWithoutTree)
            {
                //if (tmpYear.Find(x => x == task.Finish.Year) == 0)
                //{
                //    if(TasksWithoutTree.Count == 1)
                //    {
                //        NumberOfYears = task.Finish.Year - DateTime.Now.Year;
                //        //NumberOfYears = task.Finish.Year - task.Start.Year;//-1
                //    }
                //    else
                //    {
                //        ++NumberOfYears;
                //        tmpYear.Add(task.Finish.Year);
                //    }      
                //} 
                tmpYear.Add(task.Finish.Year - DateTime.Now.Year);
            }
            NumberOfYears = tmpYear.Max();
        }
    }
}
