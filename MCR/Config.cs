using System;
using Sims3.Gameplay.Utilities;

namespace Arro.MCR
{
    public class Config
    {
        public static void Parse()
        {
            var clothes = XmlDbData.ReadData("MCRClothes");
            if (clothes == null)
            {
                ClothesModuleInstalled = false;
                Console.WriteLine(ClothesModuleInstalled.ToString());
            }
            else
            {
                ClothesModuleInstalled = true;
                clothes.Tables.TryGetValue("Config", out var xmlDbTable);
                if (xmlDbTable != null)
                {
                    const int MinRows = 3;
                    const int MinColumns = 1;
                    foreach (XmlDbRow xmlDbRow in xmlDbTable.Rows)
                    {
                        Data.Clothes.Rows = Math.Max(MinRows, xmlDbRow.GetInt("Rows"));
                        Data.Clothes.Columns = Math.Max(MinColumns, xmlDbRow.GetInt("Columns"));
                        Console.WriteLine(Data.Clothes.Rows.ToString());
                        Console.WriteLine(Data.Clothes.Columns.ToString());
                    }
                }
            }
            var hair = XmlDbData.ReadData("MCRHair");
            if (hair == null)
            {
                HairModuleInstalled = false;
            }
            else
            {
                HairModuleInstalled = true;
            }
            var face = XmlDbData.ReadData("MCRFace");
            if (face == null)
            {
                FaceModuleInstalled = false;
            }
            else
            {
                FaceModuleInstalled = true;
            }
        }
        public static bool ClothesModuleInstalled { get; set; }
        public static bool HairModuleInstalled { get; set; }
        public static bool FaceModuleInstalled { get; set; }

        public static class Data
        {
            public static class Clothes
            {
                public static int Rows;
                public static int Columns;
            }
            public static class Hair
            {
            }
            public static class Face
            {
            }
        }
    }
}