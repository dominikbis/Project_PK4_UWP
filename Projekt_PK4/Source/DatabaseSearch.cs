using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public class DatabaseSearch : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Medicine> displayedMedBase;

        private string name;
        private string type;
        private double maxPrise;
        private string intended;
        private string composition;
        private bool reimbursed;

        public DatabaseSearch(ObservableCollection<Medicine> medBase)
        {
            var query = medBase.OrderBy(med => med.Name).ToList();//sort

            displayedMedBase = new ObservableCollection<Medicine>(query);

            Name = "";
            Type = "";
            MaxPrise = 0;
            Intended = "";
            Composition = "";
            Reimbursed = false;
        }


        public void ClearCriteria(ObservableCollection<Medicine> medBase)//
        {
            displayedMedBase.Clear();
            displayedMedBase.AddRange(medBase);

            Name = "";
            Type = "";
            MaxPrise = 0;
            Intended = "";
            Composition = "";
            Reimbursed = false;
        }

        public string Name
        {
            get => name;
            set { name = value;OnPropertyChanged(); }
        }
        public string Type
        {
            get => type;
            set { type = value; OnPropertyChanged(); }
        }
        public double MaxPrise
        {
            get => maxPrise;
            set { maxPrise = value; OnPropertyChanged(); }
        }
        public string Intended
        {
            get => intended;
            set { intended = value; OnPropertyChanged(); }
        }
        public string Composition
        {
            get => composition;
            set { composition = value; OnPropertyChanged(); }
        }
        public bool Reimbursed
        {
            get => reimbursed;
            set { reimbursed = value; OnPropertyChanged(); }
        }

        public void Search(ObservableCollection<Medicine> medBase)
        {
            var query = medBase
                .Where(med => med.Name.IndexOf(Name) >= 0)//filter
                .Where(med => med.Type.IndexOf(Type) >= 0)
                .Where(med => med.Price <= MaxPrise || MaxPrise == 0)
                .Where(med => med.RM_Reimbursed == Reimbursed || Reimbursed == false)
                .Where(med => med.Intended.IndexOf(Intended) >= 0)
                .Where(med => med.Composition.IndexOf(Composition) >= 0)
                .OrderBy(med => med.Name).ToList();//sort

            displayedMedBase.Clear();
            displayedMedBase.AddRange(query);
        }

    }


}
