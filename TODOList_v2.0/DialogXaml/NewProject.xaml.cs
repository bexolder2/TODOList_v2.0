using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TODOList.DialogXaml
{
    public partial class NewProject : Window
    {
        public event EventHandler SaveNewProject;

        public NewProject()
        {
            InitializeComponent();

            DataContext = Logic.GlobalVariables.BufferPrj;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MouseLeftButtonDown_Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Name saved: {Logic.GlobalVariables.BufferPrj.ProjectName}");
            Dialogs.GetData.EndSave();
            Close();
            SaveNewProject?.Invoke(this, null); //go to MainWindow.xaml.cs
            Dialogs.DialogOperations.GetTaskData();
            //Dialogs.GetData.GetProjectName();  
        }
    }
}