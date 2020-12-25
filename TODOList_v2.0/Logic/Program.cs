using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;

namespace TODOList.Logic
{
    public enum Status
    {
        Start,
        Finish,
        Stopped,
        Overdue
    }
    public enum StatusColor
    {
        Green,
        Gray,
        Yellow,
        Red,
        White
    }

    [Serializable]
    public static class Program
    {
        public static List<Project> Prj;

        static Program()
        {
            Prj = new List<Project>();
        }

        public static void AddProject()
        {
            GlobalVariables.BufferPrj = new Project();
            GlobalVariables.BufferPrj.NewProject();
            //Projects.Add(project);
        } 

        public static void DeleteProject(string projectName)
        {
            Prj.Remove(Prj.Find(x => x.ProjectName == projectName));
            GlobalVariables.DrawingTabControl.DeleteTabItem();
            GlobalVariables.DrawingTabControl.tabControl.Items.Refresh();
        }

        public static void Serialize(string fileName, Project source)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, source);
            }
        }

        public static bool Deserialize(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (!string.IsNullOrEmpty(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    Prj.Add((Project)formatter.Deserialize(fs));
                }
                return true;
            }
            else
                return false;
        }
    }
}
