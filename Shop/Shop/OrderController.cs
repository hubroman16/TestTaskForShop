using Npgsql;

public class OrderController
{
    private readonly string connectionString;
    private readonly IOrderView shelfView;

    public OrderController(string connectionString, IOrderView shelfView)
    {
        this.connectionString = connectionString;
        this.shelfView = shelfView;
    }

    public void PrintShelfInfo(int[] orderNumbers)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"
                    SELECT s.name AS shelf_name, p.name AS product_name, p.id AS product_id,
                           o.number AS order_number, pi.quantity AS product_quantity,
                           ARRAY_AGG(ps.name) AS additional_shelves
                    FROM shelf s
                    INNER JOIN product_on_shelf pos ON s.id = pos.shelf_id
                    INNER JOIN product p ON pos.product_id = p.id
                    INNER JOIN product_in_order pi ON p.id = pi.product_id
                    INNER JOIN ""order"" o ON pi.order_id = o.id
                    LEFT JOIN product_on_shelf pos2 ON p.id = pos2.product_id 
                                                      AND pos2.is_main_shelf = false
                    LEFT JOIN shelf ps ON pos2.shelf_id = ps.id
                    WHERE pos.quantity != 0 AND o.number = ANY(:orderNumbers)
                    GROUP BY s.name, p.name, p.id, o.number, pi.quantity
                    ORDER BY s.name ASC";

                command.Parameters.AddWithValue("orderNumbers", orderNumbers);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderModel order = new OrderModel();

                        order.ShelfName = reader.GetString(0);
                        order.ProductName = reader.GetString(1);
                        order.ProductId = reader.GetInt32(2);
                        order.OrderNumber = reader.GetInt32(3);
                        order.ProductQuantity = reader.GetInt32(4);

                        string[] AdditionalShelvesArray = (string[])reader.GetValue(5);
                        order.AdditionalShelves = string.Join(", ", AdditionalShelvesArray);

                        shelfView.PrintShelfInfo(order);
                    }
                }
            }
        }
    }
}
