using Magazine.Data;
using Magazine.Repo;
using Magazine.Service;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineConsoleApp
{
    
    class JournalOperations : IDisposable
    {
        private readonly IJournalService journalService;
        private readonly List<Journal> m_newCreatedJournalEvents = new List<Journal>();
        List<Journal> m_retrievedListofJournals;
        private eSource m_selectedSource;

        public JournalOperations(IKernel kernel)
        {
            journalService = kernel.Get<JournalService>();
        }

        #region Methods
        public void DisplayWelcomeScreen()
        {
            Console.WriteLine("----------========= Welcome to Journal events  =========----------\n\n");
            Console.WriteLine("Please choose the source:\n\t1. DataBase\n\t2. File\nEnter key \"1\" to choose first option. \"2\" to choose second option. Press the Escape (Esc) key to quit\n");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                Console.WriteLine("");
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        m_selectedSource = eSource.DataBase;
                        DisplayMenu(eSource.DataBase);
                        break;
                    case ConsoleKey.D2:
                        m_selectedSource = eSource.File;
                        DisplayMenu(eSource.File);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
            while (key.Key != ConsoleKey.Escape);

        }
        public void DisplayMenu(eSource source)
        {
            string selectedSource = source == eSource.DataBase ? "DataBase" : "File";
            Console.Write("\nMain Features:\n\t");
            Console.WriteLine("1. Adding new journal events.\n\t" +
                              "2. Save events to DataBase and to File.\n\t" +
                              "3. Load events from the {0}\n\t" +
                              "4. Output events to the screen.\n\t" +
                              "5. Finding elements by the text of an event\n\t" +
                              "6. Press the Escape (Esc) key to quit. \n", selectedSource);

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        AddEvent();
                        break;
                    case ConsoleKey.D2:
                        SaveEvents();
                        break;
                    case ConsoleKey.D3:
                        LoadEvents(source);
                        break;
                    case ConsoleKey.D4:
                        OutputEvents();
                        break;
                    case ConsoleKey.D5:
                        SearchEvents();
                        break;
                    case ConsoleKey.D6:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
            while (key.Key != ConsoleKey.Escape);  

        }

        public void AddEvent()
        {
            do
            {
                Console.Write("\nPlease enter new event message: ");
                string mes = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(mes))
                {
                    Console.WriteLine("Validation error: new event message can not be empty! ");
                    AddEvent();
                }

                if (mes.IsJSON())
                {
                    var convertedCollection = journalService.GetJournalsFromJSON(mes);
                    if (convertedCollection != null)
                    {
                        m_newCreatedJournalEvents.AddRange(convertedCollection);
                        Console.WriteLine("----=== New event created. Press the Escape (Esc) key to quit adding new event or any button to continue ===----");

                    }
                    else
                    {
                        Console.WriteLine("Validation error: you entered not valid JSON object! ");
                    }
                }
                else
                {
                    Console.Write("Please specify the date of new event(Optional): ");
                    string dateStr = Console.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(dateStr))
                    {
                        DateTime datetime;
                        bool result = DateTime.TryParse(dateStr, out datetime);
                        if (result)
                            m_newCreatedJournalEvents.Add(new Journal { Message = mes, MessageDate = datetime });
                    }
                    else
                        m_newCreatedJournalEvents.Add(new Journal { Message = mes, MessageDate = DateTime.Now });

                    Console.WriteLine("----=== New event created. Press the Escape (Esc) key to quit adding new event or any button to continue ===----");
                }

            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("----=== Please select option \"2\" to save created events ===----");
            DisplayMenu(m_selectedSource);

        }
        public void SaveEvents()
        {
            if (m_newCreatedJournalEvents.Count > 0)
            {
                bool result = journalService.InsertJournals(m_newCreatedJournalEvents);
                if (result)
                {
                    journalService.SaveToFile(m_newCreatedJournalEvents);
                    Console.WriteLine("----=== New created journal event are saved ===----");
                    m_newCreatedJournalEvents.Clear();
                }
                else
                    Console.WriteLine("----=== New created journal event are not saved ===----");
            }
            else
            {
                Console.WriteLine("----=== You don't have any created journal events! ===----");
            }

            DisplayMenu(m_selectedSource);
        }
        public void LoadEvents(eSource source)
        {
            m_retrievedListofJournals = journalService.GetData(source);
            string selectedSource = source == eSource.DataBase ? "DataBase" : "File";
            Console.WriteLine("The data has been uploaded from {0}", selectedSource);

            DisplayMenu(source);
        }
        public void OutputEvents()
        {
            do
            {
                if (m_retrievedListofJournals == null)
                {
                    Console.WriteLine("\nPlease first load events");
                    DisplayMenu(m_selectedSource);
                }

                if (m_retrievedListofJournals.Count > 0)
                {
                    Console.WriteLine("\n----=== You have {0} events in journal! ===----", m_retrievedListofJournals.Count);
                    foreach (var item in m_retrievedListofJournals)
                        Console.WriteLine("Message: {0}, Date: {1}.", item.Message, item.MessageDate.ToString());
                }
                else
                    Console.WriteLine("\n----=== You don't have any events in journal! ===----");
                Console.WriteLine("-----=== Press the Escape (Esc) key to quit or any button to continue ===-----");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            
            DisplayMenu(m_selectedSource);
        }
        public void SearchEvents()
        {
            do
            {
                if (m_retrievedListofJournals == null)
                {
                    Console.WriteLine("\nPlease first load events for searching");
                    DisplayMenu(m_selectedSource);
                }

                if (m_retrievedListofJournals.Count > 0)
                {
                    Console.Write("Please specify your search expression: ");
                    string searchExpression = Console.ReadLine().Trim();
                    List<Journal> temp = m_retrievedListofJournals.Where(x => x.Message.Contains(searchExpression)).ToList();
                    Console.WriteLine("\n----=== You have {0} matches in journal! ===----", temp.Count);
                    foreach (var item in temp)
                        Console.WriteLine("Message: {0}, Date: {1}.", item.Message, item.MessageDate.ToString());

                    Console.WriteLine("");
                }
                else
                    Console.WriteLine("\n----=== You don't have any events for searching in journal! ===----");
                Console.WriteLine("-----=== Press the Escape (Esc) key to quit or any button to continue ===-----");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            
            DisplayMenu(m_selectedSource);

        }
        
        

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    journalService.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
