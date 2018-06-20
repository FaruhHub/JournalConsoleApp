using Magazine.Repo;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineConsoleApp
{
    public class JournalModule : NinjectModule
    {
        public override void Load()
        {
            Bind(Type.GetType("Magazine.Data.DIConsoleEntities, Magazine.Data")).ToSelf().InSingletonScope();
            Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InSingletonScope();
        }
    }
}
