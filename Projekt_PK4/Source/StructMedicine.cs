using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    //enum TypesMedicines { pills ,}

    public class StructMedicine : ICloneable
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int AgeRestrictions { get; set; }
        public string Intended { get; set; }
        public string Composition { get; set; }
        public string Comments { get; set; }



        public StructMedicine(string name_ = "", string type_ = "", double price_ = 0, int ageRestrictions_ = 0,
            string intended_ = "", string composition_ = "", string comments_ = "")
        {
            this.Name = name_;
            this.Type = type_;
            this.Price = price_;
            this.AgeRestrictions = ageRestrictions_;
            this.Intended = intended_;
            this.Composition = composition_;
            this.Comments = comments_;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        //public static bool operator ==(StructMedicine med1, StructMedicine med2) => string.Compare(med1.Name, med2.Name, true) == 0 ? true : false;
        //public static bool operator !=(StructMedicine med1, StructMedicine med2) => string.Compare(med1.Name, med2.Name, true) != 0 ? true : false;

        //public static bool operator <(StructMedicine med1, StructMedicine med2) => string.Compare(med1.Name, med2.Name, true) < 0 ? true : false;
        //public static bool operator >(StructMedicine med1, StructMedicine med2) => string.Compare(med1.Name, med2.Name, true) > 0 ? true : false;


        //public override bool Equals(object obj)
        //{
        //    if (obj != null && GetType().Equals(obj.GetType())) //sprawdzenie czy nie null oraz czy ten sam Type
        //    {
        //        StructMedicine med = obj as StructMedicine;
        //        return Name == med.Name;
        //    }
        //    else
        //        return false;
        //}

        //public override int GetHashCode() => base.GetHashCode();
    }
}
