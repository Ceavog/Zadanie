using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

Console.WriteLine("Rejestracja Czasu Pracy");

var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = false,
    Delimiter = ";", 
    Encoding = Encoding.UTF8
};
string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var path1 = Path.Combine(sCurrentDirectory, @"..\..\..\csv\rcp1.csv");
var path2 = Path.Combine(sCurrentDirectory, @"..\..\..\csv\rcp2.CSV");

string fullPath1 = Path.GetFullPath(path1);
string fullPath2 = Path.GetFullPath(path2);

using var reader = new StreamReader(fullPath1);
using var csv = new CsvReader(reader, configuration);

var records = csv.GetRecords<Person>();


using var reader2 = new StreamReader(fullPath2);
using var csv2 = new CsvReader(reader2, configuration);
var records2 = csv2.GetRecords<Action>().ToList();

var WejsciaPracownikow = records2.Select(x=> new
{
    x.KodPracownika,
    x.Data,
}).Distinct();


var pracownicyFirma2 = new List<Person>();

foreach (var item in WejsciaPracownikow)
{


    if (!TimeSpan.TryParse(records2.FirstOrDefault(x =>
                x.KodPracownika.Equals(item.KodPracownika) && x.Data.Equals(item.Data) && x.TypAkcji.Equals("WY"))
            ?.Godzina
            .ToString(), out var wy))
    {
        Console.WriteLine($"Pracownik o kodzie {item.KodPracownika} nie wyszedł z roboty dnia {item.Data}");
        wy = TimeSpan.Zero;
    }

    if (!TimeSpan.TryParse(records2.FirstOrDefault(x =>
                x.KodPracownika.Equals(item.KodPracownika) && x.Data.Equals(item.Data) && x.TypAkcji.Equals("WE"))
            ?.Godzina
            .ToString(), out var we))
    {
        Console.WriteLine($"Pracownik o kodzie {item.KodPracownika} nie przyszedł do roboty ale z niej wyszedł dnia {item.Data}");
        we = TimeSpan.Zero;
    }
    pracownicyFirma2.Add(new Person()
        {
            KodPracownika = item.KodPracownika,
            Data = item.Data,
            GodzinaWejscia = we, 
            GodzinaWyjscia = wy
        });
 
}

Console.WriteLine("załadowano");

Console.ReadKey();
class Person
{
    public string KodPracownika { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan GodzinaWejscia { get; set; }
    public TimeSpan GodzinaWyjscia { get; set; }
}

class Action
{
    public string KodPracownika { get; set; }
    public DateTime Data { get; set; }
    public string Godzina { get; set; }
    public string TypAkcji { get; set; }
}