using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Параллельное выполнение
        var task1 = CalculateAsync();
        var task2 = CalculateAsync();
        await Task.WhenAll(task1, task2);
        Console.WriteLine(task1.Result + task2.Result); // Вывод: 84
    }
    public static async Task<int> CalculateAsync()
    {
        return await Task.Run(() => {
            // Долгая операция
            Thread.Sleep(1000);
            return 42;
        });
    }

    

}
public class Person
{
    public string Name { get; set; }
    public int BirthYear { get; set; }
    public string Country { get; set; }
}