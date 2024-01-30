using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.ProdDAL;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProdDBContext _dbContext;

        public ProductController(ProdDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var product = _dbContext.Products.ToList();
                if (product == null)
                {
                    return NotFound("Products not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("id")]
        public IActionResult Get(int Id)
        {
            try
            {
                var product = _dbContext.Products.Find(Id);
                if (product == null)
                {
                    return NotFound("Products not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



    }
}
