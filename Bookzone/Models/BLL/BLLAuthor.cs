using System.Collections.Generic;
using Bookzone.Extensions;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.Author;
using NToastNotify;

namespace Bookzone.Models.BLL
{
    public static class BllAuthor
    {
        public static IEnumerable<Author> GetAllAuthor()
        {
            return DalAuthor.GetAllAuthor();
        }
        
        public static Author GetAuthorBy(string field, string value)
        {
            return DalAuthor.GetAuthorBy(field, value);
        }
        
        private static JsonResponse NewAuthor(Author u)
        {
            return DalAuthor.NewAuthor(u);
        }
        
        private static JsonResponse DeleteAuthorBy(string field, string value)
        {
            return DalAuthor.DeleteAuthorBy(field, value);
        }

        private static JsonResponse UpdateAuthor(Author author)
        {
            return DalAuthor.UpdateAuthor(author);
        }
        
        private static JsonResponse UpdateUpdateAuthorByField(string fieldname, string value, string id)
        {
            return DalAuthor.UpdateCollectionByField(fieldname, value, id);
        }
        
        
        
        
        #region Collection api
        
        public static JsonResponse DeleteApi(int? id, IToastNotification notification)
        {
            var message = new JsonResponse();
            var collectionFromDb = GetAuthorBy("Id", id.ToString());
            if (collectionFromDb == null)
            {
                message.Success = false;
                message.Message = "Error while Deleting";
            }
            else
            {
                message = DeleteAuthorBy("Id", collectionFromDb.Id.ToString());
                if (message.Success)
                {
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




        public static JsonResponse UpdateApi(Author author, IToastNotification notification)
        {
            var message = UpdateAuthor(author);

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

        public static JsonResponse NewAuthorApi(Author author, IToastNotification notification)
        {
            if (GetAuthorBy("Name", author.Name).Id != 0)
            {
                var jsonResponse = new JsonResponse()
                {
                    Success = false,
                    Message = "Exist already"
                };
                //notification.AddWarningToastMessage(jsonResponse.Message);
                return jsonResponse;

            }
            
            var message = NewAuthor(author);

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
        
        public static JsonResponse UpdateFieldApi(string fieldname, string value, string id)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var message = UpdateUpdateAuthorByField(fieldname, value, id);
            message.Extra = value;
            return message;
        }


        #endregion

        
    }
}