using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Bookzone.Extensions;
using Bookzone.Models.BLL;
using Bookzone.Models.Entity.Document;
using Bookzone.Models.Entity.Product;
using Bookzone.Models.Entity.User;
using Braintree;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Bookzone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBraintreeService _braintreeService;
        private readonly IToastNotification _notification;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public HomeController(IWebHostEnvironment webHostEnvironment, IToastNotification notification, IBraintreeService braintreeService)
        {
            _webHostEnvironment = webHostEnvironment;
            _notification = notification;
            _braintreeService = braintreeService;
            
        }
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetObjectFromJson<string>("organizationName")))
                return View();
            var ip = Request.HttpContext.Connection.LocalIpAddress.ToString();

            ViewBag.ipAdress = "";

            var org = BllUser.GetOrganizationByIp(ip);
            if (org == null) return View();
            HttpContext.Session.SetObjectAsJson("organizationId", org.Id);
            HttpContext.Session.SetObjectAsJson("organizationName", org.Name);

            return View();
        }
        
        #region Cart
            public IActionResult Cart()
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
                if (cart == null || !cart.Any() || User.FindFirstValue(ClaimTypes.Role) == "Editor")
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    try
                    {
                        foreach (Item VARIABLE in cart)
                        {
                            if (BllUser.ValidatePurchase(VARIABLE.Product.DocumentGroup.Id.ToString(), User.FindFirstValue(ClaimTypes.NameIdentifier)).Success)
                            {
                                var index = IsExist(VARIABLE.Product.DocumentGroup.Id.ToString());
                                cart.RemoveAt(index);
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    HttpContext.Session.SetObjectAsJson("cart", cart);
                    if (cart.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                }

                var gateway = _braintreeService.GetGateway();
                var clientToken = gateway.ClientToken.Generate(); //Genarate a token
                ViewBag.ClientToken = clientToken;

                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Product.DocumentGroup.Price * item.Quantity);

                return View("Cart/Index",cart);
            }
            [HttpGet]
            public IActionResult AddToCart(string id)
            {
                if (User.Identity.IsAuthenticated && BllUser.ValidatePurchase(id, User.FindFirstValue(ClaimTypes.NameIdentifier)).Success)
                {
                    _notification.AddInfoToastMessage("You brought this document already");
                    return Json(new {Data = new JsonResponse {Success = true, Extra = CartCount().ToString()}});
                }

                if (BllDocument.GetDocumentById("id", id).DocumentGroup.Price ==  0 && User.Identity.IsAuthenticated)
                {
                    BllUser.AddPurchase(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                    _notification.AddSuccessToastMessage("Added to your collection successfully");
                    return Json(new {Data = new JsonResponse {Success = true}});
                }

                if (BllDocument.GetDocumentById("id", id).DocumentGroup.Price == 0 && !User.Identity.IsAuthenticated)
                {
                    _notification.AddErrorToastMessage("Log in to buy it");
                    return Json(new {Data = new JsonResponse {Success = false}});            
                }
                
                if (HttpContext.Session.GetObjectFromJson<List<Item>>("cart") == null)
                {
                    var cart = new List<Item>();
                    cart.Add(new Item
                    {
                        Product = BllDocument.GetDocumentById("id", id),
                        Quantity = 1
                    });
                    _notification.AddSuccessToastMessage("Added to cart successfully");
                    HttpContext.Session.SetObjectAsJson("cart", cart);
                }
                else
                {
                    var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
                    var index = IsExist(id);
                    if (index != -1)
                    {
                        _notification.AddWarningToastMessage("Already in cart");
                    }
                    else
                    {
                        cart.Add(new Item
                        {
                            Product = BllDocument.GetDocumentById("id", id),
                            Quantity = 1
                        });
                        _notification.AddSuccessToastMessage("Added to cart successfully");
                        HttpContext.Session.SetObjectAsJson("cart", cart);
                    }
                }


                return Json(new {Data = new JsonResponse {Success = true, Extra = CartCount().ToString()}});
            }
            private int IsExist(string id)
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
                for (var i = 0; i < cart.Count; i++)
                    if (cart[i].Product.DocumentGroup.Id.Equals(int.Parse(id)))
                        return i;
                return -1;
            }
            [HttpGet]
            public int CartCount()
            {
                return HttpContext.Session.GetObjectFromJson<List<Item>>("cart")?.Count ?? 0;
            }
            [HttpGet]
            public IActionResult RemoveFromCart(string id)
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("cart");
                var index = IsExist(id);
                cart.RemoveAt(index);
                HttpContext.Session.SetObjectAsJson("cart", cart);
                return Json(new {Data = new JsonResponse {Success = true, Extra = CartCount().ToString()}});
            }
            [HttpPost]
            public IActionResult Create(DocumentPurchaseVm model)
            {
                var gateway = _braintreeService.GetGateway();
                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(model.Total / 3),
                    PaymentMethodNonce = model.Nonce,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                var result = gateway.Transaction.Sale(request);

                if (result.IsSuccess())
                {
                    
                    var operation = BllUser.UpdatePurchaseApi(model, _notification);
                    HttpContext.Session.SetObjectAsJson("cart", new List<Item>());
                    return Json(new {Data = operation});
                }

                _notification.AddErrorToastMessage("Purchase failed !");
                return Json(new {Data = new JsonResponse {Success = false}});
            }
        #endregion

        #region document
            public IActionResult Document(string id)
            {
                if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");
                var operation = BllDocument.GetDocumentById("Id", id);
                if (operation.DocumentGroup.Id == 0 || operation.DocumentGroup.AccessType == "Hidden")
                    return RedirectToAction("Index");
                ViewBag.acess = BllUser.ValidatePurchase(operation.DocumentGroup.Id.ToString(),
                    User.FindFirstValue(ClaimTypes.NameIdentifier)).Success;
                ViewBag.Title = operation.DocumentGroup.OriginalTitle;
                return View("Document/index", operation);
            }
            [HttpGet]
            public IActionResult Download(string auth)
            {
                string[] authValues;

                try
                {
                    authValues = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth)).Split(";");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
                
                if(authValues.Length < 2) RedirectToAction("Index");
                
                string userid = authValues[0], id = authValues[1];
                
                var operation = BllDocument.GetDocumentById("Id", id).DocumentGroup;

                if (!User.Identity.IsAuthenticated || User.FindFirst(ClaimTypes.NameIdentifier).Value != userid ||
                    string.IsNullOrEmpty(operation.File))
                    return RedirectToAction("Document", new {id});


                return File(
                    System.IO.File.ReadAllBytes(Path.Combine(Path.Combine(
                        Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"),
                            operation.DocumentType), operation.File))), "application/x-msdownload",
                    operation.OriginalTitle + Path.GetExtension(operation.File));
            }
            [HttpGet]
            public IActionResult View(string auth)
            {
                string[] authValues;

                try
                {
                    authValues = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth)).Split(";");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
                
                if(authValues.Length < 2) RedirectToAction("Index");
                
                string userid = authValues[0], id = authValues[1];
                
                var operation = BllDocument.GetDocumentById("Id", id).DocumentGroup;

                if (!User.Identity.IsAuthenticated || User.FindFirst(ClaimTypes.NameIdentifier).Value != userid ||
                    string.IsNullOrEmpty(operation.File))
                    return RedirectToAction("Document", new {id});

                Response.Headers.Add("Content-Disposition", $"inline; filename={operation.OriginalTitle}");
                return File(
                    System.IO.File.ReadAllBytes(Path.Combine(Path.Combine(
                        Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"),
                            operation.DocumentType), operation.File))), "application/pdf");
            }
            [HttpGet]
            public IActionResult Summary(string auth)
            {
                string[] authValues;

                try
                {
                    authValues = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth)).Split(";");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
                
                if(authValues.Length < 4) RedirectToAction("Index");
                
                
                string userid = authValues[0], id = authValues[1];
                int start = int.Parse(authValues[2]), end =int.Parse(authValues[3]);

                var operation = BllDocument.GetDocumentById("Id", id).DocumentGroup;

                if (!User.Identity.IsAuthenticated || User.FindFirst(ClaimTypes.NameIdentifier).Value != userid ||
                    string.IsNullOrEmpty(operation.File))
                    return RedirectToAction("Document", new {id});

                Response.Headers.Add("Content-Disposition", $"inline; filename={operation.OriginalTitle}");

                if (start == 0 && end == 0)
                {
                    return File(
                        System.IO.File.ReadAllBytes(Path.Combine(Path.Combine(
                            Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"),
                                operation.DocumentType), operation.File))), "application/pdf");
                }
                
                if (start == 0)
                {
                    start = 1;
                }
                if (end == 0)
                {
                    end = start;
                }

                string range = String.Join(",",Enumerable.Range(start,Math.Abs(end-start+1)));
                var stream = new MemoryStream();
                var pdf = new PdfDocument(new PdfReader(Path.Combine(Path.Combine(
                    Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"),
                        operation.DocumentType), operation.File))));
                var split = new ImprovedSplitter(pdf, pageRange => new PdfWriter(stream));
                var result = split.ExtractPageRange(new PageRange(range));
                result.Close();
                return File(stream.ToArray(), "application/pdf");
            }
            [HttpGet]
            public IActionResult GetAllComments(string idDocument)
            {
                return Json(new {Data = BllDocument.GetDocumentComment(idDocument)});
            }
            [Authorize(Roles = "Administrator,Editor,Individual,Organization")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult NewComment(DocumentComment documentComment)
            {
                return Json(new {Data = BllDocument.NewDocumentCommentApi(documentComment, _notification)});
            }
            [Authorize(Roles = "Administrator,Editor,Individual,Organization")]
            [HttpGet]
            public IActionResult DeleteComment(string id)
            {
                return Json(new {Data = BllDocument.DeleteDocumentCommentApi(id, _notification)});
            }
        #endregion
        
        #region User
            [HttpPost]
            public IActionResult UpdateField(string data, string id)
            {
                return Json(new {Data = BllUser.UpdateFieldApi(data.Split("=")[0], data.Split("=")[1], id.Split("=")[1])});
            }
            public IActionResult Profile(string id)
            {
                UserInfo data;
                if (!string.IsNullOrEmpty(id))
                {
                    data = BllUser.GetAllUserInfoApi(id);
                }
                else
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        data = BllUser.GetAllUserInfoApi(id);
                    }
                    else
                    {
                        data = null;
                    }

                }

                return data != null && data.UsersGroup.Id == 0 ? RedirectToAction("Index") : View("User/profile", data);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult UpdateProfile(UserInfo user)
            {
                BllAccount.UpdateUserApi(user, _notification);
                return RedirectToAction("Profile", new {id = ""});
            }
            public IActionResult MyDocuments()
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");

                ViewBag.Title = "My Documents";

                return View("User/Mydocuments",
                    BllUser.GetAllOwnedDocuments(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            public IActionResult MyWishList()
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");


                return View("User/Mywishlist",
                    BllUser.GetUserWishList(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            public IActionResult AddToWishList(string id)
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");
                return Json(new {Data = BllUser.AddToWishListApi(id,User.FindFirstValue(ClaimTypes.NameIdentifier), _notification) } );
            }
            public IActionResult RemoveFromWishList(string id)
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");
                return Json(new {Data = BllUser.RemoveFromWishListApi(id,User.FindFirstValue(ClaimTypes.NameIdentifier), _notification) } );
            }
            public IActionResult Organization()
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Index");

                if (string.IsNullOrEmpty(HttpContext.Session.GetObjectFromJson<string>("organizationName")))
                    return RedirectToAction("Index");

                ViewBag.Title = HttpContext.Session.GetObjectFromJson<string>("organizationName");
                return View("User/Mydocuments",
                    BllUser.GetAllOwnedDocuments(HttpContext.Session.GetObjectFromJson<string>("organizationId")));
            }
        #endregion

        #region Search
            [HttpPost]
            public IActionResult Search(string term, string termType)
            {
                return View("Search/Index", BllDocument.SearchDocuments(term, termType));
            }
            public IActionResult Theme(string id, string type)
            {
                var op = BllDocument.GetDocumentBytheme(id, type);
                return View("Search/Index", op);
            }
            [HttpPost]
            public IActionResult AjaxSearch(string term, string termType)
            {
                if (termType == null)
                {
                    termType = "OriginalTitle";
                }

                return Json(new {Data = BllDocument.SearchDocuments(term, termType)});
            }
        #endregion

    }
}