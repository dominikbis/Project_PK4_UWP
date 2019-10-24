using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public delegate void MedicineEventHandler(object sender, MedicineEventArgs e);

    public class MedicineEventArgs : EventArgs
    {
        public Medicine oldMedicine;
        public Medicine newMedicine;

        public List<string> log;

        public MedicineEventArgs(Medicine oldMedicine_, Medicine newMedicine_)
        {
            oldMedicine = oldMedicine_;
            newMedicine = newMedicine_;
            log = new List<string>();
        }

        public MedicineEventArgs(Medicine medicine_)
        {
            newMedicine = medicine_;
            oldMedicine = new Medicine();
            log = new List<string>();
        }
    }

    public static class MedicineEventHandlerClass
    {

        public static void AddMedicineLog(object sender, MedicineEventArgs e)
        {
            e.log.Add($"Dodano lek: {e.newMedicine.Name}");
        }
        public static void DeleteMedicineLog(object sender, MedicineEventArgs e)
        {
            e.log.Add($"Usunieto lek: {e.newMedicine.Name}");
        }
        public static void EditMedicineLog(object sender, MedicineEventArgs e)
        {
            e.log.Add($"Edytowano lek: {e.oldMedicine.Name}");//stara nazwa leku przed edycja
        }
        public static void TimeLog(object sender, MedicineEventArgs e)
        {
            e.log.Add($"Data: {DateTime.Now}");
        }

        public static void ChangeNameLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.Name != e.newMedicine.Name)
            {
                e.log.Add($"!   Zmieniono nazwe leku: {e.oldMedicine.Name} -> {e.newMedicine.Name}");
            }
        }
        public static void ChangeTypeLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.Type != e.newMedicine.Type)
            {
                e.log.Add($"    Typ: {e.oldMedicine.Type} -> {e.newMedicine.Type}");
            }
        }
        public static void ChangePriceLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.Price != e.newMedicine.Price)
            {
                e.log.Add($"    Cena: {e.oldMedicine.Price} -> {e.newMedicine.Price}");
            }
        }
        public static void ChangeAgeRestrictionsLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.AgeRestrictions != e.newMedicine.AgeRestrictions)
            {
                e.log.Add($"    Ograniczenia wiekowe: {e.oldMedicine.AgeRestrictions} -> {e.newMedicine.AgeRestrictions}");
            }
        }
        public static void ChangeIntendedLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.Intended != e.newMedicine.Intended)
            {
                e.log.Add($"    Przeznaczenie: {e.oldMedicine.Intended} -> {e.newMedicine.Intended}");
            }
        }
        public static void ChangeCompositionLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.Composition != e.newMedicine.Composition)
            {
                e.log.Add($"    Sklad: {e.oldMedicine.Composition} -> {e.newMedicine.Composition}");
            }
        }
        public static void ChangeCommentsLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.Comments != e.newMedicine.Comments)
            {
                e.log.Add($"    Komentarz: {e.oldMedicine.Comments} -> {e.newMedicine.Comments}");
            }
        }
        public static void ChangeRM_LevelLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.RM_Level != e.newMedicine.RM_Level)
            {
                e.log.Add($"    Poziom odplatnosci: {e.oldMedicine.RM_Level} -> {e.newMedicine.RM_Level}");
            }
        }
        public static void ChangeRM_FundingLimitLog(object sender, MedicineEventArgs e)
        {
            if (e.oldMedicine.RM_FundingLimit != e.newMedicine.RM_FundingLimit)
            {
                e.log.Add($"    Limit refundacji: {e.oldMedicine.RM_FundingLimit} -> {e.newMedicine.RM_FundingLimit}");
            }
        }

        public static void AddReplacementsLog(object sender, MedicineEventArgs e)
        {
            foreach (Medicine replacements in e.newMedicine.replacements) 
            {
                if (e.oldMedicine.replacements.IndexOf(replacements) < 0) 
                {
                    e.log.Add($"        + zamiennik: {replacements.Name}");
                }
            }
        }
        public static void DeleteReplacementsLog(object sender, MedicineEventArgs e)
        {
            foreach (Medicine replacements in e.oldMedicine.replacements)
            {
                if (e.newMedicine.replacements.IndexOf(replacements) < 0)
                {
                    e.log.Add($"        - zamiennik: {replacements.Name}");
                }
            }
        }

    }

}
