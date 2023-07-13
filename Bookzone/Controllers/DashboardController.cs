using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using Bookzone.Extensions;
using Bookzone.Models.BLL;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.Author;
using Bookzone.Models.Entity.Collection;
using Bookzone.Models.Entity.Document;
using Bookzone.Models.Entity.Theme;
using Bookzone.Models.Entity.User;
using Ebook.Models.Entity.Document;
using HtmlAgilityPack;
using MARC4J.Net;
using MARC4J.Net.MARC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Bookzone.Controllers
{
    public sealed class DashboardController: Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IToastNotification _notification;
        
        public DashboardController(IWebHostEnvironment hostingEnvironment, IToastNotification notification)
        {
            this._hostingEnvironment = hostingEnvironment;
            _notification = notification;
        }

        [Authorize(Roles = "Administrator,Editor")]
        public IActionResult Index()
        {
            if (User.IsInRole("Editor"))
            {
                return View(BllStatistics.StatisticsEditorApi(User.FindFirst(ClaimTypes.NameIdentifier).Value));

            }
            else
            {
                return View(BllStatistics.StatisticsApi());

            }
        }

        #region User
            [Authorize(Roles = "Administrator")]
            public IActionResult Users()
            {
                return View("User/Index");
            }
            [Authorize(Roles = "Administrator")]
            [HttpGet]
            public IActionResult GetAllUsers(string type)
            {
                return Json(new {Data = BllAccount.GetAllUsersGroupedBy(type)});
            }
            [HttpGet]
            public IActionResult GetUserEditorBy(string id)
            {
                return Json(new {Data = BllAccount.GetUserEditorBy("Id", id)});
            }
            [HttpGet]
            [Authorize(Roles = "Administrator")]
            public IActionResult DeleteUser(int? id)
            {
                return Json(new {Data = BllAccount.DeleteApi(id, _notification, _hostingEnvironment)});
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Administrator")]
            public IActionResult EditUser(UserInfo u)
            {
                return Json(new {Data = BllAccount.UpdateApi(u, _notification)});
            }
            [Authorize(Roles = "Administrator")]
            public IActionResult NewUser()
            {
                return View("User/New");
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Administrator")]
            public IActionResult NewUser(UserInfo u)
            {
                return Json(new {Data = BllAccount.NewUserApi(u, _notification)});
            }
        #endregion

        #region Document
            [Authorize(Roles = "Administrator,Editor")]
            public IActionResult Documents()
            {
                return View("Document/Index");
            }
            [HttpGet]
            public IActionResult GetAllDocuments(string type, string extra)
            {
                return Json(new {Data = BllDocument.GetAllBooksGroupedBy(type, extra)});
            }
            [HttpGet]
            public IActionResult GetAllAuthorDocument(string value)
            {
                return Json(new {Data = BllDocument.GetAllDocumentGroupedBy("IdAuthor", value)});
            }
            [HttpGet]
            public IActionResult GetLastDocuments()
            {
                return Json(new {Data = BllDocument.GetLastDocuments()});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpGet]
            public IActionResult DeleteDocument(int? id)
            {
                return Json(new {Data = BllDocument.DeleteApi(id, _notification, _hostingEnvironment)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EditDocument(DocumentInfo documentInfo)
            {
                return Json(new {Data = BllDocument.UpdateApi(documentInfo, _notification)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            public IActionResult NewDocument()
            {
                return View("Document/New");
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult NewDocument(DocumentInfo documentInfo)
            {
                return Json(new {Data = BllDocument.NewDocumentApi(documentInfo, _notification)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpPost]
            [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
            public  IActionResult NewDocumentMarc(IFormFile marcFile)
            {
                if (Path.GetExtension(marcFile.FileName) == ".xml")
                {
                    using (Stream stream = marcFile.OpenReadStream()){
                        MarcXmlReader reader = new MarcXmlReader(stream);
                        int totalRecord = 0;
                        int currentRecord = 0;
                        try
                        {
                            totalRecord = reader.ToArray().Length;
                        }
                        catch
                        {
                            // ignored
                        }
                        var data = new List<MarcInfo>();
                        try
                        {
                            stream.Seek(0, SeekOrigin.Begin); 
                            foreach (var record in reader)
                            {
                                if (!data.Any(x => x.Title == BLLMarc.GetTitle(record)) &&
                                    !string.IsNullOrEmpty(BLLMarc.GetDocumentType(record)))
                                {
                                    data.Add(BLLMarc.RecordApi(record));
                                    currentRecord++;
                                }
                                Console.WriteLine(currentRecord + "/"+totalRecord);
                            }
                        }
                        catch (Exception)
                        {
                            _notification.AddErrorToastMessage("Some records or the file might be broken");
                        }
                        stream.Close();

                        
                        return Json(new {Data = data});
                    }
                }
                else
                {
                    using (Stream stream = marcFile.OpenReadStream()){
                        using (IMarcReader reader = new MarcStreamReader(stream, "UTF-8"))
                        {
                            int totalRecord = 0;
                            int currentRecord = 0;
                            try
                            {
                                totalRecord = reader.ToArray().Length;
                            }
                            catch
                            {
                                // ignored
                            }
                        

                            var data = new List<MarcInfo>();
                            try
                            {
                                stream.Seek(0, SeekOrigin.Begin); 
                                foreach (var record in reader)
                                {
                                    if (!data.Any(x => x.Title == BLLMarc.GetTitle(record)) &&
                                        !string.IsNullOrEmpty(BLLMarc.GetDocumentType(record)))
                                    {
                                        data.Add(BLLMarc.RecordApi(record));
                                        currentRecord++;
                                    }

                                    Console.WriteLine(currentRecord + "/"+totalRecord);
                                }
                            }
                            catch (Exception )
                            {
                                _notification.AddErrorToastMessage("Some records or the file might be broken");
                            }
                            stream.Close();
                            return Json(new {Data = data});
                        }


                    }
                }

            }
            [Authorize(Roles = "Administrator,Editor")]
            public  IActionResult ExportDocumentMarc()
            {
                using (var fs2 = new FileStream("Marc file location", FileMode.OpenOrCreate))
                {
                    using (var writer = new MarcStreamWriter(fs2, "UTF-8"))
                    {
                        var record = MarcFactory.Instance.NewRecord("00000cam a2200000 a 4500");

                        record.AddVariableField(MarcFactory.Instance.NewControlField("001", "ISBN"));
                        record.AddVariableField(MarcFactory.Instance.NewControlField("005", "Time"));
                        record.AddVariableField(MarcFactory.Instance.NewControlField("008", "Hard"));

            
                        var dataField = MarcFactory.Instance.NewDataField("200", ' ', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', "Sample title"));
                        record.AddVariableField(dataField);
            
                        writer.Write(record);
                    }
                    return Json(new {Data = "data"});
                }

            }
            [HttpGet]
            public IActionResult GetDateRange() 
            {
                return Json(new {Data = BllDocument.GetDateRange()});
            }
            [HttpGet]
            public IActionResult GetAmazonBookCover(string Code, string language)
            {
                string img = null;

                if (Code.Length == 10)
                {
                    img= $"https://ws-eu.amazon-adsystem.com/widgets/q?_encoding=UTF8&ASIN={Code}&Format=_SL_&ID=AsinImage&ServiceVersion=20070822&WS=1&tag=9w0k14ju61-21";
                   var request = (HttpWebRequest) WebRequest.Create(img);
                   request.Method = "GET";
                   request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; rv:68.0) Gecko/20100101 Firefox/68.0";
                   request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                   request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
                   request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                   var response = (HttpWebResponse)request.GetResponse();
                   var responseStream = response.GetResponseStream();
                   using var ms = new MemoryStream();
                   responseStream?.CopyTo(ms);

                   var htmlContent = Encoding.UTF8.GetString(ms.ToArray());
                   if (htmlContent == "")
                   {
                       img = null;
                   }
                }
                else
                { 
                    Code = HttpUtility.UrlEncode(Code);
                    string search;
                    if (language == "Arabic")
                    {
                         search = $"https://www.amazon.ae/s?k={Code}";

                    }
                    else
                    {
                         search = $"https://www.amazon.com/s?k={Code}";

                    }
                    var request = (HttpWebRequest) WebRequest.Create(search);
                    request.Method = "GET";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; rv:68.0) Gecko/20100101 Firefox/68.0";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
                    request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();
                    if (response.ContentEncoding?.IndexOf("gzip", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding?.IndexOf("deflate", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                    }
                    
                    
                    using var ms = new MemoryStream();
                    responseStream?.CopyTo(ms);

                    var htmlContent = Encoding.UTF8.GetString(ms.ToArray());
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlContent);
                    try
                    {
                        img = htmlDocument.DocumentNode
                            .SelectSingleNode(
                                "//*[@id=\"search\"]/div[1]/div[1]/div/span[3]/div[2]/div[2]/div/div/div/div/div/div[1]/div/div[2]/div/span/a/div/img")
                            .GetAttributeValue("src", "nothing").Split("._")[0] + ".jpg";

                    }
                    catch
                    {
                        // ignored
                    }
                }

                return Json(new {Data = img});


            }
        #endregion

        #region DocumentSummary
            [HttpGet]
            public IActionResult GetAllDocumentSummary(string id)
            {
                return Json(new {Data = BllDocument.GetAllDocumentSummary(id)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EditDocumentSummary( IFormCollection form )
            {
                return Json(new {Data = BllDocument.UpdateDocumentSummaryApi(form, _notification)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpGet]
            public IActionResult DeleteDocumentSummary(int id)
            {
                return Json(new {Data = BllDocument.DeleteDocumentSummaryApi(id, _notification)});
            }
        #endregion

        #region Theme
            [Authorize(Roles = "Administrator")]
            public IActionResult Theme()
            {
                return View("Theme/Index");
            }
            [HttpGet]
            public IActionResult GetAllTheme()
            {
                return Json(new {Data = BllTheme.GetAllThemes()});
            }
            [HttpGet]
            public IActionResult GetThemeByCollection(string id)
            {
                return Json(new {Data = BllCollection.GetThemeByCollection(id)});
            }
            [Authorize(Roles = "Administrator")]
            [HttpGet]
            public IActionResult DeleteTheme(int? id)
            {
                return Json(new {Data = BllTheme.DeleteApi(id, _notification)});
            }
            [Authorize(Roles = "Administrator")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EditTheme(Theme u)
            {
                return Json(new {Data = BllTheme.UpdateApi(u, _notification)});
            }
            [Authorize(Roles = "Administrator")]
            public IActionResult NewTheme()
            {
                return View("Theme/New");
            }
            [Authorize(Roles = "Administrator")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult NewTheme(Theme u)
            {
                return Json(new {Data = BllTheme.NewThemeApi(u, _notification)});
            }
        #endregion
        
        #region Collection
            [Authorize(Roles = "Administrator, Editor")]
            public IActionResult Collection()
            {
                return View("Collection/Index");
            }
            [Authorize(Roles = "Administrator, Editor")]
            [HttpGet]
            public IActionResult GetAllCollection()
            {
                return Json(new {Data = BllCollection.GetAllCollection()});
            }
            [HttpGet]
            public IActionResult GetCollectionBy(string id)
            {
                return Json(new {Data = BllCollection.GetCollectionBy("Id", id)});
            }
            [HttpGet]
            public IActionResult GetCollectionForEditor(string id)
            {
                return Json(new {Data = BllCollection.GetCollectionForEditor(id)});
            }
            [HttpGet]
            public IActionResult GetAllCollectionByTheme(string id)
            {
                return Json(new {Data = BllCollection.GetAllCollectionByTheme(id)});
            }
            [HttpGet]
            public IActionResult GetAllEditorByTheme(string id)
            {
                return Json(new {Data = BllTheme.GetAllEditorByTheme(id)});
            }
            [Authorize(Roles = "Administrator, Editor")]
            [HttpGet]
            public IActionResult DeleteCollection(int? id)
            {
                return Json(new {Data = BllCollection.DeleteApi(id, _notification)});
            }
            [Authorize(Roles = "Administrator, Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EditCollection(Collection u)
            {
                return Json(new {Data = BllCollection.UpdateApi(u, _notification)});
            }
            [Authorize(Roles = "Administrator, Editor")]
            public IActionResult NewCollection()
            {
                return View("Collection/New");
            }
            [Authorize(Roles = "Administrator, Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult NewCollection(Collection collection)
            {
                return Json(new {Data = BllCollection.NewCollectionApi(collection, _notification)});
            }
        #endregion

        #region Author
            [Authorize(Roles = "Administrator,Editor")]
            public IActionResult Author()
            {
                return View("Author/Index");
            }
            [HttpGet]
            public IActionResult GetAllAuthor()
            {
                return Json(new {Data = BllAuthor.GetAllAuthor()});
            }
            [HttpGet]
            public IActionResult GetAuthorBy(string id)
            {
                return Json(new {Data = BllAuthor.GetAuthorBy("Id", id)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpGet]
            public IActionResult DeleteAuthor(int? id)
            {
                return Json(new {Data = BllAuthor.DeleteApi(id, _notification)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EditAuthor(Author u)
            {
                return Json(new {Data = BllAuthor.UpdateApi(u, _notification)});
            }
            [Authorize(Roles = "Administrator,Editor")]
            public IActionResult NewAuthor()
            {
                return View("Author/New");
            }
            [Authorize(Roles = "Administrator, Editor")]
            [HttpPost]
            public IActionResult NewAuthor(Author author)
            {
                return Json(new {Data = BllAuthor.NewAuthorApi(author, _notification)});
            }
        #endregion

        #region ExtraFunctions
            [HttpPost]
            public IActionResult OnPostMyUploader(IFormFile myUploader, string id, string type)
            {
                try
                {
                    id = id.Split("=")[1];
                }
                catch
                {
                    // ignored
                }

                string uniqueFilxeName, doctype = null;
                
                if (myUploader != null )
                {
                    
                    uniqueFilxeName = Guid.NewGuid() + "_"+id+Path.GetExtension(myUploader.FileName);
                    string filePath;

                    if (type == "documentFile")
                    {
                        doctype = DalDocument.GetDocumentBy("id", id).DocumentType;
                        filePath = Path.Combine(Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"),doctype),uniqueFilxeName);
                    }
                    else
                    {
                        uniqueFilxeName = Guid.NewGuid() + "_"+id+Path.GetExtension(myUploader.FileName);
                        filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, $"img/{type}"),uniqueFilxeName);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        myUploader.CopyTo(fileStream);
                    }
                }
                else
                {
                    uniqueFilxeName = "default.png";
                }



                JsonResponse data = null;
                string path = null;
                
                switch (type)
                {
                    case "profile":
                    {
                        path = DalAccount.GetUserBy("id", id).Photo;
                        data = BllUser.UpdateFieldApi("UsersGroup.Photo", uniqueFilxeName, id);
                        break;
                    }

                    case "document":
                    {
                        path = DalDocument.GetDocumentBy("id", id).CoverPage;
                        data = BllDocument.UpdateFieldApi("DocumentGroup.CoverPage", uniqueFilxeName, id);
                        break;
                    }
                    
                    case "documentFile":
                    {
                        path = DalDocument.GetDocumentBy("id", id).File;
                        BllDocument.UpdateFieldApi("DocumentGroup.FileFormat", Path.GetExtension(uniqueFilxeName), id);
                        data = BllDocument.UpdateFieldApi("DocumentGroup.File", uniqueFilxeName, id);

                        break;
                    }

                    case "author":
                    {
                        path = DalAuthor.GetAuthorBy("id", id).Photo;
                        data = BllAuthor.UpdateFieldApi("Photo", uniqueFilxeName, id);
                        break;
                    }
                    
                }

                if (path == "default.png") return Json(new {Data = data});
                if (type == "documentFile")
                {
                    try
                    {
                        if (doctype != null)
                            if (path != null)
                                System.IO.File.Delete(Path.Combine(
                                    Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"), doctype),
                                    path));
                    }
                    catch
                    {
                        //
                    }
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(_hostingEnvironment.WebRootPath+$"/img/{type}/"+path);
                    }
                    catch
                    {
                        //
                    } 
                }

                return Json(new { Data = data });
            }
            [HttpPost]
            public IActionResult GoogleCover(string url, string id)
            {

                var uniqueFilxeName = Guid.NewGuid() + "_"+id+".jpg";
                var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, $"img/document"),uniqueFilxeName);

                using (var client = new WebClient()) 
                {
                    client.DownloadFile(new Uri(url), filePath);
                    
                }
                var path = DalDocument.GetDocumentBy("id", id).CoverPage;
                var data = BllDocument.UpdateFieldApi("DocumentGroup.CoverPage", uniqueFilxeName, id);
                if (path != "default.png")
                {
                    System.IO.File.Delete(_hostingEnvironment.WebRootPath+$"/img/document/"+path);
                }
                return Json(new {Data = data});
            }
        #endregion
    }
}