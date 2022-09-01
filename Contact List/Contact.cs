using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_List
{
    public class Contact
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string Culture { get; set; }

        public Contact(string name, int number, string cult)
        {
            if (name != null)
            {
                Name = name;
                Number = number;
                Culture = cult;
            }
            else
            {
                Console.WriteLine("Name cannot be null");
            }
        }
    }
}
