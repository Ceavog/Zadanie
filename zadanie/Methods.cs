using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using zadanie.Model;
using Action = zadanie.Model.Action;

namespace zadanie;

public static class Helper
{
    /// <summary>
    /// Reads csv file.
    /// </summary>
    /// <param name="filePath">Path to file</param>
    /// <param name="delimiter">If not specified default delimiter is ","</param>
    /// <typeparam name="T">Type to which map records</typeparam>
    /// <returns></returns>
    public static List<T> ReadCsvFile<T>(string filePath, string delimiter = ",") 
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = delimiter, 
            Encoding = Encoding.UTF8
        };
        
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, configuration);

        return csv.GetRecords<T>().ToList();
    }

    /// <summary>
    /// Returns paths to files from given folder.
    /// Folder must be in project directory.  
    /// </summary>
    /// <param name="folderName"></param>
    /// <returns></returns>
    public static string[] PathByFolderName(string folderName)
    {
        var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var pathToFolder = Path.GetFullPath(Path.Combine(sCurrentDirectory, @$"..\..\..\{folderName}\"));
        var files = Directory.GetFiles(pathToFolder);
        return files;
    }

    /// <summary>
    /// Maps List of type Action to List of type Person
    /// </summary>
    /// <param name="actionRectords">List of type action</param>
    /// <returns></returns>
    public static List<Person> MapActionToPerson(List<Action> actionRectords)
    {
        var registeredActions = actionRectords.Select(x=> new
        {
            x.EmployeeId,
            x.Date,
        }).Distinct();


        var employeesFromCompany = new List<Person>();

        foreach (var item in registeredActions)
        {


            if (!TimeSpan.TryParse(actionRectords.FirstOrDefault(x =>
                        x.EmployeeId.Equals(item.EmployeeId) && x.Date.Equals(item.Date) && x.ActionType.Equals("WY"))
                    ?.Time
                    .ToString(), out var wy))
            {
                Console.WriteLine($"Pracownik o kodzie {item.EmployeeId} nie wyszedł z roboty dnia {item.Date}");
                wy = TimeSpan.Zero;
            }

            if (!TimeSpan.TryParse(actionRectords.FirstOrDefault(x =>
                        x.EmployeeId.Equals(item.EmployeeId) && x.Date.Equals(item.Date) && x.ActionType.Equals("WE"))
                    ?.Time
                    .ToString(), out var we))
            {
                Console.WriteLine($"Pracownik o kodzie {item.EmployeeId} nie przyszedł do roboty ale z niej wyszedł dnia {item.Date}");
                we = TimeSpan.Zero;
            }
            employeesFromCompany.Add(new Person()
            {
                EmployeeId = item.EmployeeId,
                Date = item.Date,
                StartWorkTime = we, 
                FinishWorkTime = wy
            });

        }
        return employeesFromCompany;
    }
}