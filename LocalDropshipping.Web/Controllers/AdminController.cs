﻿using Humanizer;
using LocalDropshipping.Web.Attributes;
using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Extensions;
using LocalDropshipping.Web.Helpers;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LocalDropshipping.Web.Controllers
{
    public class AdminController : Controller
    {
        public ICategoryService CategoryService { get; }

        private readonly IAdminService _service;
        private readonly IProductsService _productsService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LocalDropshippingContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly IConsumerService _consumerService;
        private readonly IWithdrawlsService _withdrawlsService;
        private readonly IProfilesService _profilesService;

        public AdminController(IAdminService service, IProductsService productsService, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, LocalDropshippingContext context, ICategoryService categoryService, IAccountService accountService, IOrderService orderService, IConsumerService consumerService,IWithdrawlsService withdrawlsService,IProfilesService profilesService)
        {
            _service = service;
            _productsService = productsService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _categoryService = categoryService;
            _orderService = orderService;
            _consumerService = consumerService;
            _withdrawlsService = withdrawlsService;
            _profilesService = profilesService;
            CategoryService = _categoryService;
            _accountService = accountService;
            _orderService = orderService;
        }



        #region Admin Login
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {


            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _service.AdminLoginUser(model.Email, model.Password);

                if (result.Succeeded)
                {
                    // Check if the user is an admin
                    bool isAdmin = await _service.IsUserAdminAsync(model.Email);

                    // Check if the user is superadmin
                    bool isSuperAdmin = await _service.IsUserSuperAdminAsync(model.Email);

                    // Check if the user is active
                    bool isActive = await _service.IsUserActiveAsync(model.Email);

                    if (isAdmin || isSuperAdmin)
                    {
                        if (isActive)
                        {
                            // Redirect to the admin dashboard if the user is an admin active
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Your account is not active.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Oops, it seems like you're not an admin.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }


            return View(model);
        }

        public IActionResult AddNewUser()
        {
            try
            {
                string? currentUserID = _userManager.GetUserId(HttpContext.User);
                var currentUser = _userService.GetById(currentUserID);
                bool isAdmin = currentUser.IsAdmin;
                bool isSuperAdmin = currentUser.IsSuperAdmin;

                UserViewModel model = new UserViewModel();

                if (isAdmin)
                {
                    model.IsSeller = true;
                }


                TempData["IsAdmin"] = isAdmin;
                TempData["IsSuperAdmin"] = isSuperAdmin;

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later!!!";
                return RedirectToAction("AdminLogin", "Admin");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check Line 137 To 144: In this line, I am getting FullName Using Email & UserName 
                    string[] emailParts = model.Email.Split('@');
                    string emailUsername = emailParts.Length > 0 ? emailParts[0] : string.Empty;
                    string[] usernameWords = model.UserName.Split(' ');
                    emailUsername = string.Join(" ", emailUsername.Split(' ').Except(usernameWords));
                    emailUsername = emailUsername.Trim();
                    User user = new User
                    {
                        Fullname = emailUsername,
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        IsAdmin = model.IsAdmin,
                        IsSeller = model.IsSeller,
                        IsActive = true,
                    };

                    // Use the AccountService to register the user and send the email
                    var result = await _accountService.RegisterAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        TempData["RegistrationConfirmation"] = "Registration was successful. You can now log in.";
                        HttpContext.Session.SetString("AdminSideVerificationEmailSent", "True");
                        return RedirectToAction("AdminSideVerificationEmailSent");
                    }
                    if (result.Succeeded)
                    {

                        _userService.Add(user);

                        return RedirectToAction("StaffMember", "Admin");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                SetRoleByCurrentUser();

                return View(model);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return View();
            }
        }

        #region AdminSideEmailVerification

        public IActionResult AdminSideVerificationEmailSent()
        {
            return View();
        }

        #endregion
        public IActionResult StaffMember([FromQuery] Pagination pagination)
        {
            SetRoleByCurrentUser();
            var staffMembers = _userService.GetAllStaffMember();
            var count = staffMembers.Count();
            staffMembers = staffMembers.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
            return View(new PageResponse<List<User>>(staffMembers, pagination.PageNumber, pagination.PageSize, count));
        }
        public IActionResult EditUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            _userService.Delete(userId);
            SetRoleByCurrentUser();
            return View("GetAllSellers", _userService.GetAll());
        }

        [HttpPost]
        public IActionResult ActivateUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                _userService.ActivateUser(userId);
            }

            var sellers = _userService.GetAll();


            SetRoleByCurrentUser();

            return View("StaffMember", sellers);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("AdminLogin", "Admin");
        }


        public IActionResult GetAllSellers([FromQuery] Pagination pagination, string searchString, string sortByName, string currentFilter)
        {
            SetRoleByCurrentUser();
            ViewBag.CurrentSort = sortByName;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortByName) ? "name_asc" : (sortByName == "name_asc" ? "name_desc" : "name_asc");

            if (searchString != null)
            {
                pagination.PageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var sellers = _userService.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                sellers = sellers.Where(x => x.Fullname.ToLower().Contains(searchString.ToLower())
                || x.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }
            switch (sortByName)
            {
                case "name_asc":
                    sellers = sellers.OrderBy(s => s.Fullname).ToList();
                    break;
                case "name_desc":
                    sellers = sellers.OrderByDescending(s => s.Fullname).ToList();
                    break;

                default:
                    sellers = sellers.OrderBy(s => s.Id).ToList();
                    break;
            }
            var count = sellers.Count();
            sellers = sellers.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
            return View(new PageResponse<List<User>>(sellers, pagination.PageNumber, pagination.PageSize, count));
        }


        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.Admin | Roles.SuperAdmin, "AdminLogin", "Admin")]
        public IActionResult Dashboard()
        {
            SetRoleByCurrentUser();
            return View();
        }



        [HttpGet]
        public IActionResult OrdersList([FromQuery] Pagination pagination, string searchString, string sortOrder, string currentFilter)
        {
            try
            {
                SetRoleByCurrentUser();
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_asc" : (sortOrder == "name_asc" ? "name_desc" : "name_asc");
                ViewBag.PriceSortParm = string.IsNullOrEmpty(sortOrder) ? "price_asc" : (sortOrder == "price_asc" ? "price_desc" : "price_asc");
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                if (searchString != null)
                {
                    pagination.PageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                List<Order> orders = _orderService.GetAll();
                if (!string.IsNullOrEmpty(searchString))
                {
                    if(Enum.TryParse<OrderStatus>(searchString.ApplyCase(LetterCasing.Sentence), out OrderStatus status))
                    orders = orders.Where(x => x.OrderStatus == status).ToList();
                }
                switch (sortOrder)
                {
                    case "name_asc":
                        orders = orders.OrderBy(s => s.OrderStatus).ToList();
                        break;
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.OrderStatus).ToList();
                        break;
                    case "Date":
                        orders = orders.OrderBy(s => s.OrderDate).ToList();
                        break;
                    case "date_desc":
                        orders = orders.OrderByDescending(s => s.OrderDate).ToList();
                        break;
                    case "price_asc":
                        orders = orders.OrderBy(s => s.GrandTotal).ToList();
                        break;
                    case "price_desc":
                        orders = orders.OrderByDescending(s => s.GrandTotal).ToList();
                        break;

                    default:
                        orders = orders.OrderBy(s => s.Id).ToList();
                        break;
                }
                
                var count = orders.Count();
                orders = orders.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
                return View(new PageResponse<List<Order>>(orders, pagination.PageNumber, pagination.PageSize, count));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        #endregion

        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult Products([FromQuery] Pagination pagination, string searchString, string sortProduct, string currentFilter)
        {
            //Add ViewBag to save SortOrder of table
            ViewBag.CurrentSort = sortProduct;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortProduct) ? "name_asc" : (sortProduct == "name_asc" ? "name_desc" : "name_asc");
            ViewBag.PriceSortParm = string.IsNullOrEmpty(sortProduct) ? "price_asc" : (sortProduct == "price_asc" ? "price_desc" : "price_asc");

            if (searchString != null)
            {
                pagination.PageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            List<Product> data = _productsService.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) ||
                x.Description.ToLower().Contains(searchString.ToLower()) ||
                 x.Category.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
            switch (sortProduct)
            {
                case "name_asc":
                    data = data.OrderBy(s => s.Name).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(s => s.Name).ToList();
                    break;
                case "price_asc":
                    data = data.OrderBy(s => s.Variants.FirstOrDefault().VariantPrice).ToList();
                    break;
                case "price_desc":
                    data = data.OrderByDescending(s => s.Variants.FirstOrDefault().VariantPrice).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.ProductId).ToList();
                    break;
            }
            var count = data.Count();
            data = data.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
            return View(new PageResponse<List<Product>>(data, pagination.PageNumber, pagination.PageSize, count));
        }
        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult AddUpdateProduct(int id = 0)
        {
            SetRoleByCurrentUser();
            ViewBag.Categories = _categoryService.GetAll();
            var productVeiwModel = new ProductViewModel(_productsService.GetById(id));
            return View(productVeiwModel);
        }


        [HttpPost]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult AddUpdateProduct(ProductViewModel model)
        {
            SetRoleByCurrentUser();
            ModelState.Remove("ProductId");

            // TODO: ValidateModel Here
            if (model.HasVariants == 0)
            {
                // Prouct without variants

            }
            else
            {
                // Prouct with variants

            }


            var form = Request.Form;
            var formFiles = form.Files;
            if (model.ProductId == 0)
            {
                if (model.HasVariants == 0)
                {
                    // Product without variants
                    var featuredImage = formFiles["featuredImage"]!;
                    var featureImageLink = featuredImage.SaveTo("images/products", model.Name!);

                    var otherImages = formFiles.GetFiles("otherImages")!.ToArray();
                    var otherImagesLinks = otherImages.SaveTo("images/products", model.Name + " thumbnails");

                    var productVideos = formFiles.GetFiles("productVideos")!.ToArray();
                    var videoLinks = productVideos.SaveTo("videos/products", model.Name);

                    var product = new Product()
                    {
                        Name = model.Name,
                        CategoryId = model.CategoryId,
                        IsBestSelling = model.IsBestSelling,
                        IsFeatured = model.IsFeatured,
                        IsNewArravial = model.IsNewArravial,
                        Description = model.Description,
                        SKU = model.SKU,
                        Variants = new List<ProductVariant>
                        {
                            new ProductVariant
                            {
                                IsMainVariant = true,
                                VariantPrice = model.Price,
                                FeatureImageLink = featureImageLink,
                                Images = otherImagesLinks.Select(imageLink => new ProductVariantImage { Link = imageLink }).ToList(),
                                Videos = videoLinks.Select(videoLink => new ProductVariantVideo { Link = videoLink }).ToList(),
                                Quantity = model.Quantity
                            }
                        },
                    };

                    _productsService.Add(product);
                    TempData["ProductAdded"] = "Product added successfully";
                    return RedirectToAction("Products");
                }
                else
                {
                    // Product with variants
                    var product = new Product()
                    {
                        Name = model.Name,
                        CategoryId = model.CategoryId,
                        IsBestSelling = model.IsBestSelling,
                        IsFeatured = model.IsFeatured,
                        IsNewArravial = model.IsNewArravial,
                        Description = model.Description,
                        SKU = model.SKU,
                        Variants = new List<ProductVariant>(),
                    };

                    for (int variantNo = 1; variantNo <= model.VariantCounts; variantNo++)
                    {
                        product.Variants.Add(new ProductVariant
                        {
                            VariantType = form["variant-type"],
                            Variant = form["variant-" + variantNo + "-value"],
                            VariantPrice = Convert.ToInt32(form["variant-" + variantNo + "-price"]),
                            FeatureImageLink = formFiles["variant-" + variantNo + "-feature-image"]!.SaveTo("images/products", model.Name + " " + form["variant-type"]),
                            Images = formFiles.GetFiles("variant-" + variantNo + "-images").ToArray().SaveTo("images/products", model.Name + " " + form["variant-type"]).Select(x => new ProductVariantImage { Link = x }).ToList(),
                            Videos = formFiles.GetFiles("variant-" + variantNo + "-videos").ToArray().SaveTo("videos/products", model.Name + " " + form["variant-type"]).Select(x => new ProductVariantVideo { Link = x }).ToList(),
                            Quantity = Convert.ToInt32(form["variant-" + variantNo + "-quantity"]),
                            IsMainVariant = false
                        });
                    }

                    _productsService.Add(product);
                    TempData["ProductAdded"] = "Product added successfully";
                    return RedirectToAction("Products");
                }

            }
            else
            {
                var product = new Product();
                product.Name = model.Name;
                product.CategoryId = model.CategoryId;
                product.IsBestSelling = model.IsBestSelling;
                product.IsFeatured = model.IsFeatured;
                product.IsNewArravial = model.IsNewArravial;
                product.Description = model.Description;
                product.SKU = model.SKU;
                product.Variants = new List<ProductVariant>();

                if (model.HasVariants == 1)
                {
                    for (int variantNo = 1; variantNo <= model.VariantCounts; variantNo++)
                    {
                        product.Variants.Add(new ProductVariant
                        {
                            ProductVariantId = Convert.ToInt32(form["variant-" + variantNo + "-variant-id"]),
                            VariantType = form["variant-type"],
                            Variant = form["variant-" + variantNo + "-value"],
                            VariantPrice = Convert.ToInt32(form["variant-" + variantNo + "-price"]),
                            Quantity = Convert.ToInt32(form["variant-" + variantNo + "-quantity"]),
                            IsMainVariant = false
                        });
                    }
                    _productsService.Update(model.ProductId, product, false);
                    TempData["updated"] = "Product updated successfully";
                }
                else
                {
                    product.Variants.Add(new ProductVariant
                    {
                        ProductVariantId = model.MainVariantId,
                        IsMainVariant = true,
                        VariantPrice = model.Price,
                        Quantity = model.Quantity
                    });
                    _productsService.Update(model.ProductId, product);
                    TempData["updated"] = "Product updated successfully";
                }

                return RedirectToAction("Products");
            }

            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
       
        public IActionResult Withdrawal()
        {
            try
            {
                var withdrawals = _withdrawlsService.GetAll();
                var profiles = _profilesService.GetAllProfiles();

                var combinedData = from withdrawal in withdrawals
                                   join profile in profiles
                                   on withdrawal.UserEmail equals profile.User.Email into joinedData
                                   from profileData in joinedData.DefaultIfEmpty() // Left join
                                   select new AddWithdrawalUserViewModel
                                   {
                                       WithDrawalId = withdrawal.WithdrawalId,
                                       UserEmail = withdrawal.UserEmail,
                                       AmountInPkr = withdrawal.AmountInPkr,
                                       paymentStatus = withdrawal.paymentStatus,
                                       ProcessedBy = withdrawal.ProcessedBy,
                                       CreatedDate = withdrawal.CreatedDate,
                                       AccountTitle = withdrawal.AccountTitle,
                                       BankAccountNumberOrIBAN = profileData?.BankAccountNumberOrIBAN, // Use null conditional operator
                                       BankName = profileData?.BankName, // Use null conditional operator
                                       Withdrawals = new List<Withdrawals> { withdrawal },
                                       Profiles = profileData != null ? new List<Profiles> { profileData } : new List<Profiles>()
                                   };

                return View(combinedData.ToList());
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Withdrawal(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var email = _userService.GetUserEmailById(userId);
                model.UpdatedBy = email;
                model.ProcessedBy = email;
                model.UpdatedBy = email;
                var result = _withdrawlsService.UpdateWithDrawal(model);
                if (result != null) return RedirectToAction("Withdrawal");
                return RedirectToAction("Withdrawal");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                SetRoleByCurrentUser();
                Product product = _productsService.Delete(id);
                TempData["Message"] = "Product deleted successfully.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Message"] = "no";
            }
            return RedirectToAction("Products");
        }

        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult Product(int id)
        {
            Product? product = _productsService.GetById(id);
            if (product != null)
            {
                return View(product);
            }

            TempData["Message"] = "Product does not exist.";
            return RedirectToAction("Products");
        }


        private void SetRoleByCurrentUser()
        {
            string? currentUserID = _userManager.GetUserId(HttpContext.User);
            User? currentUser = _userService.GetById(currentUserID);
            TempData["IsAdmin"] = currentUser.IsAdmin;
            TempData["IsSuperAdmin"] = currentUser.IsSuperAdmin;
        }
        //private void GetCurrentLoggedInUser()
        //{
        //    string? currentUserID = _userManager.GetUserId(HttpContext.User);
        //    var currentUser = _userService.GetById(currentUserID);
        //    bool isAdmin = currentUser.IsAdmin;
        //    bool isSuperAdmin = currentUser.IsSuperAdmin;
        //}
        private string GetCurrentLoggedInUserEmail()
        {
            string? currentUserID = _userManager.GetUserId(HttpContext.User);
            var currentUser = _userService.GetById(currentUserID);
            string currentUserEmail = currentUser.Email;

            return currentUserEmail;
        }

        public IActionResult CategoryList([FromQuery] Pagination pagination, string searchString, string sortByName, string currentFilter)
        {
            ViewBag.CurrentSort = sortByName;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortByName) ? "name_asc" : (sortByName == "name_asc" ? "name_desc" : "name_asc");
            if (searchString != null)
            {
                pagination.PageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            List<Category> category = _categoryService.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                category = category.Where(c => c.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
            switch (sortByName)
            {
                case "name_asc":
                    category = category.OrderBy(s => s.Name).ToList();
                    break;
                case "name_desc":
                    category = category.OrderByDescending(s => s.Name).ToList();
                    break;

                default:
                    category = category.OrderBy(s => s.CategoryId).ToList();
                    break;
            }
            var count = category.Count();
            category = category.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
            return View(new PageResponse<List<Category>>(category, pagination.PageNumber, pagination.PageSize, count));
        }

        public IActionResult AddNewCategory()
        {
            SetRoleByCurrentUser();
            return View();
        }

        [HttpPost]
        public IActionResult AddNewCategory(Category categoryModel)
        {
            SetRoleByCurrentUser();

            if (ModelState.IsValid)
            {
                var createdBy = GetCurrentLoggedInUserEmail();
                var category = new Category
                {
                    Name = categoryModel.Name,
                    ImagePath = categoryModel.ImagePath,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy,
                    ModifiedDate = DateTime.Today,
                    ModifiedBy = createdBy,
                    IsActive = true,
                    IsDeleted = false

                };
                _categoryService.Add(category);
                RedirectToAction("CategoryList", "Admin");
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            SetRoleByCurrentUser();
            var user = _categoryService.Delete(id);
            SetRoleByCurrentUser();
            return View("CategoryList", _categoryService.GetAll());
        }
        [HttpGet]
        public IActionResult UpdateCategory()//int id
        {
            SetRoleByCurrentUser();
            return View();
        }

        [HttpPost]
        public IActionResult UpdateCategory(int categoryId, CategoryDto categoryDto)
        {
            SetRoleByCurrentUser();
            if (ModelState.IsValid)
            {
                var createdBy = GetCurrentLoggedInUserEmail();
                var category = new CategoryDto
                {
                    Name = categoryDto.Name,
                    ImagePath = categoryDto.ImagePath,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy,
                    ModifiedDate = DateTime.Today,
                    ModifiedBy = createdBy,
                    IsActive = true,
                    IsDeleted = false
                };
                if (category != null)
                {
                    _categoryService.Update(categoryId, categoryDto);
                }

                RedirectToAction("CategoryList", _categoryService.GetAll());
            }
            return View();
        }

        public IActionResult GetAllConsumers([FromQuery] Pagination pagination, string searchString, string sortByName, string currentFilter)
        {
            SetRoleByCurrentUser();
            ViewBag.CurrentSort = sortByName;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortByName) ? "name_asc" : (sortByName == "name_asc" ? "name_desc" : "name_asc");
            if (searchString != null)
            {
                pagination.PageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var consumers = _consumerService.GetAllConsumer();
            if (!string.IsNullOrEmpty(searchString))
            {
                consumers = consumers.Where(c => c.FullName.ToLower().Contains(searchString.ToLower())
                || c.PrimaryPhoneNumber.Contains(searchString)
                || c.PrimaryPhoneNumber.Contains(searchString)).ToList();
            }
            switch (sortByName)
            {
                case "name_asc":
                    consumers = consumers.OrderBy(s => s.FullName).ToList();
                    break;
                case "name_desc":
                    consumers = consumers.OrderByDescending(s => s.FullName).ToList();
                    break;

                default:
                    consumers = consumers.ToList();
                    break;
            }
            var count = consumers.Count();
            consumers = consumers.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
            return View(new PageResponse<List<Consumer>>(consumers, pagination.PageNumber, pagination.PageSize, count));
        }

        [HttpPost]
        public IActionResult BlockOrUnblockConsumer(int userId)
        {
            SetRoleByCurrentUser();
            var consumer = _consumerService.BlockOrUnblockConsumer(userId);
            var consumers = _consumerService.GetAllConsumer();
            SetRoleByCurrentUser();
            return View("GetAllConsumers", consumers);
        }

        public IActionResult Reports()
        {
            SetRoleByCurrentUser();
            return View();
        }
    }
}