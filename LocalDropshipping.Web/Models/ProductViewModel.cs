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

        [ValidateNever]
        public int HasVariants { get; set; } = 0;

        public List<ProductVariantViewModel> Variants { get; set; }


        public ProductViewModel() { }

        public ProductViewModel(Product? product = null)
        {
            if (product != null)
            {
                var hasVariants = product.Variants.Count > 1;
                if (!hasVariants)
                {
                    Name = product.Name;
                    CategoryId = product.CategoryId;
                    IsBestSelling = product.IsBestSelling;
                    IsFeatured = product.IsFeatured;
                    IsNewArravial = product.IsNewArravial;
                    Description = product.Description;
                    SKU = product.SKU;
                    Price = product.Variants.First().VariantPrice;
                }
                else
                {
                    HasVariants = 1;
                    VariantCounts = product.Variants.Count;
                    Name = product.Name;
                    CategoryId = product.CategoryId;
                    IsBestSelling = product.IsBestSelling;
                    IsFeatured = product.IsFeatured;
                    IsNewArravial = product.IsNewArravial;
                    Description = product.Description;
                    SKU = product.SKU;
                    Price = product.Variants.First().VariantPrice;
                    Variants = new List<ProductVariantViewModel>();
                    Variants.AddRange(product.Variants.Select(x => new ProductVariantViewModel { Quantity = x.Quantity, VariantPrice = x.VariantPrice, VariantType = x.VariantType, VariantId = x.ProductVariantId, Variant = x.Variant }));
                }

                ProductId = product.ProductId;
            }
        }

        public Product ToEntity()
        {
            var product = new Product();
            product.Name = Name;
            product.CategoryId = CategoryId;
            product.IsBestSelling = IsBestSelling;
            product.IsFeatured = IsFeatured;
            product.IsNewArravial = IsNewArravial;
            product.Description = Description;
            product.SKU = SKU;
            product.Variants = new List<ProductVariant>();

            if (HasVariants == 1)
            {
                product.Variants.AddRange(Variants.Select(x => new ProductVariant
                {
                    Quantity = x.Quantity,
                    VariantPrice = x.VariantPrice,
                    VariantType = x.VariantType,
                    Variant = x.Variant,
                }));
            }
            else
            {
                product.Variants.Add(new ProductVariant
                {
                    IsMainVariant = true,
                    VariantPrice = Price,
                    Quantity = Quantity
                });
            }

            return product;
        }
    }
}
