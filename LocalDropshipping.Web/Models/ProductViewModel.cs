using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsNewArravial { get; set; }

        public bool IsBestSelling { get; set; }

        public bool IsFeatured { get; set; }

        public int Quantity { get; set; }

        public string? SKU { get; set; }


        [DisplayName("Category")]
        public int CategoryId { get; set; }


        [Range(0, int.MaxValue)]
        public int Price { get; set; }


        [ValidateNever]
        public int VariantCounts { get; set; } = 1;

        public ProductViewModel() { }

        public ProductViewModel(Product? product = null)
        {
            if (product != null)
            {
                ProductId = product.ProductId;
                Name = product.Name;
                Description = product.Description;
                CategoryId = product.CategoryId;
                IsNewArravial = product.IsNewArravial;
                IsBestSelling = product.IsBestSelling;
                IsFeatured = product.IsFeatured;
                SKU = product.SKU;
                var mainVariant = product.Variants.First(x => x.IsMainVariant);
                var actualVariants = product.Variants.Where(x => x.VariantType != "MAIN_VARIANT");

                //MainVariantId = mainVariant.VariantId;
                Price = mainVariant.VariantPrice;
                Quantity = mainVariant.Quantity;
                //Variants = actualVariants.Select(x => new ProductVariantViewModel
                //{
                //    VariantId = x.VariantId,
                //    VariantType = x.VariantType,
                //    VariantPrice = x.VariantPrice,
                //    Quantity = x.Quantity,
                //    FeatureImageLink = x.FeatureImageLink

                //}).ToList();
            }
        }

        public Product ToEntity()
        {
            var product = new Product();
            product.Name = Name;
            product.Description = Description;
            product.CategoryId = CategoryId;
            product.IsNewArravial = IsNewArravial;
            product.IsBestSelling = IsBestSelling;
            product.IsFeatured = IsFeatured;
            product.SKU = SKU;

            //product.Variants = new List<ProductVariant>
            //{
            //    // Main Variant
            //    new ProductVariant
            //    {
            //        VariantId = MainVariantId,
            //        CreatedDate = DateTime.Now,
            //        VariantType = "MAIN_VARIANT",
            //        VariantPrice = Price,
            //        Quantity = Quantity,
            //    }
            //};

            //// Actual Variants
            //if (Variants != null)
            //{
            //    product.Variants.AddRange(Variants.Select(x =>
            //    {
            //        return new ProductVariant
            //        {
            //            CreatedDate = DateTime.Now,
            //            VariantType = x.VariantType,
            //            VariantPrice = x.VariantPrice,
            //            Quantity = x.Quantity,
            //        };
            //    }));
            //}

            return product;
        }

    }
}
