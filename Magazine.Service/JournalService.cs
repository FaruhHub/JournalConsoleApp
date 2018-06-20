using Magazine.Data;
using Magazine.Repo;
using System;
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Magazine.Service
{
    public class JournalService : IJournalService
    {
        private readonly IRepository<Journal> repoJournal;

        public JournalService(IRepository<Journal> repoJournal)
        {
            this.repoJournal = repoJournal;
        }

        public bool InsertJournals(List<Journal> journals)
        {            
            return repoJournal.InsertCollection(journals);
        }        

        public List<Journal> GetData(eSource source)
        {
            try
            {
                if (source == eSource.DataBase)
                    return repoJournal.GetData();
                else
                {
                    string rootPath = System.Environment.CurrentDirectory.Replace(@"MagazineConsoleApp\bin\Debug", @"Magazine.Data\Uploads\");
                    string fileName = "UploadedEvents.csv";
                    string pathToFile = rootPath + fileName;
                    List<Journal> lst = new List<Journal>();
                    using (CsvReader csvReader = new CsvReader(new StreamReader(pathToFile), hasHeaders: false))
                    {
                        while (csvReader.ReadNextRecord())
                        {
                            string mes = csvReader[0];
                            string dateStr = csvReader[1];
                            if (string.IsNullOrEmpty(mes) || string.IsNullOrEmpty(dateStr))
                                continue;
                            DateTime datetime = Convert.ToDateTime(dateStr);
                            lst.Add(new Journal { Message = mes, MessageDate = datetime });
                        }
                    }
                    return lst;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(List<Journal>);
            }
        }


        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    repoJournal.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public bool SaveToFile(List<Journal> entityCollection)
        {
            try
            {
                string rootPath = System.Environment.CurrentDirectory.Replace(@"MagazineConsoleApp\bin\Debug", @"Magazine.Data\Uploads\");
                string fileName = "UploadedEvents.csv";
                string pathToFile = rootPath + fileName;

                using (FileStream fs = new FileStream(pathToFile, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (var entity in entityCollection)
                        {
                            string row = string.Format("{0},{1}", entity.Message.ToString(), entity.MessageDate);
                            sw.WriteLine(row);
                            
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        public List<Journal> GetJournalsFromJSON(string journalsInJSON)
        {
            try
            {
                if (!journalsInJSON.StartsWith("[") && !journalsInJSON.EndsWith("]"))
                    journalsInJSON = "[" + journalsInJSON + "]";
                List<Journal> journals = JsonConvert.DeserializeObject<List<Journal>>(journalsInJSON);
                return journals;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: {0},\nStackTrace: {1}", ex.Message, ex.StackTrace);
                return default(List<Journal>);
            }
        }
    }
}
