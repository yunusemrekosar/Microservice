using CatalogService.Api.Core.Application;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {

        private readonly CatalogDbContext _context;

        public CatalogController(CatalogDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
        }

        // GET: api/<ItemsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("/api/catalog/items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Item>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Item>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {

            var totalItems = await _context.Items
                .LongCountAsync();

            var itemsOnPage = await _context.Items
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<Item>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }


        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Item), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Item>> ItemByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest();

            var item = await _context.Items.SingleOrDefaultAsync(ci => ci.Id == id);
 

            if (item != null)
                return item;
            
            return NotFound();
        }

        [HttpGet]
        [Route("ItemCategories")]
        [ProducesResponseType(typeof(List<ItemCategory>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ItemCategory>>> CatalogTypesAsync()
        {
            return await _context.ItemCategories.ToListAsync();
        }

        // GET api/v1/[controller]/CatalogBrands
        [HttpGet]
        [Route("ItemBrands")]
        [ProducesResponseType(typeof(List<ItemBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ItemBrand>>> CatalogBrandsAsync()
        {
            return await _context.ItemBrands.ToListAsync();
        }

        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody] Item productToUpdate)
        {
            var items = await _context.Items.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (items == null)
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });

            var oldPrice = items.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            items = productToUpdate;
            _context.Items.Update(items);

            await _context.SaveChangesAsync();

            //if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            //{
            //    //Create Integration Event to be published through the Event Bus
            //    var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);

            //    // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
            //    await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            //    // Publish through the Event Bus and mark the saved event as published
            //    await _catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            //}
            //else // Just save the updated product because the Product's Price hasn't changed.
            //{

            //}

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody] Item product)
        {
            var item = new Item
            {
                ItemBrandId = product.ItemBrandId,
                ItemCategoryId = product.ItemCategoryId,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price
            };

            _context.Items.Add(item);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = _context.ItemCategories.SingleOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            _context.ItemCategories.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
