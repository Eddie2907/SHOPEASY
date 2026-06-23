using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        public OrderService db;
 
        public OrderController(OrderService db1){
            db = db1;
        }
        [HttpGet]
        public ActionResult GetAllOrders(){
            var res = db.GetAllOrders();
            return Ok(res);
        }
        [HttpGet("{OrderId}")]
        public ActionResult GetOrderById(int orderId){
            var res = db.GetOrderById(orderId);
            if(res == null){
                return NotFound();
            }
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrder(Order obj){
            if(obj == null)
            {
                return BadRequest();
            }
            db.AddOrder(obj);
            return CreatedAtAction("AddOrder",obj);
        }
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;
 
namespace dotnetapp.Services{
    public class OrderService{
        public static List<Order> orders = new List<Order>();
 
        public List<Order> GetAllOrders(){
            return orders.ToList();
        }
        public Order GetOrderById(int id){
            return orders.Find(r => r.OrderId == id);
        }
        public void AddOrder(Order obj){
            orders.Add(obj);
        }
    }
}
 
using dotnetapp.Services;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add Event services to the container.
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<OrderService>();
builder.Services.AddSwaggerGen();
 
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
    public class Order
    {
        public int OrderId{get;set;}
        public string CustomerName{get;set;}
        public DateTime OrderDate {get;set;}
        public decimal TotalAmount{get;set;}
        public string Status{get;set;}
    }
}
