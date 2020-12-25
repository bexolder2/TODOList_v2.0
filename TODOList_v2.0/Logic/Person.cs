using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList.Logic
{
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public List<Task> AvailableTasks { get; set; }

        public Person()
        {
            AvailableTasks = new List<Task>();
        }
    }
}
