using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enum;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IProductsService productService;
        private readonly ICategoryService categoryService;
        private readonly ISubscriptionsService sub;

        public AdminController(IProductsService productService, ICategoryService categoryService, ISubscriptionsService Sub)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            sub = Sub;
        }

        #region Product
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(productService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            return Ok(productService.GetById(id));
        }

        [HttpPost]
        public IActionResult PostProduct(ProductDto productDto)
        {
            Product product = productDto.ToEntity();
            productService.Add(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            productService.Delete(id);
            return Ok();
        }

        [HttpPost("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            Product product = productDto.ToEntity();
            productService.Update(id, productDto);
            return Ok();
        }
        [HttpGet]
        public IActionResult GetProductbyPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = productService.GetProductsByPriceRange(minPrice, maxPrice);
            if (products.Count == 0)
            {
                return NotFound("No Products Available");
            }
            return Ok(products);
        }
        #endregion

        #region Category
        [HttpGet]
        public IActionResult GetAllCategories()

        {
            return Ok(categoryService.GetAll());
        }


        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            return Ok(categoryService.GetById(id));
        }



        [HttpPost]
        public IActionResult AddCategory(CategoryDto categoryDto)
        {
            Category category = categoryDto.ToEntity();
            categoryService.Add(category);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            categoryService.Delete(id)
;
            return Ok();
        }
        [HttpPost("{id}")]
        public IActionResult UpdateCategory(int id, CategoryDto categoryDto)
        {
            Category category = categoryDto.ToEntity();
            categoryService.Update(id, categoryDto);
            return Ok();
        }
        #endregion

        #region Subscriptions Todo(Zubair)
        [HttpGet]
        public IActionResult GetAllSubscriptions()

        {
            return Ok(sub.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetSubscriptionById(int id)
        {
            return Ok(sub.GetById(id));
        }
        [HttpPost]
        public IActionResult AddSubscription(SubscriptionsDto subscriptionsDt)
        {
            Subscription Subb = subscriptionsDt.ToEntity();
            sub.Add(Subb);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteSubscription(int id)
        {
            sub.Delete(id)
;
            return Ok();
        }
        [HttpPost("{id}")]
        public IActionResult UpdatePaymentStatus(int id, PaymentStatus newPaymentStatus)
        {
            var updatedPStatus = sub.UpdatePaymentStatus(id, newPaymentStatus);

            if (updatedPStatus != null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{id}")]
        public IActionResult UpdateSubscriptionStatus(int id)
        {
            var updatedMembership = sub.UpdateSubscriptionStatus(id);

            if (updatedMembership != null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

    }
}
