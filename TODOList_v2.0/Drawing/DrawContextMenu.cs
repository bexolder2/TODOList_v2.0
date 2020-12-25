using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TODOList.Logic;

namespace TODOList.Drawing
{
    public class DrawContextMenu
    {
        public ContextMenu contextMenuForTasks { get; private set; }
        public ContextMenu contextMenuForTabItems { get; private set; }

        public DrawContextMenu()
        {
            contextMenuForTasks = new ContextMenu();
            contextMenuForTabItems = new ContextMenu();
            CreateMenuItemsForTasks();
            CreateMenuItemsForTabItems();
        }

        private void CreateMenuItemsForTasks()
        {
            MenuItem menuItem1 = new MenuItem();
            menuItem1.Header = "Добавить подзадачу";
            menuItem1.Click += AddTask_Click;
            contextMenuForTasks.Items.Add(menuItem1);
            
            MenuItem menuItem2 = new MenuItem();
            menuItem2.Header = "Удалить задачу";
            menuItem2.Click += DeleteTask_Click;
            contextMenuForTasks.Items.Add(menuItem2);

            MenuItem menuItem3 = new MenuItem();
            menuItem3.Header = "Редактировать задачу";
            menuItem3.Click += EditTask_Click;
            contextMenuForTasks.Items.Add(menuItem3);
        }

        private void CreateMenuItemsForTabItems()
        {
            MenuItem menuItem1 = new MenuItem();
            menuItem1.Header = "Добавить задачу";
            menuItem1.Click += AddRootTask_Click;
            contextMenuForTabItems.Items.Add(menuItem1);

            MenuItem menuItem2 = new MenuItem();
            menuItem2.Header = "Удалить проект";
            menuItem2.Click += DeleteProject_Click;
            contextMenuForTabItems.Items.Add(menuItem2);
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].drawTT.GetTreeViewItemFocus();
            Project tmpPrj = Program.Prj.Find(x => x.ProjectName == GlobalVariables.DrawingTabControl.GetFocusTabItemHeader());
            tmpPrj.DeleteTask();
            GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].drawTT.RefreshTreeView();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.ChildFlag = true;
            GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].drawTT.GetTreeViewItemFocus();
            Dialogs.DialogOperations.GetTaskData();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].drawTT.GetTreeViewItemFocus();
            Dialogs.DialogOperations.GetTaskData(GlobalVariables.Operations.Edit);
        }

        private void AddRootTask_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.ChildFlag = false;
            GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].drawTT.GetTreeViewItemFocus();
            Dialogs.DialogOperations.GetTaskData();
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            Program.DeleteProject(GlobalVariables.DrawingTabControl.GetFocusTabItemHeader());
        }

        //public void CreateMenuItem(string title)
        //{
        //    MenuItem menuItem = new MenuItem();
        //    menuItem.Header = title;
        //    menuItem.Click += (object sender, RoutedEventArgs e) => 
        //    {

        //    };
        //    contextMenu.Items.Add(menuItem);
        //}
    }
}
