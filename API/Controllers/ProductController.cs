using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class ProductController : Controller
    {
        private Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }
        
        //Get products
        [HttpGet]
        public List<Product> Get()
        {
            return _context.products.ToList();
        }

        // Details
        [HttpGet("{Id}")]
        public Product GetProduct(int Id)
        {
            var product = _context.products.Where(a => a.Id == Id).SingleOrDefault();
            return product;
        }

        //Create product
        [HttpPost]
        public IActionResult PostProduct([FromBody]Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error. Invalid model");

            _context.products.Add(product);
            _context.SaveChanges();

            return Ok();
        }

        // Edit product
        [HttpPost("{id}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if (product != null)
            {
                using (var scope = new TransactionScope())
                {
                    _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    scope.Complete();
                    return new OkResult();
                }
            }

            return NoContent();
        }

        //Delete product
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.products.FindAsync(id);

            if (product == null )
            {
                return NotFound();
            }

            _context.products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool productExists(long id) =>
      _context.products.Any(e => e.Id == id);
    }
}