using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using zadanie;
using zadanie.Model;
using Action = zadanie.Model.Action;

Console.WriteLine("Rejestracja Czasu Pracy");


var files = Helper.PathByFolderName("csv");

var recordsFromFile1 = Helper.ReadCsvFile<Person>(files[0],";");
var recordsFromFile2 = Helper.ReadCsvFile<Action>(files[1], ";");

var employeesFromCompany2 = Helper.MapActionToPerson(recordsFromFile2);

Console.WriteLine("załadowano");

Console.ReadKey();


