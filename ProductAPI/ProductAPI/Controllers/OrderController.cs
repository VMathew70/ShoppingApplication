using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.ProdDAL;
using System.Collections.Generic;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ProdDBContext _dbContext;

        public OrderController(ProdDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var orders = _dbContext.OrderHeader.ToList();
            if (orders == null)
            {
                return NotFound("Orders not found");
            }
            return Ok(orders);
        }


        [HttpPost]
        public IActionResult Post(List<CartItem> model)
        {
            try
            {
                OrderHeader orderHeader = new OrderHeader();
                orderHeader.customername = model[0].customername;
                orderHeader.email = model[0].email;
                orderHeader.ordernumber = model[0].ordernumber;
                _dbContext.Add(orderHeader);
                _dbContext.SaveChanges();

                foreach (CartItem item in model)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.productid = item.ProductId;
                    orderDetail.ordernumber = item.ordernumber;
                    orderDetail.orderquantity = item.Quantity;
                    orderDetail.price = item.Price;
                    _dbContext.Add(orderDetail);
                    _dbContext.SaveChanges();

                    //Update Product qty
                    var product = _dbContext.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        product.quantity = product.quantity - item.Quantity;
                        _dbContext.Products.Update(product);
                        _dbContext.SaveChanges();
                    }
                }

                
                return Ok("Order Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public IActionResult Post(Order model)
        //{
        //    try
        //    {
        //        _dbContext.Add(model);
        //        _dbContext.SaveChanges();

        //        var product = _dbContext.Products.Find(model.productid);
        //        if (product != null)
        //        {
        //            product.quantity = product.quantity - model.orderquantity;
        //            _dbContext.Products.Update(product);
        //            _dbContext.SaveChanges();
        //        }
        //        return Ok("Order Created");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
