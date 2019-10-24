using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{

    public class ReplacementsCreator
    {
        public ObservableCollection<Medicine> medToDelete;
        public ObservableCollection<Medicine> medToAdd;

        public ReplacementsCreator()
        {
            medToAdd = new ObservableCollection<Medicine>();
            medToDelete = new ObservableCollection<Medicine>();
        }

        public void Update(Medicine mainMed)
        {
            foreach (Medicine med in medToAdd)
            {
                med.replacements.Add(mainMed);
            }
            foreach (Medicine med in medToDelete)
            {
                med.replacements.Remove(mainMed);
            }
        }

        public static void deleteAll(Medicine med)
        {
            foreach (Medicine replacement in med.replacements)
            {
                replacement.replacements.Remove(med);
            }
        }

    }
}
