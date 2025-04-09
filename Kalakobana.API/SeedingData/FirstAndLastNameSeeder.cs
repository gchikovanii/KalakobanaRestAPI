using ClosedXML.Excel;

namespace SeedingData
{
    public class FirstAndLastNameSeeder : IFirstAndLastNameSeeder
    {
        private readonly string path = @"C:\Resources\names.xlsx";
        private readonly List<string> firstNames = new();
        private readonly List<string> lastNames = new();
        public List<string> SeedFirstNames()
        {
            using (var workBook = new XLWorkbook(path))
            {
                var workSheet = workBook.Worksheet(1);
                int row = 2;
                while (!string.IsNullOrWhiteSpace(workSheet.Cell(row, 1).GetString()))
                {
                    string firstName = workSheet.Cell(row, 1).GetString().Trim();
                    if (firstName.Contains("w"))
                    {
                        row++;
                        continue;
                    }
                    firstNames.Add(firstName);
                    row++;
                }
            }
            return firstNames;
        }
        public List<string> SeedLastNames()
        {
            using (var workBook = new XLWorkbook(path))
            {
                var workSheet = workBook.Worksheet(1);
                int row = 2;
                while (!string.IsNullOrWhiteSpace(workSheet.Cell(row, 6).GetString()))
                {
                    string lastName = workSheet.Cell(row, 6).GetString().Trim();
                    if (lastName.Contains("w"))
                    {
                        row++;
                        continue;
                    }
                    if (lastName.Contains("-"))
                    {
                        string[] sepparatedLastNames = lastName.Split('-');
                        foreach (var lName in sepparatedLastNames)
                        {
                            lastNames.Add(lName);
                        }
                        row++;
                        continue;
                    }
                    lastNames.Add(lastName);
                    row++;
                }
            }
            return lastNames;
        }
    }
}
