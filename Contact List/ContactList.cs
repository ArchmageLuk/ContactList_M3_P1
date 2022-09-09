using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_List
{
    public class ContactList
    {
        string ContactsPathEN = @"D:/Projects/C# learning/Code/ContactList_M3_P1/Contact List/Contact_List_EN.txt";
        string ContactsPathUA = @"D:/Projects/C# learning/Code/ContactList_M3_P1/Contact List/Contact_List_UA.txt";
        string ContactsPathOther = @"D:/Projects/C# learning/Code/ContactList_M3_P1/Contact List/Contact_List_Other.txt";
        string ContactsPathNumbers = @"D:/Projects/C# learning/Code/ContactList_M3_P1/Contact List/Contact_List_Numbers.txt";

        char[] AlphabetEN = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        char[] AlphabetUA = "АБВГҐДЕЄЖЗИІЇКЛМНОПРСТУФХЦЧШЩЬЮЯ".ToCharArray();
        char[] AlphabetOther = "#".ToCharArray();
        char[] AlphabetNumbers = "1234567890".ToCharArray();
                
        Dictionary<string, List<Contact>> ContactListEN { get; set; }
        Dictionary<string, List<Contact>> ContactListUA { get; set; }
        Dictionary<string, List<Contact>> ContactListOther { get; set; }
        Dictionary<string, List<Contact>> ContactListNumbers { get; set; }

        public ContactList()
        {    
            ContactListEN = CreateAlphDict(AlphabetEN);
            ContactListUA = CreateAlphDict(AlphabetUA);
            ContactListOther = CreateAlphDict(AlphabetOther);
            ContactListNumbers = CreateAlphDict(AlphabetNumbers);

            TextToDict(ContactsPathEN, "EN");
            TextToDict(ContactsPathUA, "UA");
            TextToDict(ContactsPathOther, "Other");
            TextToDict(ContactsPathNumbers, "Number");
        }

        //INTERACTIONS----------------------------------------------------
        public void ContactListInteraction()
        {
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine("Welcome to your Contact List");

            var contacts = Counting(ContactListEN);

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

        //TOOLS--------------------------------------------------------------------------------------
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

        private Dictionary<string, List<Contact>> CreateAlphDict(char[] alphabet)
        {
            Dictionary<string, List<Contact>> dict = new Dictionary<string, List<Contact>>();
            foreach (char letter in alphabet)
            {
                var letString = letter.ToString();
                var list = new List<Contact>();
                var nullContact = new Contact($"No contacts", 0);
                list.Add(nullContact);
                dict.Add(letString, list);
                list.Clear();
            }
            return dict;
        }
        
        //SAVING INFO--------------------------------------------------------------------------
        private void Save(Contact contact)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Saving...");

            ContToDict(contact);

            DictToText(ContactListEN, ContactsPathEN);
            DictToText(ContactListUA, ContactsPathUA);
            DictToText(ContactListNumbers, ContactsPathNumbers);
            DictToText(ContactListOther, ContactsPathOther);

            ShowList();
            Run();
        }

        private void ContToDict(Contact contact)
        {
            char[] nameAnalysis = contact.Name.ToCharArray();

            var path = "EmptyPath";
            bool check = false;

            foreach (char letter in AlphabetEN)
            {
                if (letter == nameAnalysis[0])
                {
                    AddToDict(nameAnalysis[0], ContactListEN, contact);
                    contact.Culture = "en-US";
                    check = true;
                    break;
                }
            }

            foreach (char letter in AlphabetUA)
            {
                if (letter == nameAnalysis[0])
                {
                    AddToDict(nameAnalysis[0], ContactListUA, contact);
                    contact.Culture = "ua-UA";
                    check = true;
                    break;
                }
            }

            foreach (char letter in AlphabetNumbers)
            {
                if (letter == nameAnalysis[0])
                {
                    AddToDict(nameAnalysis[0], ContactListNumbers, contact);
                    contact.Culture = "Number";
                    check = true;
                    break;
                }
            }

            if (check == false)
            {
                AddToDict(nameAnalysis[0], ContactListOther, contact);
                contact.Culture = "Other";
            }
            

            void AddToDict(char symbol, Dictionary<string, List<Contact>> dict, Contact cont)
            {
                foreach (var letter in dict.Keys)
                {
                    if (symbol.ToString() == letter)
                    {
                        var list = dict[letter];
                        list.Add(cont);
                        list = Sorting(list);
                        dict[letter] = list;
                    }
                }
            }
        }

        private void DictToText(Dictionary<string, List<Contact>> targDict, string path)
        {
            List<string> newArr = new List<string>();

            foreach (var info in targDict)
            {
                newArr.Add(info.Key);
                var valueArr = info.Value.ToArray();
                foreach (var item in valueArr)
                {
                    string cont = item.Name + " - " + item.Number;
                    newArr.Add(cont);
                }
            }

            File.WriteAllLines(path, newArr);
            Console.WriteLine(" ");
            Console.WriteLine("Saved in file");
        }

        //DEPACKINGS--------------------------------------------------------------------------------
        private void TextToDict(string filepath, string cult)
        {
            var data = File.ReadAllLines(filepath);
            foreach (var line in data)
            {
                if (line.Length > 1)
                {
                    var arrLine = line.Split(" - ");
                    var name = arrLine[0];
                    var number = Int32.Parse(arrLine[1]);
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

                var cult = CultureInfo.CurrentCulture.Name;

                if (cult == "en-US")
                {
                    FindAndRemove(ContactListEN);
                    DictToText(ContactListEN, ContactsPathEN);
                }
                else if (cult == "ua-UA")
                {
                    FindAndRemove(ContactListUA);
                    DictToText(ContactListUA, ContactsPathOther);
                }
                else
                {
                    FindAndRemove(ContactListOther);
                    DictToText(ContactListOther, ContactsPathOther);
                }

                void FindAndRemove(Dictionary<string, List<Contact>> contactList)
                {
                    foreach (var letter in contactList.Keys)
                    {
                        if (nameAnalysis[0].ToString() == letter)
                        {
                            var list = contactList[letter];
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (delName == list[i].Name)
                                {
                                    contactList[letter].Remove(list[i]);
                                }
                            }
                        }
                    }
                }                
            }
            
            ShowList();
            Run();
        }

        void ClearAll()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Clear EN, UA, Numbers or Other?");
            var clearWhat = Console.ReadLine();

            switch (clearWhat)
            {
                case "EN":
                    File.Delete(ContactsPathEN);
                    File.Create(ContactsPathEN);
                    break;
                case "UA":
                    File.Delete(ContactsPathUA);
                    File.Create(ContactsPathUA);
                    break;
                case "Numbers":
                    File.Delete(ContactsPathNumbers);
                    File.Create(ContactsPathNumbers);
                    break;
                case "Other":
                    File.Delete(ContactsPathOther);
                    File.Create(ContactsPathOther);
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
            ShowList();
            Run();
        }

        void ShowList()
        {
            Console.WriteLine(" ");
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine($"There is {Counting(ContactListEN)} EN, {Counting(ContactListUA)} UA, {Counting(ContactListNumbers)} and {Counting(ContactListOther)} Other contacts now");

            void Display(Dictionary<string, List<Contact>> dictionary)
            {
                foreach (var contact in dictionary)
                {
                    Console.WriteLine(contact.Key);
                    var list = contact.Value;
                    foreach (var line in list)
                    {
                        Console.WriteLine(line.Name + " - " + line.Number);
                    }
                }
            }

            Console.WriteLine("ENGLISH CONTACTS-------------------------------------------------------------------");
            Display(ContactListEN);
            Console.WriteLine("UKRAINIAN CONTACTS-----------------------------------------------------------------");
            Display(ContactListUA);
            Console.WriteLine("NUMBER CONTACTS--------------------------------------------------------------------");
            Display(ContactListNumbers);
            Console.WriteLine("OTHER CONTACTS---------------------------------------------------------------------");
            Display(ContactListOther);
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
