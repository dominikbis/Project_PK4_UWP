using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Windows.Storage;
using Windows.Storage.Streams;

namespace Projekt_PK4
{
    public class Database
    {
        public DatabaseLog log;

        public ObservableCollection<Medicine> medBase;

        public string Name { get; protected set; }

        StorageFolder storageFolder;
        public string baseFile;
        public string logFile;


        public Database(string name_, string folderPath_)
        {
            log = new DatabaseLog();
            medBase = new ObservableCollection<Medicine>();
            this.Name = name_;

            storageFolder = ApplicationData.Current.LocalFolder;
            this.baseFile = folderPath_ + @"\" + name_ + ".txt";
            this.logFile = folderPath_ + @"\" + name_ + ".log";
        }

        public Database(string basePath_)
        {
            log = new DatabaseLog();
            medBase = new ObservableCollection<Medicine>();

            storageFolder = ApplicationData.Current.LocalFolder;
            this.baseFile = basePath_ + ".txt";
            this.logFile = basePath_ + ".log";

            Load();
        }

        public Medicine this[int index]//indeksatory
        {
            get { return medBase[index]; }
            set { medBase[index] = value; }
        }


        public void AddMedicine(Medicine medicine)
        {
            medBase.Add(medicine);
            log.OnAddEventLog(new MedicineEventArgs(medicine));
        }

        public void AddMedicine(Medicine medicine, ReplacementsCreator NewReplacements)
        {
            medBase.Add(medicine);
            NewReplacements.Update(medicine);
            log.OnAddEventLog(new MedicineEventArgs(medicine));
        }

        public void DeleteMedicine(int index)
        {
            log.OnDeleteEventLog(new MedicineEventArgs(medBase[index]));

            ReplacementsCreator.deleteAll(medBase[index]);

            medBase.RemoveAt(index);
        }

        public void DeleteMedicine(Medicine med)
        {
            log.OnDeleteEventLog(new MedicineEventArgs(med));

            ReplacementsCreator.deleteAll(med);

            medBase.Remove(med);
        }

        public void EditMedicine(Medicine med, int index)
        {
            log.OnChangeEventLog(new MedicineEventArgs(medBase[index], med));
            medBase[index].Copy(med);
        }

        public void EditMedicine(Medicine med, int index, ReplacementsCreator NewReplacements)
        {
            log.OnChangeEventLog(new MedicineEventArgs(medBase[index], med));
            medBase[index].Copy(med);
            NewReplacements.Update(medBase[index]);
        }


        public void Save()
        {
            Task taskSB = Task.Run(() => SaveBase());
            Task taskSL = Task.Run(() => SaveLog());

            taskSB.Wait();
            taskSL.Wait();
        }

        public void Load()
        {
            Task taskLB = Task.Run(() => LoadBase());
            Task taskLL = Task.Run(() => LoadLog());

            taskLB.Wait();
            taskLL.Wait();
        }


        private async Task SaveBase()
        {
            StorageFile storageFile = await storageFolder.CreateFileAsync(baseFile, CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite);

            using (IOutputStream outputStream = stream.GetOutputStreamAt(0)) 
            {
                using (DataWriter dataWriter = new DataWriter(outputStream))
                {
                    dataWriter.WriteInt32(Name.Length);
                    dataWriter.WriteString(Name);

                    dataWriter.WriteInt32(medBase.Count());

                    SaveMedicines(dataWriter);
                    SaveReplacements(dataWriter);

                    await dataWriter.StoreAsync();
                }
                //await outputStream.FlushAsync();
            }
            stream.Dispose();
        }

        private async Task SaveLog()
        {
            StorageFile storageFile = await storageFolder.CreateFileAsync(logFile, CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite);

            using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
            {
                using (DataWriter dataWriter = new DataWriter(outputStream))
                {
                    dataWriter.WriteInt32(log.Log.Count());

                    foreach (List<string> changeList in log.Log)
                    {
                        dataWriter.WriteInt32(changeList.Count());

                        foreach (String change in changeList)
                        {
                            dataWriter.WriteInt32(change.Length);
                            dataWriter.WriteString(change);
                        }
                    }

                    await dataWriter.StoreAsync();
                }
                //await outputStream.FlushAsync();
            }
            stream.Dispose();
        }

        private async Task LoadBase()
        {
            StorageFile storageFile = await storageFolder.GetFileAsync(baseFile);

            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;
            int length;
            int MedCounter;

            using (IInputStream inputStream = stream.GetInputStreamAt(0))
            {
                using (DataReader dataReader = new DataReader(inputStream))
                {
                    await dataReader.LoadAsync((uint)size);

                    length = dataReader.ReadInt32();
                    Name = dataReader.ReadString((uint)length);

                    MedCounter = dataReader.ReadInt32();

                    LoadMedicines(dataReader, MedCounter);
                    LoadReplacements(dataReader, MedCounter);
                }
            }
            stream.Dispose();
        }

        private async Task LoadLog()
        {
            StorageFile storageFile = await storageFolder.GetFileAsync(logFile);

            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);

            ulong size = stream.Size;
            int length;
            int LogCounter;
            int ChangeCounter;

            using (IInputStream inputStream = stream.GetInputStreamAt(0))
            {
                using (DataReader dataReader = new DataReader(inputStream))
                {
                    await dataReader.LoadAsync((uint)size);

                    LogCounter = dataReader.ReadInt32();

                    for (int i = 0; i < LogCounter; i++) 
                    {
                        log.Log.Add(new List<string>());

                        ChangeCounter = dataReader.ReadInt32();

                        for (int j = 0; j < ChangeCounter; j++)
                        {
                            length = dataReader.ReadInt32();
                            log.Log[i].Add(dataReader.ReadString((uint)length));
                        }
                    }
                }
            }
            stream.Dispose();
        }




        private void SaveMedicines(DataWriter dataWriter)
        {
            foreach (Medicine med in medBase)
            {
                dataWriter.WriteInt32(med.Name.Length);
                dataWriter.WriteString(med.Name);

                dataWriter.WriteInt32(med.Type.Length);
                dataWriter.WriteString(med.Type);

                dataWriter.WriteDouble(med.Price);

                dataWriter.WriteInt32(med.AgeRestrictions);

                dataWriter.WriteInt32(med.Intended.Length);
                dataWriter.WriteString(med.Intended);

                dataWriter.WriteInt32(med.Composition.Length);
                dataWriter.WriteString(med.Composition);

                dataWriter.WriteInt32(med.Comments.Length);
                dataWriter.WriteString(med.Comments);

                dataWriter.WriteDouble(med.RM_FundingLimit);

                dataWriter.WriteInt32((Int32)med.RM_Level);

                dataWriter.WriteGuid(med.GuidMed);
            }
        }

        private void SaveReplacements(DataWriter dataWriter)
        {
            foreach (Medicine med in medBase)
            {
                dataWriter.WriteInt32(med.replacements.Count());

                foreach (Medicine replacement in med.replacements)
                {
                    dataWriter.WriteGuid(replacement.GuidMed);
                }
            }
        }

        private void LoadMedicines(DataReader dataReader, int MedCounter)
        {
            StructMedicine sMed = new StructMedicine();
            ReimbursedMedicine rMed = new ReimbursedMedicine();

            int length;

            for (int i = 0; i < MedCounter; i++)
            {
                length = dataReader.ReadInt32();
                sMed.Name = dataReader.ReadString((uint)length);

                length = dataReader.ReadInt32();
                sMed.Type = dataReader.ReadString((uint)length);

                sMed.Price = dataReader.ReadDouble();

                sMed.AgeRestrictions = dataReader.ReadInt32();

                length = dataReader.ReadInt32();
                sMed.Intended = dataReader.ReadString((uint)length);

                length = dataReader.ReadInt32();
                sMed.Composition = dataReader.ReadString((uint)length);

                length = dataReader.ReadInt32();
                sMed.Comments = dataReader.ReadString((uint)length);

                rMed.FundingLimit = dataReader.ReadDouble();

                rMed.Level = (LevelOfFunding)dataReader.ReadInt32();

                medBase.Add(new Medicine(sMed, rMed));
                medBase[i].GuidMed = dataReader.ReadGuid();
            }
        }

        private void LoadReplacements(DataReader dataReader, int MedCounter)
        {
            int RM_Counter;
            Guid RM_GuidMed;

            for (int i = 0; i < MedCounter; i++)
            {
                RM_Counter = dataReader.ReadInt32();
                for (int j = 0; j < RM_Counter; j++)
                {
                    RM_GuidMed = dataReader.ReadGuid();

                    foreach (Medicine med in medBase)
                    {
                        if (med.GuidMed == RM_GuidMed)
                        {
                            if (medBase[i].replacements.IndexOf(med) < 0)
                            {
                                medBase[i].replacements.Add(med);
                                med.replacements.Add(medBase[i]);
                            }
                            break;
                        }
                    }
                }
            }
        }

    }

}