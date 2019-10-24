using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public class Medicine : StructMedicine, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public ObservableCollection<Medicine> replacements;
        protected ReimbursedMedicine reimbursedMedicine;

        public Guid GuidMed { get; set; }


        public Medicine(string name = "", string type = "", double price = 0, int ageRestrictions = 0, string intended = "", string composition = "", string comments = "")
            : base(name, type, price, ageRestrictions, intended, composition, comments)
        {
            reimbursedMedicine = new ReimbursedMedicine();
            replacements = new ObservableCollection<Medicine>();

            GuidMed = Guid.NewGuid(); 
        }

        public Medicine(StructMedicine structMedicine_, ReimbursedMedicine reimbursedMedicine_)
            : base(structMedicine_.Name, structMedicine_.Type, structMedicine_.Price, structMedicine_.AgeRestrictions, structMedicine_.Intended, structMedicine_.Composition, structMedicine_.Comments)
        {
            reimbursedMedicine = (ReimbursedMedicine)reimbursedMedicine_.Clone();

            replacements = new ObservableCollection<Medicine>();

            GuidMed = Guid.NewGuid();
        }

        public Medicine Copy()
        {
            Medicine tempMedicine = new Medicine();
            tempMedicine = (Medicine)this.MemberwiseClone();
            tempMedicine.reimbursedMedicine = (ReimbursedMedicine)this.reimbursedMedicine.Clone();

            tempMedicine.replacements = new ObservableCollection<Medicine>(this.replacements);

            return tempMedicine;
        }

        public void Copy(Medicine med)
        {
            Name = med.Name;
            Type = med.Type;
            Price = med.Price;
            AgeRestrictions = med.AgeRestrictions;
            Intended = med.Intended;
            Composition = med.Composition;
            Comments = med.Comments;

            GuidMed = med.GuidMed;
            replacements = new ObservableCollection<Medicine>(med.replacements);

            reimbursedMedicine = (ReimbursedMedicine)this.reimbursedMedicine.Clone();
        }


        public new string Name
        {
            get => base.Name;
            set { base.Name = value; OnPropertyChanged(); }
        }
        public new string Type
        {
            get => base.Type;
            set { base.Type = value; OnPropertyChanged(); }
        }
        public new double Price
        {
            get => base.Price;
            set
            {
                base.Price = value;
                OnPropertyChanged();
                OnPropertyChanged("PriceAfterDiscount");
            }
        }
        public new int AgeRestrictions
        {
            get => base.AgeRestrictions;
            set { base.AgeRestrictions = value; OnPropertyChanged(); }
        }
        public new string Intended
        {
            get => base.Intended;
            set { base.Intended = value; OnPropertyChanged(); }
        }
        public new string Composition
        {
            get => base.Composition;
            set { base.Composition = value; OnPropertyChanged(); }
        }
        public new string Comments
        {
            get => base.Comments;
            set { base.Comments = value; OnPropertyChanged(); }
        }
        public bool RM_Reimbursed => reimbursedMedicine.Level != LevelOfFunding.nierefundowany ? true : false;
        public double RM_FundingLimit
        {
            get => reimbursedMedicine.FundingLimit;
            set
            {
                reimbursedMedicine.FundingLimit = value;
                OnPropertyChanged();
                OnPropertyChanged("PriceAfterDiscount");
            }
        }
        public LevelOfFunding RM_Level
        {
            get => reimbursedMedicine.Level;
            set {
                reimbursedMedicine.Level = value;
                OnPropertyChanged();
                OnPropertyChanged("RM_Reimbursed");
                OnPropertyChanged("PriceAfterDiscount");
            }
        }
        public double PriceAfterDiscount => Price - reimbursedMedicine.Discount() > 0 ? Price - reimbursedMedicine.Discount() : 0;//zle dziala ryczalt, bo czasami jest darmowy

        public void Refresh()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Type");
            OnPropertyChanged("Price");
            OnPropertyChanged("AgeRestrictions");
            OnPropertyChanged("Intended");
            OnPropertyChanged("Composition");
            OnPropertyChanged("Comments");
            OnPropertyChanged("RM_Reimbursed");
            OnPropertyChanged("RM_FundingLimit");
            OnPropertyChanged("RM_Level");
            OnPropertyChanged("PriceAfterDiscount");
        }
    }

}