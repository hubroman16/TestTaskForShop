
public class Program
{
    public static void Main()
    {
        string connectionString = "Host=localhost;Username=postgres;Password=15127628;Database=TestDB";
        IOrderView shelfView = new OrderView();
        OrderController shelfController = new OrderController(connectionString, shelfView);

        Console.Write("Введите номера заказов через запятую:\t");
        try
        {
            string[] arr = Console.ReadLine().Split(',');
            int[] req = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                req[i] = int.Parse(arr[i]);
            }
            shelfController.PrintShelfInfo(req);
        }
        catch (Exception)
        {
            Console.WriteLine("Неправильный ввод");
        }
    }
}
