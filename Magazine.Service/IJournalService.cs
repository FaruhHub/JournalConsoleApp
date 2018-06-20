using Magazine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Service
{
    public interface IJournalService : IDisposable
    {
        List<Journal> GetData(eSource source);
        bool InsertJournals(List<Journal> journals);
        List<Journal> GetJournalsFromJSON(string journalsInJSON);
        bool SaveToFile(List<Journal> entityCollection);
    }
}
