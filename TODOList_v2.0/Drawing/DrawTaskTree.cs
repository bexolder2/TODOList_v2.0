using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TODOList.Logic;

namespace TODOList.Drawing
{
    public class DrawTaskTree
    {
        private TreeView treeView;
        private DrawContextMenu drawCM;
        public Logic.Task SelectedTask { get; private set; }

        public DrawTaskTree(Grid grid, Project project)
        {
            treeView = new TreeView();
            HierarchicalDataTemplate hdt = new HierarchicalDataTemplate();
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("TaskName"));
            hdt.ItemsSource = new Binding("Children");
            drawCM = new DrawContextMenu();
            textBlock.SetValue(TextBlock.ContextMenuProperty, drawCM.contextMenuForTasks);
            hdt.VisualTree = textBlock;
            treeView.ItemTemplate = hdt;
            treeView.ItemsSource = project.Root;
           
            Thickness margin = treeView.Margin;
            margin.Top = 10;
            margin.Right = 1000;
            margin.Bottom = 10;
            treeView.Margin = margin;

            CreateTaskTree(grid);
        }

        private void CreateTaskTree(Grid grid)
        {
            grid.Children.Add(treeView);
        }

        public void RefreshTreeView()
        {
            treeView.Items.Refresh();
        }

        public void GetTreeViewItemFocus()
        {
            if (treeView.SelectedItem != null)
            {
                SelectedTask = (treeView.SelectedItem as Logic.Task);
            }
        }

        public void Search(Project project, GlobalVariables.Operations operations)
        {
            foreach (var child in project.Root)
            {
                if (SelectedTask.Equals(child))//TODO: null check
                {
                    switch (operations)
                    {
                        case GlobalVariables.Operations.Add:
                            child.AddChildren(GlobalVariables.BufferTask);
                            break;  
                        case GlobalVariables.Operations.Delete:
                            project.Root.Remove(SelectedTask);
                            break;
                        case GlobalVariables.Operations.Edit:
                            EditMode(child);
                            break;
                        case GlobalVariables.Operations.FillBuffer:
                            {
                                GlobalVariables.BufferTask.TaskName = child.TaskName;
                                GlobalVariables.BufferTask.ShortDescription = child.ShortDescription;
                                GlobalVariables.BufferTask.LongDescription = child.LongDescription;
                                GlobalVariables.BufferTask.Start = child.Start;
                                GlobalVariables.BufferTask.Finish = child.Finish;
                                GlobalVariables.BufferTask.BufferResponsible = child.Responsible.Name;

                                GlobalVariables.ChildFlag = false;
                            }                          
                            break;
                    }      
                    return;
                }
                ChildrenSearch(child, operations);
            }
        }

        private void ChildrenSearch(Logic.Task child, GlobalVariables.Operations operations)
        {
            foreach (var child_ in child.Children)
            {
                if (SelectedTask.Equals(child_))
                {
                    switch (operations)
                    {
                        case GlobalVariables.Operations.Add:
                            child_.AddChildren(GlobalVariables.BufferTask);
                            break;
                        case GlobalVariables.Operations.Delete:
                            child.DeleteChildren(SelectedTask);
                            break;
                        case GlobalVariables.Operations.Edit:
                            EditMode(child_);
                            break;
                        case GlobalVariables.Operations.FillBuffer:
                            {
                                GlobalVariables.BufferTask.TaskName = child_.TaskName;
                                GlobalVariables.BufferTask.ShortDescription = child_.ShortDescription;
                                GlobalVariables.BufferTask.LongDescription = child_.LongDescription;
                                GlobalVariables.BufferTask.Start = child_.Start;
                                GlobalVariables.BufferTask.Finish = child_.Finish;
                                GlobalVariables.BufferTask.BufferResponsible = child_.Responsible.Name;

                                GlobalVariables.ChildFlag = true;
                            }
                            break;
                    }
                    return;
                }
                ChildrenSearch(child_, operations);
            }
        }
        
        private void EditMode(Logic.Task task)
        {
            task.TaskName = GlobalVariables.BufferTask.TaskName;
            task.ShortDescription = GlobalVariables.BufferTask.ShortDescription;
            task.LongDescription = GlobalVariables.BufferTask.LongDescription;
            task.Start = GlobalVariables.BufferTask.Start;
            task.Finish = GlobalVariables.BufferTask.Finish;
            task.Responsible.Name = GlobalVariables.BufferTask.Responsible.Name;
            task.SetStatus(Status.Start);

            foreach(var resp in GlobalVariables.BufferTask.Responsible.AvailableTasks)
            {
                task.Responsible.AvailableTasks.Add(resp);
            }
        }
    }
}
