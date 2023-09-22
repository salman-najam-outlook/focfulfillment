using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models.ProductViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsNewArravial { get; set; }

        public bool IsBestSelling { get; set; }

        public bool IsTopRated { get; set; }

        public int Quantity { get; set; }

        public int MainVariantId { get; set; }

        public string SKU { get; set; }


        [DisplayName("Category")]
        public int CategoryId { get; set; }


        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Range(0, int.MaxValue)]
        public int DiscountedPrice { get; set; }


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
                    IsTopRated = product.IsTopRated;
                    IsNewArravial = product.IsNewArravial;
                    Description = product.Description;
                    SKU = product.SKU;
                    Price = product.Variants.First().VariantPrice;
                    DiscountedPrice = product.Variants.First().DiscountedPrice;
                    Quantity = product.Variants.First().Quantity;
                    MainVariantId = product.Variants.First().ProductVariantId;
                    Variants = product.Variants.Select(x => new ProductVariantViewModel
                    {
                        Quantity = x.Quantity,
                        VariantPrice = x.VariantPrice,
                        VariantType = x.VariantType,
                        VariantId = x.ProductVariantId,
                        Variant = x.Variant,
                        DiscountedPrice = x.DiscountedPrice,
                        FeatureImageLink = x.FeatureImageLink,
                        Images = x.Images.Select(x => x.Link).ToList(),
                        Videos = x.Videos.Select(x => x.Link).ToList()
                    }).ToList();
                }
                else
                {
                    HasVariants = 1;
                    VariantCounts = product.Variants.Count;
                    Name = product.Name;
                    CategoryId = product.CategoryId;
                    IsBestSelling = product.IsBestSelling;
                    IsTopRated = product.IsTopRated;
                    IsNewArravial = product.IsNewArravial;
                    Description = product.Description;
                    SKU = product.SKU;
                    Price = product.Variants.First().VariantPrice;
                    DiscountedPrice = product.Variants.First().DiscountedPrice;
                    Variants = new List<ProductVariantViewModel>();
                    Variants.AddRange(
                        product.Variants
                        .Select(x => new ProductVariantViewModel
                        {
                            Quantity = x.Quantity,
                            VariantPrice = x.VariantPrice,
                            VariantType = x.VariantType,
                            VariantId = x.ProductVariantId,
                            Variant = x.Variant,
                            DiscountedPrice = x.DiscountedPrice,
                            FeatureImageLink = x.FeatureImageLink,
                            Images = x.Images.Select(x => x.Link).ToList(),
                            Videos = x.Videos.Select(x => x.Link).ToList()
                        }));
                }

                ProductId = product.ProductId;
            }
        }
    }
}
