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
    public class DrawTabItem
    {
        public DrawTaskTree drawTT { get; private set; }
        public Navigation.Navigator nv { get; private set; }
        private Grid TabGrid;
        private DrawContextMenu drawCM;

        private Label lMonth { get; set; }

        public enum Month
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        public void CreateTabItem(string title, Project project, TabControl tabControl)
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = title;
            TabGrid = new Grid();
            tabItem.Content = TabGrid;
            drawTT = new DrawTaskTree(TabGrid, project);
            nv = new Navigation.Navigator(TabGrid);

            drawCM = new DrawContextMenu();
            CreateButtons();
            SetLabelProperties();
            tabItem.ContextMenu = drawCM.contextMenuForTabItems;
            tabControl.Items.Add(tabItem);
            tabItem.Focus();
        }

        #region UI
        private void SetLabelProperties()
        {
            lMonth = new Label();

            Thickness margin = lMonth.Margin;
            margin.Left = 370;
            margin.Top = 480;
            margin.Right = 690;
            lMonth.Margin = margin;
            TabGrid.Children.Add(lMonth);
        }

        private void CreateButtons()
        {
            Button bBack = new Button();
            Button bNext = new Button();
            Button bShow = new Button();

            #region margins
            bBack.Content = "<";
            bNext.Content = ">";
            bShow.Content = "Show diagram";
            bBack.Height = 20;
            bBack.Width = 20;
            bNext.Height = 20;
            bNext.Width = 20;
            bShow.Height = 20;
            bShow.Width = 100;
            Thickness margin = bBack.Margin;
            margin.Left = 200;
            margin.Top = 480;
            margin.Right = 950;
            margin.Bottom = 10;
            bBack.Margin = margin;
            Thickness margin2 = bNext.Margin;
            margin2.Left = 230;
            margin2.Top = 480;
            margin2.Right = 920;
            margin2.Bottom = 10;
            bNext.Margin = margin2;
            Thickness margin3 = bShow.Margin;
            margin3.Left = 260;
            margin3.Top = 480;
            margin3.Right = 800;
            margin3.Bottom = 10;
            bShow.Margin = margin3;
            #endregion

            bBack.IsEnabled = false;
            bNext.IsEnabled = false;

            bBack.Click += (object sender, RoutedEventArgs e) =>
            {
                if (nv.CurrentPage - 1 >= 0) //если месяцы НЕ закончились
                {
                    Back();
                }
                else //Если месяцы ЗАКОНЧИЛИСЬ
                {

                    if (nv.CurrentYear - 1 >= 0) //Если ЕСТЬ год позади 
                    {
                        nv.CurrentPage = 12;
                        IncOrDecDate(-1);
                        Back();
                    }
                    else //Если НЕТ года позади 
                    {
                        IncOrDecDate(nv.pages_.Count - 1);
                        nv.CurrentPage = 12;
                        Back();
                    }
                }
            };

            bNext.Click += (object sender, RoutedEventArgs e) =>
            {
                if (nv.pages_.Count != 0)
                {
                    if (nv.pages_[nv.CurrentYear].Count > nv.CurrentPage + 1) //если месяцы НЕ закончились
                    {
                        Next();
                    }
                    else //Если месяцы ЗАКОНЧИЛИСЬ
                    {
                        if (nv.pages_.Count > nv.CurrentYear + 1) //Если есть год ВПЕРЕДИ
                        {
                            nv.CurrentPage = -1;
                            IncOrDecDate(1);
                            Next();
                        }
                        else if (nv.CurrentYear - 1 >= 0) //Если есть год ПОЗАДИ
                        {
                            nv.CurrentPage = -1;
                            IncOrDecDate(-1);
                            Next();
                        }
                        else //Если год ЕДИНСТВЕННЫЙ
                        {
                            nv.CurrentPage = -1;
                            Next();
                        }
                    }
                }
            };

            bShow.Click += (object sender, RoutedEventArgs e) =>
            {
                bBack.IsEnabled = true;
                bNext.IsEnabled = true;
                nv.frame_.RefreshTaskList();
                nv.frame_.ConvertToList(Program.Prj.Find(x => x.ProjectName == GlobalVariables.DrawingTabControl.GetFocusTabItemHeader()));
                nv.frame_.RefreshYears();
                nv.frame_.CheckYears();
                nv.RefreshPagesList();
                nv.AddPages(nv.frame_);
                nv.CurrentPage = 0;
                if (nv.pages_.Count != 0)
                    nv.frame_.frame.Navigate(nv.pages_[0][0].page);
                SetLabelText();
                //MessageBox.Show($"Invalidate y:{nv.CurrentYear} p:{nv.CurrentPage} counter:{nv.pages_.Count}");
            };

            TabGrid.Children.Add(bBack);
            TabGrid.Children.Add(bNext);
            TabGrid.Children.Add(bShow);
        }

        private void IncOrDecDate(int inc)
        {
            nv.CurrentYear += inc;
            nv.Year = nv.Year.AddYears(inc);
        }

        private void Back()
        {
            //
            nv.pages_[nv.CurrentYear][nv.CurrentPage - 1].canvas.UpdateRectangles(Program.Prj.
                Find(x => x.ProjectName == GlobalVariables.DrawingTabControl.GetFocusTabItemHeader()), 
                nv.pages_[nv.CurrentYear][nv.CurrentPage - 1], nv.CurrentYear, nv.CurrentPage - 1);
            //
            nv.frame_.frame.Navigate(nv.pages_[nv.CurrentYear][nv.CurrentPage - 1].page);
            --nv.CurrentPage;
            SetLabelText();
        }

        private void Next()
        {
            //
            if (nv.CurrentPage < 0)
            {
                nv.pages_[nv.CurrentYear][nv.CurrentPage + 1].canvas.UpdateRectangles(Program.Prj.
                    Find(x => x.ProjectName == GlobalVariables.DrawingTabControl.GetFocusTabItemHeader()), 
                    nv.pages_[nv.CurrentYear][nv.CurrentPage + 1], nv.CurrentYear, nv.CurrentPage + 1);//+2
            }
            else
            {
                nv.pages_[nv.CurrentYear][nv.CurrentPage + 1].canvas.UpdateRectangles(Program.Prj.
                    Find(x => x.ProjectName == GlobalVariables.DrawingTabControl.GetFocusTabItemHeader()), 
                    nv.pages_[nv.CurrentYear][nv.CurrentPage + 1], nv.CurrentYear, nv.CurrentPage + 1);
            }
            //
            nv.frame_.frame.Navigate(nv.pages_[nv.CurrentYear][nv.CurrentPage + 1].page);
            ++nv.CurrentPage;
            SetLabelText();
        }

        private void SetLabelText()
        {
            foreach (Month m in Enum.GetValues(typeof(Month)))
            {
                if ((int)m == nv.CurrentPage)
                {
                    lMonth.Content = $"{m} {nv.Year.Year}";
                }
            }
        }
        #endregion
    }
}
