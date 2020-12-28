using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TODOList.Logic;

namespace TODOList.Drawing.Navigation
{
    public class Canvases
    {
        private object locker;

        private List<SolidColorBrush> _Brushes;

        public ScrollViewer _Scroll { get;  set; }//private
        public Canvas _Canvas { get; private set; }

        public List<TextBlock> _TextBlocks { get; private set; }
        public List<TextBlock> _TasksTextBlocks { get; private set; }
        public List<Line> _Lines { get; private set; }
        public List<List<List<Rectangle>>> _Rectangles { get;  set; } //private
        public List<CheckBox> _CheckBoxesPerf { get; private set; }
        public List<CheckBox> _CheckBoxesPause { get; private set; }

        private enum CheckBoxType
        {
            Performed,
            Pause
        };

        private int YForLines = 50;
        private int XForName = 10;
        private int XForCbPerf = 200;
        private int XForCbPause = 250;
        private int StartXForRectangles;
        private int RectCounter;  

        public Canvases(Frames fr, int year, int month)
        {
            locker = new object();

            _Brushes = new List<SolidColorBrush>();
            InitializeBrushes();
            _Scroll = new ScrollViewer();
            _Canvas = new Canvas();

            _TextBlocks = new List<TextBlock>();
            _Lines = new List<Line>();
            _Rectangles = new List<List<List<Rectangle>>>();

            _CheckBoxesPerf = new List<CheckBox>();
            _CheckBoxesPause = new List<CheckBox>();
            _TasksTextBlocks = new List<TextBlock>();

            Markup(year, month);
            InitializeRectList(fr);
        }

        public void AddLine(Logic.Task task, int year, int month)
        {
            CreateCheckBoxes(CheckBoxType.Performed, task);
            CreateCheckBoxes(CheckBoxType.Pause, task);
            CreateHorizontalLine(0, (int)_Canvas.Width, YForLines);
            CreateTextBlock(task.TaskName);
            RectCounter = CountNumberOfRect(task);
            InitializeRectangle(task);
            AddRectOnCurrentPage(year, month - 1, task.TaskName);

            IncYForLines();
        }

        public void ExpandCanvasHeight()
        {
            _Canvas.Height *= 2;
        }

        private void IncYForLines()
        {
            if (_Canvas.Height > YForLines)
                YForLines += 25;
            else
            {
                ExpandCanvasHeight();
                YForLines += 25;
            }
        }

        private void InitializeBrushes()
        {
            _Brushes.Add(CreateBrush(0, 255, 0));
            _Brushes.Add(CreateBrush(128, 128, 128));
            _Brushes.Add(CreateBrush(255, 255, 0));
            _Brushes.Add(CreateBrush(255, 0, 0));
            _Brushes.Add(CreateBrush(255, 255, 255));
        }

        private SolidColorBrush CreateBrush(int r, int g, int b)
        {
            SolidColorBrush _Brush = new SolidColorBrush();
            _Brush.Color = Color.FromRgb((byte)r, (byte)g, (byte)b);
            return _Brush;
        }

        private void Markup(int year, int month)
        {
            int counter = DateTime.DaysInMonth(year, month);

            _Scroll.Width = 970;
            _Scroll.Height = 462;
            _Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            _Canvas.Width = _Scroll.Width + 250;
            _Canvas.Height = _Scroll.Height * 2;
            int MarginTopForFirstLine = 10;
            int MarginBottomForFirstLine = (int)_Canvas.Height - 20;

            _Canvas.Background = Brushes.Beige;

            CreateTextBlocks(counter, MarginTopForFirstLine, MarginBottomForFirstLine);
            CreateVerticalLines(counter, _TextBlocks[1], _TextBlocks[2]);

            foreach (var l in _Lines)
            {
                _Canvas.Children.Add(l);
            }

            CreateStartHorizontalLine();

            foreach (var tb in _TextBlocks)
            {
                _Canvas.Children.Add(tb);
            }
            
            _Scroll.Content = _Canvas;
        }

        private void CreateTextBlocks(int counter, int MarginTopForFirstLine, int MarginBottomForFirstLine)
        {
            #region TextBlockses
            TextBlock tb1 = new TextBlock();
            TextBlock tb2 = new TextBlock();
            TextBlock tb3 = new TextBlock();

            tb1.Text = "Task name";
            tb2.Text = "Performed";
            tb3.Text = "Pause";

            tb1.Margin = SetMargin<TextBlock>(tb1, 10, MarginTopForFirstLine, (int)_Canvas.Width - 80, MarginBottomForFirstLine);
            tb2.Margin = SetMargin<TextBlock>(tb2, (int)_Canvas.Width - (int)tb1.Margin.Right + 100, MarginTopForFirstLine, (int)tb2.Margin.Left + 80, MarginBottomForFirstLine);
            tb3.Margin = SetMargin<TextBlock>(tb3, (int)tb2.Margin.Left + 60, MarginTopForFirstLine, (int)_Canvas.Width - (int)tb3.Margin.Left - 60, MarginBottomForFirstLine);

            _TextBlocks.Add(tb1);
            _TextBlocks.Add(tb2);
            _TextBlocks.Add(tb3);

            int MarginLeft = (int)tb3.Margin.Left + 50;
            //StartXForRectangles = MarginLeft;
            int MarginRight = (int)_Canvas.Width - MarginLeft - 20;
            for (int i = 0; i < counter; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = (i + 1).ToString();
                tb.Margin = SetMargin<TextBlock>(tb, MarginLeft, MarginTopForFirstLine, MarginRight, MarginBottomForFirstLine);
                _TextBlocks.Add(tb);
                MarginLeft += 30;
            }
            #endregion
        }

        private void CreateVerticalLines(int counter, TextBlock tb2, TextBlock tb3)
        {
            #region Vertical Lines
            Line line1 = new Line();
            Line line2 = new Line();
            line1.Stroke = Brushes.Black;
            line2.Stroke = Brushes.Black;
            line1.X1 = (int)tb2.Margin.Left - 5;
            line1.X2 = (int)tb2.Margin.Left - 5;
            line1.Y1 = 0;
            line1.Y2 = _Canvas.Height;
            line2.X1 = (int)tb3.Margin.Left - 2;
            line2.X2 = (int)tb3.Margin.Left - 2;
            line2.Y1 = 0;
            line2.Y2 = _Canvas.Height;

            _Lines.Add(line1);
            _Lines.Add(line2);

            int XForLine = (int)tb3.Margin.Left + 40;
            StartXForRectangles = XForLine;
            for (int i = 0; i <= counter; i++)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.X1 = XForLine;
                line.X2 = XForLine;
                line.Y1 = 0;
                line.Y2 = _Canvas.Height;
                _Lines.Add(line);
                XForLine += 30;
            }
            #endregion 
        }

        private void CreateStartHorizontalLine()
        {
            #region Horizontal Line
            CreateHorizontalLine(0, (int)_Canvas.Width, 25);
            #endregion
        }

        private void CreateCheckBoxes(CheckBoxType cbt, Logic.Task task)
        {
            CheckBox cb = new CheckBox();
            cb.IsChecked = false;
            cb.Name = SetControlName<CheckBox>(cb, task.TaskName);
            
            switch (cbt)
            {
                case CheckBoxType.Performed:
                    cb.Click += (object sender, RoutedEventArgs e) =>
                    {
                        if (cb.IsChecked == true)
                        {
                            SearchRectForFill(StatusColor.Gray, task.TaskName);
                            task.SetStatus(Status.Finish);
                        }
                        else
                        {
                            SearchRectForFill(StatusColor.Green, task.TaskName);
                            task.SetStatus(Status.Start);
                        }
                    };                 
                    SetCheckBoxMargin(cb, XForCbPerf, YForLines - 17);
                    _CheckBoxesPerf.Add(cb);
                    _Canvas.Children.Add(cb);
                    break;
                case CheckBoxType.Pause:
                    cb.Click += (object sender, RoutedEventArgs e) =>
                    {
                        if (cb.IsChecked == true)
                        {
                            SearchRectForFill(StatusColor.Yellow, task.TaskName);
                            task.SetStatus(Status.Stopped);
                        }
                        else
                        {
                            SearchRectForFill(StatusColor.Green, task.TaskName);
                            task.SetStatus(Status.Start);
                        }
                    };
                    SetCheckBoxMargin(cb, XForCbPause, YForLines - 17);
                    _CheckBoxesPause.Add(cb);
                    _Canvas.Children.Add(cb);
                    break;
            }
        }

        public void SearchRectForFill(StatusColor color, string Name)
        {
            lock (locker)
            {
                char trimSymbol = ' ';
                for (int i = 0; i < _Rectangles.Count; ++i)
                {
                    foreach (var rt in _Rectangles)
                    {
                        for (int j = 0; j < rt.Count; ++j)
                        {
                            if (rt[j].Count > 0)
                            {
                                Application.Current.Dispatcher.Invoke(() => FillRectangle(rt[j].Find(x => x.Name == Name.Trim(trimSymbol)), color));
                            }
                        }
                    }
                }
            }  
        }

        private void CreateTextBlock(string text)
        {
            TextBlock tb = new TextBlock();
            tb.Text = text;
            tb.Margin = SetMargin<TextBlock>(tb, XForName, YForLines - 18, 0, 0);
            char trimSymbol = ' ';
            tb.Name = text.Trim(trimSymbol);
            _TasksTextBlocks.Add(tb);
            _Canvas.Children.Add(tb);
        }

        private void CreateHorizontalLine(int x1, int x2, int y)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y;
            line.Y2 = y;
            _Lines.Add(line);
            _Canvas.Children.Add(line);
        }

        private void SetCheckBoxMargin(CheckBox cb, int Left, int Top)
        {
            Thickness margin = cb.Margin;
            margin.Left = Left;
            margin.Top = Top;
            cb.Margin = margin;
        }

        private Thickness SetMargin<T>(object control, int Left, int Top, int Right, int Bottom) where T : FrameworkElement
        {
            var _control = control as T;
            Thickness margin = _control.Margin;
            margin.Left = Left;
            margin.Top = Top;
            margin.Right = Right;
            margin.Bottom = Bottom;
            return margin;
        }

        private string SetControlName<T>(object control, string text) where T : FrameworkElement
        {
            char trimSymbol = ' ';
            var _control = control as T;
            if(Char.IsNumber(text, 0))
            {
                MessageBox.Show("Incorrect task name!");
                return string.Empty;
            }
            else
            {
                return text.Trim(trimSymbol);
            }      
        }

        private void InitializeRectList(Frames fr)
        {
            int year = DateTime.Now.Year;

            for (int i = 0; i <= fr.NumberOfYears; i++) //-1
            {
                List<List<Rectangle>> RYear = new List<List<Rectangle>>();//year
                for (int j = 0; j < 12; j++)
                {
                    List<Rectangle> RMonth = new List<Rectangle>();//month
                    RYear.Add(RMonth);
                }
                _Rectangles.Add(RYear);
                ++year;
            }  
        }

        private void AddRectOnCurrentPage(int year, int month, string RectName)
        {
            //string writePath = @"E:\Download\logs.txt";
            //string text = $"Year: {year}  Month: {month}  Name: {RectName}";
            //using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(text);
            //    sw.WriteLine("");

            //    for (int i = 0; i < _Rectangles.Count; ++i)
            //    {
            //        for(int j = 0; j < _Rectangles[i].Count; ++j)
            //        {
            //            sw.WriteLine($"Year: {i}  Month: {j} Count: {_Rectangles[i][j].Count}");
            //        }
            //    }
            //}

            if (_Rectangles[year][month].Count != 0)
                if(_Rectangles[year][month].Find(x => x.Name == RectName) != null)
                    _Canvas.Children.Add(_Rectangles[year][month].Find(x => x.Name == RectName));
        }

        private void InitializeRectangle(Logic.Task task)
        {
            int CurrentYear = task.Start.Year - DateTime.Now.Year;
            int CurrentMonth = task.Start.Month;
            int DaysRectangleCounter = 0;
            int Width = 0;
            int tmpLeft;
            DateTime tmpMonth = task.Start;
            DateTime tmpYear = task.Start;

            for (int i = 0; i <= RectCounter; ++i) //=
            {
                Rectangle rect = new Rectangle();
                
                if ((tmpMonth.Day + DaysRectangleCounter) < DateTime.DaysInMonth(tmpYear.Year, CurrentMonth)) //Если НЕТ выхода за пределы месяца
                {
                    ++DaysRectangleCounter;
                }
                else
                {
                    Width = (DaysRectangleCounter + 1) * 30;
                    tmpLeft = tmpMonth.Day - 1;
                    CreateRectangle(rect, task, Width, tmpLeft, CurrentYear, CurrentMonth - 1);

                    if (CurrentMonth + 1 <= 12) //Если НЕТ выхода за пределы года ..=
                    {
                        ++CurrentMonth;
                        tmpMonth = new DateTime(tmpYear.Year, CurrentMonth, 1);
                        DaysRectangleCounter = 0;
                    }
                    else
                    {
                        if(CurrentYear + 1 < _Rectangles.Count)
                        {
                            ++CurrentYear;
                            CurrentMonth = 1;
                            tmpYear = tmpYear.AddYears(1);
                            tmpMonth = new DateTime(tmpYear.Year, 1, 1);        
                            DaysRectangleCounter = 0;
                        }
                    }

                }
            }
            Rectangle rect2 = new Rectangle();
            if (RectCounter == 1)
                Width = 30;
            else
                Width = (DaysRectangleCounter) * 30;
            int tmpLeft2 = tmpMonth.Day - 1;
            CreateRectangle(rect2, task, Width, tmpLeft2, CurrentYear, CurrentMonth - 1);
        }

        private void CreateRectangle(Rectangle rect, Logic.Task task, int Width, int Left, int CurrentYear, int CurrentMonth)
        {
            FillRectangle(rect, task.TaskStatusColor);
            rect.Name = SetControlName<Rectangle>(rect, task.TaskName);
            rect.Height = 25;
            rect.Width = Width;
            int LeftMargin = StartXForRectangles + Left * 30;
            int TopMargin = (int)_TasksTextBlocks.Find(x => x.Name == task.TaskName).Margin.Top - 7;
            rect.Margin =  SetMargin<Rectangle>(rect, LeftMargin, TopMargin, 0, 0); 
            _Rectangles[CurrentYear][CurrentMonth].Add(rect);
        }

        private int CountNumberOfRect(Logic.Task task)
        {
            if((task.Finish - task.Start).Days == 0)
            {
                return 1;
            }
            else
            {
                return (task.Finish - task.Start).Days;
            }
        }

        private void FillRectangle(Rectangle rect, StatusColor color)
        {
            if(rect != null)
            {
                switch (color)
                {
                    case StatusColor.Green:
                        rect.Fill = _Brushes[(int)StatusColor.Green];
                        break;
                    case StatusColor.Gray:
                        rect.Fill = _Brushes[(int)StatusColor.Gray];
                        break;
                    case StatusColor.Yellow:
                        rect.Fill = _Brushes[(int)StatusColor.Yellow];
                        break;
                    case StatusColor.Red:
                        rect.Fill = _Brushes[(int)StatusColor.Red];
                        break;
                    case StatusColor.White:
                        rect.Fill = _Brushes[(int)StatusColor.White];
                        break;
                }
            }  
        }
        
        public void UpdateRectangles(Project project, Pages page, int year, int month)
        {
            foreach(var child in project.Root)
            {
                if (child.Children.Count == 0)
                {
                    SearchAndFill(page, year, month, child);
                }
                else
                {
                    SearchAndFill(page, year, month, child);
                    SubSearch(child, page, year, month);
                }
            }
        }

        private void SubSearch(Logic.Task child, Pages page, int year, int month)
        {
            foreach (var child_ in child.Children)
            {
                if (child_.Children.Count == 0)
                {
                    SearchAndFill(page, year, month, child_);
                }
                else
                {
                    SearchAndFill(page, year, month, child_);
                    SubSearch(child_, page, year, month);
                }
            }
        }
        
        private void SearchAndFill(Pages page, int year, int month, Logic.Task child)
        {
            List<Rectangle> results = page.canvas._Rectangles[year][month].FindAll(x => x.Name == child.TaskName);
            foreach (var item in results)
            {
                FillRectangle(item, child.TaskStatusColor);
            }
        }
    }
}
