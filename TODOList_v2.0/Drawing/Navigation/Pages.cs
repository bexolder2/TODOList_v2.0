using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TODOList.Drawing.Navigation
{
    public class Pages
    {
        public Page page { get; private set; }
        public Canvases canvas { get; set; }

        public Pages()
        {
            page = new Page();
        }

        public void CreatePage(Frames fr, int year, int month, int yearIndex)
        {
            CreateCanvas(fr, year, month);
            InitializeTasksOnCanvas(fr, yearIndex, month);
            page.Content = canvas._Scroll;
        }

        private void CreateCanvas(Frames fr, int year, int month)
        {
            canvas = new Canvases(fr, year, month);
        }

        private void InitializeTasksOnCanvas(Frames fr, int year, int month)
        {
            foreach (var task in fr.TasksWithoutTree)
            {
                NewTask(task, year, month);
            }
        }

        public void NewTask(Logic.Task task, int year, int month)
        {
            canvas.AddLine(task, year, month);
        }       
    }
}
