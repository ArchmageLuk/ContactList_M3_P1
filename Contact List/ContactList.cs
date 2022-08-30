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
        char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#".ToCharArray();

        Dictionary<string, List<Contact>> TheContactList { get; set; }

        public ContactList()
        {
            var dictionary = new Dictionary<string, List<Contact>>();
            
            foreach (char letter in Alphabet)
            {
                var letString = letter.ToString();
                var list = new List<Contact>();
                var nullContact = new Contact($"No contacts", 0);
                list.Add(nullContact);
                dictionary.Add(letString, list);
                list.Clear();
            }
            TheContactList = dictionary;
            TextToDict(ContactsPath);
        }

        //INTERACTIONS----------------------------------------------------
        public void ContactListInteraction()
        {
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine("Welcome to your Contact List");

            var contacts = Counting(TheContactList);

            if (contacts > 0)
            {
                Console.WriteLine("These are your current contacts:");
                ShowList();
            }
            else
            {
                Console.WriteLine("You have no contacts for now");
            }

            Run();
        }

        private int Counting(IDictionary<string, List<Contact>> collection)
        {
            int contacts = 0;

            foreach (var list in collection)
            {
                contacts = contacts + list.Value.Count();
            }
            return contacts;
        }

        private List<Contact> Sorting(List<Contact> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (string.Compare(list[i].Name, list[i+1].Name) > 0)
                {
                    var bigger = list[i+1];
                    var smaller = list[i];
                    list[i] = bigger;
                    list[i+1] = smaller;
                    continue;
                }
            }
            return list;
        }

        //SAVING INFO--------------------------------------------------------------------------
        private void Save(Contact contact)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Saving...");

            ContToDict(contact);
            DictToText();

            ShowList();
            Run();
        }

        private void ContToDict(Contact contact)
        {
            char[] nameAnalysis = contact.Name.ToCharArray();

            foreach (var letter in TheContactList.Keys)
            {
                if (nameAnalysis[0].ToString() == letter)
                {
                    var list = TheContactList[letter];
                    list.Add(contact);
                    list = Sorting(list);
                    TheContactList[letter] = list;
                }
            }
        }

        private void DictToText()
        {
            List<string> newArr = new List<string>();

            foreach (var info in TheContactList)
            {
                newArr.Add(info.Key);
                var valueArr = info.Value.ToArray();
                foreach (var item in valueArr)
                {
                    string cont = item.Name + " - " + item.Number;
                    newArr.Add(cont);
                }
            }

            File.WriteAllLines(ContactsPath, newArr);
            Console.WriteLine(" ");
            Console.WriteLine("Saved in file");
        }

        //DEPACKINGS--------------------------------------------------------------------------------
        private void TextToDict(string filepath)
        {
            var data = File.ReadAllLines(filepath);
            foreach (var line in data)
            {
                if (line.Length > 1)
                {
                    
                    var arrLine = line.Split(" - ");
                    var name = arrLine[0];
                    var number = int.Parse(arrLine[1]);
                    var contact = new Contact(name, number);
                    ContToDict(contact);
                }
            }
        }

        //FUNCTIONS----------------------------------------------------------------------------------
        void AddNumber()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Please enter the name");
            string name = Console.ReadLine();

            Console.WriteLine("Please enter the number");
            var number = Int32.Parse(Console.ReadLine());

            if (name != null & number != 0)
            {
                var contact = new Contact(name, number);
                Save(contact);
                Run();
            }
            else
            {
                Console.WriteLine("Name or number cannot be empty");
                Run();
            }
        }

        void RemoveContact()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Write name to delete");
            var delName = Console.ReadLine();
            if (string.IsNullOrEmpty(delName))
            {
                Console.WriteLine(" ");
                Console.WriteLine("Name cannot be empty");
                Run();
            }
            else
            {
                char[] nameAnalysis = delName.ToCharArray();
                
                foreach (var letter in TheContactList.Keys)
                {
                    if (nameAnalysis[0].ToString() == letter)
                    {
                        var list = TheContactList[letter];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (delName == list[i].Name)
                            {
                                TheContactList[letter].Remove(list[i]);
                            }
                        }
                    }
                }
            }
            DictToText();
            ShowList();
            Run();
        }

        void ClearAll()
        {

            File.Delete(ContactsPath);
            File.Create(ContactsPath);
            ShowList();
            Run();
        }

        void ShowList()
        {
            Console.WriteLine(" ");
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine($"There is {Counting(TheContactList)} contacts now");
            foreach (var contact in TheContactList)
            {
                Console.WriteLine(contact.Key);
                var list = contact.Value;
                foreach (var line in list)
                {
                    Console.WriteLine(line.Name + " - " + line.Number);
                }
            }
            Run();
        }

        //WORKLOGICS
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
                        RemoveContact();
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
