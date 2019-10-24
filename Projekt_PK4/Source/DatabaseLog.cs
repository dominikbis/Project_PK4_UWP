using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public class DatabaseLog
    {
        protected event MedicineEventHandler AddEventLog;
        protected event MedicineEventHandler DeleteEventLog;
        protected event MedicineEventHandler ChangeEventLog;


        public List<List<string>> Log = new List<List<string>>();


        public void OnAddEventLog(MedicineEventArgs e)
        {
            AddEventLog.Invoke(this, e);
            if (e.log.Count > 0)
            {
                Log.Add(e.log);
            }
        }
        public void OnDeleteEventLog(MedicineEventArgs e)
        {
            DeleteEventLog.Invoke(this, e);
            if (e.log.Count > 0)
            {
                Log.Add(e.log);
            }
        }
        public void OnChangeEventLog(MedicineEventArgs e)
        {
            ChangeEventLog.Invoke(this, e);
            if (e.log.Count > 2)//2 - liczba minimalna, oznacza brak zmian - (1)informacja o nazwie, (2)informacja o czasie edycji 
            {
                Log.Add(e.log);
            }
        }


        public DatabaseLog()
        {
            AddEventHandler();
        }

        protected void AddEventHandler()
        {

            AddEventLog += MedicineEventHandlerClass.AddMedicineLog;
            AddEventLog += MedicineEventHandlerClass.ChangeTypeLog;
            AddEventLog += MedicineEventHandlerClass.ChangePriceLog;
            AddEventLog += MedicineEventHandlerClass.ChangeAgeRestrictionsLog;
            AddEventLog += MedicineEventHandlerClass.ChangeIntendedLog;
            AddEventLog += MedicineEventHandlerClass.ChangeCompositionLog;
            AddEventLog += MedicineEventHandlerClass.ChangeCommentsLog;
            AddEventLog += MedicineEventHandlerClass.ChangeRM_LevelLog;
            AddEventLog += MedicineEventHandlerClass.ChangeRM_FundingLimitLog;
            AddEventLog += MedicineEventHandlerClass.AddReplacementsLog;
            AddEventLog += MedicineEventHandlerClass.TimeLog;

            ChangeEventLog += MedicineEventHandlerClass.EditMedicineLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeNameLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeTypeLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangePriceLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeAgeRestrictionsLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeIntendedLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeCompositionLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeCommentsLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeRM_LevelLog;
            ChangeEventLog += MedicineEventHandlerClass.ChangeRM_FundingLimitLog;
            ChangeEventLog += MedicineEventHandlerClass.AddReplacementsLog;
            ChangeEventLog += MedicineEventHandlerClass.DeleteReplacementsLog;
            ChangeEventLog += MedicineEventHandlerClass.TimeLog;

            DeleteEventLog += MedicineEventHandlerClass.DeleteMedicineLog;
            DeleteEventLog += MedicineEventHandlerClass.TimeLog;
        }
    }
}
