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
    public class DrawTabControl
    {
        public TabControl tabControl { get; private set; }
        public List<DrawTabItem> tabItems { get; private set; }

        public DrawTabControl()
        {
            tabControl = new TabControl();
            tabItems = new List<DrawTabItem>();
        }

        public void CreateTabControl(Grid grid, string name)
        {
            tabControl.Name = name;
            Thickness margin = tabControl.Margin;
            margin.Top = 20;
            tabControl.Margin = margin;
            grid.Children.Add(tabControl);
        }     

        public string GetFocusTabItemHeader()
        {
            return (tabControl.Items[tabControl.SelectedIndex] as TabItem).Header.ToString();
        }

        public void AddTabItem(string title, Project project)
        {
            DrawTabItem ti = new DrawTabItem();
            ti.CreateTabItem(title, project, tabControl);
            tabItems.Add(ti);
        }

        public void DeleteTabItem()
        {
            tabControl.Items.Remove(tabControl.Items[tabControl.SelectedIndex]);
        }

        public int GetTabItemIndex()
        {
            if (GlobalVariables.DrawingTabControl.tabItems.Count == 0)
                return 0;
            else
                return GlobalVariables.DrawingTabControl.tabItems.Count - 1;
        }
    }
}
