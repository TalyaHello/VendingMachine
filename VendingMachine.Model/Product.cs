using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Model
{
    public class Product
    {
        public static IReadOnlyList<Product> Products = new List<Product>() {
            new Product("Coffee", 12),
            new Product("Coffee with milk", 25),
            new Product("Tea", 6),
            new Product("Chips", 30),
            new Product("Milky way", 32),
            new Product("Some", 40),
        };
        private Product(string name, int price) {
            Name = name;
            Price = price;
        }
        public string Name { get; }
        public int Price { get; }
    }
}
