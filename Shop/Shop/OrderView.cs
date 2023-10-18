public class OrderView : IOrderView
{
    public void PrintShelfInfo(OrderModel order)
    {
        Console.Write("\n");
        Console.WriteLine("===Стеллаж {0}", order.ShelfName);
        Console.WriteLine("{0} (id={1})", order.ProductName, order.ProductId);
        Console.WriteLine("заказ {0}, {1} шт", order.OrderNumber, order.ProductQuantity);

        if (!string.IsNullOrEmpty(order.AdditionalShelves))
        {
            Console.WriteLine("доп стеллаж: {0}", order.AdditionalShelves);
        }

        Console.WriteLine();
    }
}
