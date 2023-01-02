namespace zadanie.Model;

public class Person
{
    public string EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartWorkTime { get; set; }
    public TimeSpan FinishWorkTime { get; set; }
}