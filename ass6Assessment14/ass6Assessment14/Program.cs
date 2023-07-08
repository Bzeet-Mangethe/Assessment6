using System;
using System.Collections.Generic;

public class Product : IEquatable<Product>
{
    public int Id { get; set; }
    public decimal Price { get; set; }

    public bool Equals(Product other)
    {
        if (other == null) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class Inventory
{
    private Dictionary<Product, int> products;
    public decimal totalValue;

    public event EventHandler InventoryChanged;

    public Inventory()
    {
        products = new Dictionary<Product, int>();
        totalValue = 0;
    }

    public void AddProduct(Product product, int quantity)
    {
        if (products.ContainsKey(product))
        {
            products[product] += quantity;
        }
        else
        {
            products.Add(product, quantity);
        }

        UpdateTotalValue();
        OnInventoryChanged();
    }

    public void RemoveProduct(Product product)
    {
        if (products.ContainsKey(product))
        {
            products.Remove(product);
            UpdateTotalValue();
            OnInventoryChanged();
        }
    }

    public void UpdateProductQuantity(Product product, int quantity)
    {
        if (products.ContainsKey(product))
        {
            products[product] = quantity;
            UpdateTotalValue();
            OnInventoryChanged();
        }
    }

    public void UpdateTotalValue()
    {
        totalValue = 0;
        foreach (var kvp in products)
        {
            totalValue += kvp.Key.Price * kvp.Value;
        }
    }

    private void OnInventoryChanged()
    {
        InventoryChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Create some products
        Product product1 = new Product { Id = 1, Price = 10.99m };
        Product product2 = new Product { Id = 2, Price = 24.99m };
        Product product3 = new Product { Id = 3, Price = 7.99m };

        // Create an inventory
        Inventory inventory = new Inventory();
        inventory.InventoryChanged += Inventory_InventoryChanged;

        // Add products to the inventory
        inventory.AddProduct(product1, 5);
        inventory.AddProduct(product2, 10);
        inventory.AddProduct(product3, 3);

        // Update product quantity
        inventory.UpdateProductQuantity(product2, 15);

        // Remove a product
        inventory.RemoveProduct(product1);
    }

    private static void Inventory_InventoryChanged(object sender, EventArgs e)
    {
        Inventory inventory = (Inventory)sender;
        Console.WriteLine("Inventory changed. Total value: " + inventory.totalValue);
    }
}
