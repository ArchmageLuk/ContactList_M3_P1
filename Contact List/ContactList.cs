using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_List
{
    public class ContactList
    {
        string ContactsPath = @"D:/Projects/C# learning/Code/ContactList_M3_P1/Contact List/Contact_List.txt";
        string[] ContactsArr { get; set; } = new string[100];
        Dictionary<int, string> Contacts { get; set; }

        public ContactList()
        {
            Dictionary<int, string> contactlist = new Dictionary<int, string>();
            var number = 0;
            var name = "Nobody";
            ContactsArr = File.ReadAllLines(ContactsPath);
            foreach (string contact in ContactsArr)
                {
                    string[] person = contact.Split(" - ");
                    number = int.Parse(person[0]);
                    name = person[1];
                    contactlist.Add(number, name);
                }
            Contacts = contactlist;
        }

        public void ContactListInteraction()
        {
            Console.WriteLine("Welcome to your Contact List");
            Console.WriteLine("These are your current contacts:");
            foreach (var contact in Contacts)
            {
                Console.WriteLine($"{contact.Key} - {contact.Value}");
            }
            Run();
        }

        void Save(int number, string name)
        {
            for(int i = 0; i < ContactsArr.Length; i++)
            {
                Console.WriteLine(ContactsArr[i]);
                if (string.IsNullOrEmpty(ContactsArr[i]))
                {
                    ContactsArr[i] = number + " - " + name;
                    break;
                }
            }
            File.WriteAllLines(ContactsPath, ContactsArr);
        }

        void AddNumber()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Please enter the number");
            var number = int.Parse(Console.ReadLine());

            Console.WriteLine("Please enter the name");
            string name = Console.ReadLine();

            Contacts.Add(number, name);
            Save(number, name);
            Run();
        }

        void RemoveNumber()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Write number to delete");
            var delNumber = int.Parse(Console.ReadLine());
            Contacts.Remove(delNumber);
            Run();
        }

        void ClearAll()
        {
            Contacts = null;
            Run();
        }

        void ShowList()
        {
            foreach (var contact in Contacts)
            {
                Console.WriteLine(contact);
            }
            Run();
        }

            void Run()
            {
                Console.WriteLine(" ");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("Type 'Add', 'Remove', 'Clear all', 'Show list' to perform respective actions");
                Console.WriteLine(" ");

                string command = Console.ReadLine();
                switch (command)
                {
                    case "Add":
                        AddNumber();
                        break;
                    case "Remove":
                        RemoveNumber();
                        break;
                    case "Clear all":
                        ClearAll();
                        break;
                    case "Show list":
                        ShowList();
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
    }
}
