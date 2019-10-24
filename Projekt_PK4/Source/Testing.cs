using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projekt_PK4
{
    public static class RandomDatabase
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz     ";//A-Z a-z 5x' '
        private static Random rnd = new Random();

        private static string RandomText(int lengthMin, int lengthMax)
        {
            return new string(Enumerable.Repeat(chars, rnd.Next(lengthMin, lengthMax)).Select(letter => letter[rnd.Next(chars.Length)]).ToArray());
        }

        private static double RandomPrice(int maxValue)
        {
            return (double)rnd.Next(maxValue * 100) / 100;
        }

        private static LevelOfFunding RandomLevelOfFunding()
        {
            return (LevelOfFunding)Enum.GetValues(typeof(LevelOfFunding)).GetValue(rnd.Next(Enum.GetValues(typeof(LevelOfFunding)).Length));
        }


        public static void AddRandomMedicineToDatabase(Database database)
        {
            string nameMed = RandomText(3, 20);
            while (Regex.IsMatch(nameMed, "^[^ ].*$") == false) 
            {
                nameMed = RandomText(3, 20);
            }


            StructMedicine sMed = new StructMedicine(nameMed, RandomText(0, 15), RandomPrice(1000), rnd.Next(80), RandomText(0, 200), RandomText(0, 200), RandomText(0, 200));
            ReimbursedMedicine rMed = new ReimbursedMedicine(RandomPrice(1000), RandomLevelOfFunding());

            database.AddMedicine(new Medicine(sMed, rMed));
        }

        public static void GenerateRandomReplacementsInDatabase(Database database)
        {
            if (database.medBase.Count() > 0) 
            {
                int index = rnd.Next(database.medBase.Count());
                Medicine med = database[index].Copy();
                ReplacementsCreator creator = new ReplacementsCreator();

                int count = rnd.Next(5);
                for (int i = 0; i < count; i++)
                {
                    Medicine rMed = database.medBase[rnd.Next(database.medBase.Count())];

                    if (database[index] != rMed && med.replacements.IndexOf(rMed) < 0)
                    {
                        med.replacements.Add(rMed);
                        creator.medToAdd.Add(rMed);
                    }
                }

                database.EditMedicine(med, index, creator);
            }

        }
    }
}
