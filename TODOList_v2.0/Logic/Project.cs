using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList.Logic
{
    [Serializable]
    public class Project : IDataErrorInfo
    {
        public string ProjectName { get; set; }
        public List<Task> Root { get; set; }

        public Project()
        {
            Root = new List<Task>();
        }

        public Project(string prName)
        {
            ProjectName = prName;
        }

        public void NewProject()
        {
            Dialogs.DialogOperations.GetNewProjectData(this);
        } 

        public void DeleteTask()
        {
            //GlobalVariables.DrawingTabControl.drawTT.SearchForDelete(this);
            GlobalVariables.DrawingTabControl.tabItems[GlobalVariables.DrawingTabControl.GetTabItemIndex()].
                drawTT.Search(this, GlobalVariables.Operations.Delete);
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "ProjectName":
                        if (string.IsNullOrWhiteSpace(ProjectName))
                        {
                            error = "Enter correct name.";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error => null;
    }
}
