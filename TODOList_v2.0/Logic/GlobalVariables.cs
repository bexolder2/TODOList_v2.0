using System;
using System.Collections.Generic;

namespace TODOList.Logic
{
    public static class GlobalVariables
    {
        public static DateTime CurrentDate { get; set; }
        public static List<Project> Projects { get; set; }
        public static Project BufferPrj;// = new Project();
        public static Task BufferTask;
        public static Drawing.DrawTabControl DrawingTabControl = new Drawing.DrawTabControl();

        public static DialogXaml.NewProject newPr;// = new DialogXaml.NewProject();
        public static DialogXaml.NewTask newTask;

        public static bool ChildFlag { get; set; }

        public enum Operations
        {
            Add,
            Delete,
            Edit,
            FillBuffer
        };

        static GlobalVariables()
        {
            //LevelsCounter = 0;
            ChildFlag = false;
        }
    }
}
