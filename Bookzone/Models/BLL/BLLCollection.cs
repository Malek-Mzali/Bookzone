using System.Collections.Generic;
using Bookzone.Extensions;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.Collection;
using Bookzone.Models.Entity.Theme;
using NToastNotify;

namespace Bookzone.Models.BLL
{
    public static class BllCollection
    {
        public static IEnumerable<Collection> GetAllCollection()
        {
            return DalCollection.GetAllCollection();
        }
        
        public static IEnumerable<Collection> GetCollectionForEditor(string value)
        {
            return DalCollection.GetCollectionForEditor(value);
        }
        
        public static Collection GetCollectionBy(string field, string value)
        {
            return DalCollection.GetCollectionBy(field, value);
        }
        
        private static JsonResponse NewCollection(Collection u)
        {
            return DalCollection.NewCollection(u);
        }
        
        private static JsonResponse DeleteCollectionBy(string field, string value)
        {
            return DalCollection.DeleteCollectionBy(field, value);
        }

        private static JsonResponse UpdateCollection(Collection collection)
        {
            return DalCollection.UpdateCollection(collection);
        }
        
        private static JsonResponse UpdateCollectionByField(string fieldname, string value, string id)
        {
            return DalCollection.UpdateCollectionByField(fieldname, value, id);
        }
        
        public static Theme GetThemeByCollection(string themeId)
        {
            return DalCollection.GetThemeByCollection(themeId);
        }

        public static IEnumerable<Collection> GetAllCollectionByTheme(string themeId)
        {
            return DalCollection.GetAllCollectionByTheme(themeId);
        }
        
        
        #region Collection api
        
        public static JsonResponse DeleteApi(int? id, IToastNotification notification)
        {
            var message = new JsonResponse();
            var collectionFromDb = GetCollectionBy("Id", id.ToString());
            if (collectionFromDb == null)
            {
                message.Success = false;
                message.Message = "Error while Deleting";
            }
            else
            {
                message = DeleteCollectionBy("Id", collectionFromDb.Id.ToString());
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




        public static JsonResponse UpdateApi(Collection collection, IToastNotification notification)
        {
            var message = UpdateCollection(collection);

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

        public static JsonResponse NewCollectionApi(Collection collection, IToastNotification notification)
        {

            
            var message = NewCollection(collection);

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
            var message = UpdateCollectionByField(fieldname, value, id);
            message.Extra = value;
            return message;
        }


        #endregion

        
    }
}