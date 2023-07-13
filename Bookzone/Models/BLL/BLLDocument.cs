using System.Collections.Generic;
using System.IO;
using Bookzone.Extensions;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.Document;
using Ebook.Models.Entity.Document;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NToastNotify;

namespace Bookzone.Models.BLL
{
    public static class BllDocument
    {
        public static IEnumerable<DocumentInfo> GetAllBooksGroupedBy(string type, string extra)
        {
            return DalDocument.GetAllBooksGroupedBy(type, extra);
        }

        public static IEnumerable<DocumentInfo> GetAllDocuments()
        {
            return DalDocument.GetAllDocuments();
        }
        
        public static IEnumerable<DocumentInfo> SearchDocuments(string term, string type)
        {
            return DalDocument.SearchDocuments(term, type);
        }

        public static IEnumerable<DocumentInfo> GetAllDocumentGroupedBy(string field, string value)
        {
            return DalDocument.GetAllDocumentGroupedBy(field, value);
        }

        public static IEnumerable<DocumentInfo> GetLastDocuments()
        {
            return DalDocument.GetLastDocuments();
        }

        public static IEnumerable<DocumentComment> GetDocumentComment(string idDocument)
        {
            return DalDocument.GetDocumentComment(idDocument);
        }
        public static IEnumerable<DocumentInfo> GetDocumentBytheme(string id, string type)
        {
            return DalDocument.GetDocumentByTheme(id, type);
        }

        public static IEnumerable<DocumentSummary> GetAllDocumentSummary(string id)
        {
            return DalDocument.GetAllDocumentSummary(id);
        }

        private static Document GetDocumentBy(string field, string value)
        {
            return DalDocument.GetDocumentBy(field, value);
        }

        public static DocumentInfo GetDocumentById(string field, string value)
        {
            return DalDocument.GetDocumentById(field, value);
        }
        
        public static IEnumerable<int> GetDateRange()
        {
            return DalDocument.GetDateRange();
        }

        private static JsonResponse DeleteDocumentBy(string field, string value, string documentType)
        {
            return DalDocument.DeleteDocumentBy(field, value, documentType);
        }

        private static JsonResponse DeleteDocumentSummaryBy(string field, string value)
        {
            return DalDocument.DeleteDocumentSummaryBy(field, value);
        }

        private static JsonResponse UpdateDocumentByField(string fieldname, string value, string id)
        {
            return DalDocument.UpdateDocumentByField(fieldname, value, id);
        }

        private static JsonResponse UpdateDocument(DocumentInfo documentInfo)
        {
            return DalDocument.UpdateDocument(documentInfo);
        }

        private static JsonResponse NewDocument(DocumentInfo documentInfo)
        {
            return DalDocument.NewDocument(documentInfo);
        }
        
        private static JsonResponse UpdateDocumentSummary(IEnumerable<DocumentSummary> summaries)
        {
            return DalDocument.UpdateDocumentSummary(summaries);
        }

        private static JsonResponse NewDocumentComment(DocumentComment documentComment)
        {
            return DalDocument.NewDocumentComment(documentComment);
        }

        private static JsonResponse EditDocumentComment(DocumentComment documentComment)
        {
            return DalDocument.EditDocumentComment(documentComment);
        }
        
        private static JsonResponse DeleteDocumentCommentBy(string field, string value)
        {
            return DalDocument.DeleteDocumentCommentBy( field,  value);
        }

        #region DocumentApi

        public static JsonResponse DeleteApi(int? id, IToastNotification notification,
            IWebHostEnvironment webHostEnvironment)
        {
            var message = new JsonResponse();
            var bookFromDb = GetDocumentBy("Id", id.ToString());
            if (bookFromDb == null)
            {
                message.Success = false;
                message.Message = "Error while Deleting";
            }
            else
            {
                message = DeleteDocumentBy("Id", id.ToString(), bookFromDb.DocumentType);
                if (message.Success)
                {
                    if (bookFromDb.CoverPage != "default.png")
                        try
                        {
                            File.Delete(webHostEnvironment.WebRootPath + $"/img/document/{bookFromDb.CoverPage}");
                        }
                        catch
                        {
                            //
                        }

                    if (bookFromDb.File != "")
                        try
                        {
                            File.Delete(Path.Combine(
                                Path.Combine(Path.Combine(Directory.GetCurrentDirectory() + "\\Documents\\"),
                                    bookFromDb.DocumentType), bookFromDb.File));
                        }
                        catch
                        {
                            //
                        }

                    message.Message = "Delete successful";
                    notification.AddSuccessToastMessage(message.Message);
                }
                else
                {
                    notification.AddErrorToastMessage(message.Message);
                }
            }

            return message;
        }

        public static JsonResponse UpdateApi(DocumentInfo documentInfo, IToastNotification notification)
        {
            var message = UpdateDocument(documentInfo);

            if (message.Success)
            {
                message.Message = "Update successful";
                notification.AddSuccessToastMessage(message.Message);
            }
            else
            {
                message.Success = false;
                notification.AddErrorToastMessage(message.Message);
            }

            return message;
        }

        public static JsonResponse UpdateFieldApi(string fieldname, string value, string id)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var message = UpdateDocumentByField(fieldname, value, id);
            message.Extra = value;
            return message;
        }

        public static JsonResponse NewDocumentApi(DocumentInfo documentInfo, IToastNotification notification)
        {
            if (GetDocumentBy("OriginalTitle", documentInfo.DocumentGroup.OriginalTitle).Id != 0)
            {
                var jsonResponse = new JsonResponse()
                {
                    Success = false,
                    Message = "Exist already"
                };
                notification.AddWarningToastMessage(jsonResponse.Message);
                return jsonResponse;

            }
            
            var message = NewDocument(documentInfo);

            if (message.Success)
            {
                message.Message = "Created successfully";
                notification.AddSuccessToastMessage(message.Message);
            }
            else
            {
                notification.AddErrorToastMessage(message.Message);
            }

            return message;
        }

        public static JsonResponse UpdateDocumentSummaryApi(IFormCollection form, IToastNotification notification)
        {
            var summaries = new List<DocumentSummary>();
            for (var i = 0; i < form["Id"].Count; i++)
            {
                var documentSummary = new DocumentSummary
                {
                    Id = int.Parse(form["Id"][i]),
                    IdDocument = int.Parse(form["IdDocument"][i]),
                    Title = form["Title"][i],
                    Start = int.Parse(form["Start"][i]),
                    End = int.Parse(form["End"][i])
                };
                summaries.Add(documentSummary);
            }

            var message = UpdateDocumentSummary(summaries);

            if (message.Success)
            {
                message.Message = "Update successful";
                notification.AddSuccessToastMessage(message.Message);
            }
            else
            {
                message.Success = false;
                notification.AddErrorToastMessage(message.Message);
            }

            return message;
        }

        public static JsonResponse DeleteDocumentSummaryApi(int id, IToastNotification notification)
        {
            var message = DeleteDocumentSummaryBy("Id", id.ToString());
            if (!message.Success) return message;
            message.Message = "Delete successful";
            notification.AddSuccessToastMessage(message.Message);

            return message;
        }
        
        public static JsonResponse NewDocumentCommentApi(DocumentComment documentComment, IToastNotification notification)
        {
            var message = NewDocumentComment(documentComment);

            if (message.Success)
            {
                message.Message = "Added successfully";
                notification.AddSuccessToastMessage(message.Message);
            }
            else
            {
                notification.AddErrorToastMessage(message.Message);
            }

            return message;
        }

        public static JsonResponse DeleteDocumentCommentApi(string id, IToastNotification notification)
        {
            var message = DeleteDocumentCommentBy("Id", id);
            if (!message.Success) return message;
            message.Message = "Delete successful";
            notification.AddSuccessToastMessage(message.Message);

            return message;
        }
        
        public static JsonResponse UpdateDocumentCommentApi(DocumentComment documentComment, IToastNotification notification)
        {
            var message = EditDocumentComment(documentComment);

            if (message.Success)
            {
                message.Message = "Edit successful";
                notification.AddSuccessToastMessage(message.Message);
            }
            else
            {
                message.Success = false;
                notification.AddErrorToastMessage(message.Message);
            }

            return message;
        }
        #endregion
    }
}