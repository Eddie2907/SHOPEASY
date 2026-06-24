using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ProductService ps;

        public ProductController(ProductService ps1)
        {
            ps = ps1;
        }

        [HttpGet]

        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            var products = ps.GetAllProduct();
            return Ok(products);
        }

        [HttpGet("{productId}")]

        public ActionResult GetProductById(int productId)
        {
            Product product  = ps.GetProductById(productId);
            if(product== null){
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]

        public ActionResult CreateProduct (Product newProduct)
        {
            if(newProduct == null)
            {
                return NotFound();
            }
            ps.AddProduct(newProduct);
            return CreatedAtAction("CreateProduct", newProduct);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            ps.UpdateProduct(id, updatedProduct);
            return NoContent();

            return NotFound();
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteProduct(int id)
        {
          ps.DeleteProduct(id);
          return NoContent();
        }
        
    }
}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using dotnetapp.Models;

namespace dotnetapp.Services
{
    public class ProductService
    {
        public static List<Product> _products = new List<Product>()
        {
            new Product {Id = 1, Name = "Product 1", Price = 10.99m, Description = "Destination 1"},
            new Product {Id = 2, Name = "Product 2", Price = 20.49m, Description = "Destination 2"},
            new Product {Id = 3, Name = "Product 3", Price = 15.99m, Description = "Destination 3"}
        };

        public IEnumerable<Product> GetAllProduct()
        {
            return _products;
        }

        public Product GetProductById (int ProductId)
        {
            return _products.FirstOrDefault(p => p.Id == ProductId);

        }

        public void AddProduct(Product newProduct)
        {
            _products.Add(newProduct);
            
        }

        public void UpdateProduct (int id, Product updatedProduct)
        {
            var product = _products.Find( p => p.Id == id);

            product.Name = updatedProduct.Name;
             product.Price = updatedProduct.Price;
              product.Description = updatedProduct.Description;

        }

        public void DeleteProduct(int id)
        {
            var product = _products.Find(p => p.Id ==id);

            _products.Remove(product);
        }
    }
}

program.cs
using dotnetapp.Models;
using dotnetapp.Services;
var builder = WebApplication.CreateBuilder(args);

// Add Event services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetapp.Models
{
    public class Product
    {
        public int Id {get; set;}

        public string Name {get; set;}

        public decimal Price {get; set;}

        public string Description {get; set;} 
    }
}
