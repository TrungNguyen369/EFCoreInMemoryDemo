using EFCoreInMemoryDemo.DatabaseContext;
using EFCoreInMemoryDemo.DataModel;
using EFCoreInMemoryDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInMemoryDemo.Controllers
{
    
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly MyDatabaseContext _dbcontext;
        private readonly ILogger<ProductController> _logger;


        public ProductController(MyDatabaseContext dbcontext, ILogger<ProductController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetProductList")]
        public async Task<IActionResult> GetProductList()
        {
            _logger.LogInformation("This is an information log.");
            return Ok(_dbcontext.Products.ToList());
        }

        [HttpPost]
        [Route("PostProduct")]
        public async Task<IActionResult> PostProduct(ProductModel obj)
        {
            ProductDataModel product = new ProductDataModel();
            product.Id = Guid.NewGuid();
            product.ProductName = obj.ProductName;
            product.Category = obj.Category;
            product.Price = obj.Price;

            _dbcontext.Products.Add(product);
            _dbcontext.SaveChanges();

            return Ok(product);
        }


        [HttpPut]
        [Route("PutProduct")]
        public async Task<IActionResult> PutProduct(ProductModel obj)
        {

            var product = _dbcontext.Products.Where(e => e.Id == obj.Id).FirstOrDefault();

            product.ProductName = obj.ProductName;
            product.Category = obj.Category;
            product.Price = obj.Price;

            _dbcontext.Products.Update(product);
            _dbcontext.SaveChanges();

            return Ok(product);
        }

        [HttpGet]
        [Route("GetProductHistory")]
        public async Task<IActionResult> GetProductHistory()
        {
            return Ok(_dbcontext.Products.TemporalAll().ToList());
        }
    }
}
