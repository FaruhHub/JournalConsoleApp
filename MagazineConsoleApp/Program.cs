using Magazine.Data;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernal = new StandardKernel(new JournalModule());
            JournalOperations journalOperation = new JournalOperations(kernal);
            journalOperation.DisplayWelcomeScreen();
            
        }
    }
}
