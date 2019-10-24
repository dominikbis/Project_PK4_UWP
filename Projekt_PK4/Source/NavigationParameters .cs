using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public class NavigationParameters
    {
        public Database database;
        public int index;

        public NavigationParameters(Database database_, int index_)
        {
            database = database_;
            index = index_;
        }
    }
}
