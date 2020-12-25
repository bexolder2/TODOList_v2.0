using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TODOList.Logic;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace TODOList
{
    public partial class MainWindow : Window
    {
        private object locker = new object();

        public MainWindow()
        {
            InitializeComponent();

            this.Closed += MainWindow_Closed;
            GlobalVariables.DrawingTabControl.CreateTabControl(RootGrid, "TabControl1");
            Dialogs.DialogOperations.InitProject += DialogOperations_InitProject;

            Thread TimeChecker = new Thread(CheckTasksTime);
            TimeChecker.IsBackground = true;
            TimeChecker.Start();
            //Dialogs.DialogOperations.NewTask += DialogOperations_NewTask;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            foreach (var obj in Program.Prj)
            {
                Program.Serialize($"{obj.ProjectName}.dat", obj);
            }
        }

        //private void DialogOperations_NewTask()
        //{
        //    if (GlobalVariables.newTask != null)
        //        GlobalVariables.newTask.SaveNewTask += NewTask_SaveNewTask;
        //}

        //private void NewTask_SaveNewTask(object sender, EventArgs e)
        //{
        //    //GlobalVariables.DrawingTabControl.AddTaskItem(Program.Prj.Last().ProjectName); //TODO: current name WTF?? maybe move to NewTask.xaml.cs
        //}

        private void DialogOperations_InitProject()
        {
            if (GlobalVariables.newPr != null)
                GlobalVariables.newPr.SaveNewProject += NewPr_SaveNewProject;
        }

        private void NewPr_SaveNewProject(object sender, EventArgs e)
        {
            GlobalVariables.DrawingTabControl.AddTabItem(Program.Prj.Last().ProjectName, Program.Prj.Last());
            GlobalVariables.BufferPrj = null;
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            Program.AddProject();
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "binary files (*.dat)|*.dat";
            open.RestoreDirectory = true;

            if (open.ShowDialog() == true)
            {
                filePath = open.FileName;
            }

            if (Program.Deserialize(filePath))
            {
                NewPr_SaveNewProject(null, null);
                GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].drawTT.RefreshTreeView();
            }
        }

        public void CheckTasksTime()
        {
            while (true)
            {
                lock (locker)
                {
                    if (GlobalVariables.DrawingTabControl.tabControl.Items.Count != 0)
                    {
                        foreach (var tsk in Program.Prj.Find(x => x.ProjectName ==
                        Dispatcher.Invoke(() => GlobalVariables.DrawingTabControl.GetFocusTabItemHeader())).Root)
                        {
                            if (tsk.Children.Count == 0)
                            {
                                Compare(tsk);
                            }
                            else
                            {
                                Compare(tsk);
                                CheckSubTasksTime(tsk);
                            }
                        }
                    }
                }
                Thread.Sleep(5000); //TODO: set 5-10 minutes
            }
        }

        private void CheckSubTasksTime(Logic.Task child)
        {
            foreach (var tsk_ in child.Children)
            {
                if (tsk_.Children.Count == 0)
                {
                    Compare(tsk_);
                }
                else
                {
                    Compare(tsk_);
                    CheckSubTasksTime(tsk_);
                }
            }
        }

        private void Compare(Logic.Task task)
        {
            if ((task.Finish - DateTime.Now).Days == 1)
            {
                MessageBox.Show($"Deadline {task.TaskName} coming to an end.");
            }

            if (task.Finish < DateTime.Now)
            {
                MessageBox.Show($"{task.TaskName} - Red");
                int year = GlobalVariables.DrawingTabControl.tabItems[Dispatcher.Invoke(() => GlobalVariables.DrawingTabControl.GetTabItemIndex())].nv.CurrentYear;
                int month = GlobalVariables.DrawingTabControl.tabItems[Dispatcher.Invoke(() => GlobalVariables.DrawingTabControl.GetTabItemIndex())].nv.CurrentPage;
                if (GlobalVariables.DrawingTabControl.tabItems[Dispatcher.Invoke(() => GlobalVariables.DrawingTabControl.GetTabItemIndex())].nv.pages_.Count > 0)
                {
                    GlobalVariables.DrawingTabControl.tabItems[Dispatcher.Invoke(() => GlobalVariables.DrawingTabControl.GetTabItemIndex())].
                        nv.pages_[year][month].canvas.SearchRectForFill(StatusColor.Red, task.TaskName);
                }
            }
        }
    }
}
