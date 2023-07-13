using System.Collections.Generic;
using System.Linq;
using Bookzone.Extensions;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.Product;
using Bookzone.Models.Entity.User;
using Ebook.Models.Entity.Document;
using NToastNotify;

namespace Bookzone.Models.BLL
{
    public static class BllUser
    {
        public static IEnumerable<DocumentInfo> GetAllOwnedDocuments(string userid)
        {
            return DalUser.GetAllOwnedDocuments(userid);
        }
        private static JsonResponse UpdateField(string fieldname, string value, string id)
        {
            return DalUser.UpdateUsrProfileByField(fieldname, value, id);
        }
        public static JsonResponse AddPurchase(string docId, string userId)
        {
            return DalUser.AddPurchase(docId, userId);
        }
        public static JsonResponse ValidatePurchase(string docId, string userId)
        {
            return DalUser.ValidatePurchase(docId, userId);
        }
        public static Organization GetOrganizationByIp(string ip)
        {
            return DalUser.GetOrganizationByIp(ip);
        }
        public static IEnumerable<DocumentInfo> GetWishListByUser(string id)
        {
            return DalUser.GetWishListByUser(id);
        }
        public static JsonResponse ValidateStateWishList(string docId, string userId)
        {
            return DalUser.ValidateStateWishList(docId, userId);
        }
        public static JsonResponse AddToWishList(string docId, string userId)
        {
            return DalUser.AddToWishList(docId, userId);
        }
        private static JsonResponse RemoveFromWishList(string docId, string userId)
        {
            return DalUser.RemoveFromWishList(docId, userId);
        }
        
        
        
        public static UserInfo GetAllUserInfoApi(string id)
        {
            var isId = int.TryParse(id, out _);
            var data = new UserInfo
            {
                UsersGroup = isId ? BllAccount.GetUserBy("Id", id) : BllAccount.GetUserBy("Email", id)
            };
            switch (data.UsersGroup.Role)
            {
                case "Individual":
                    data.IndividualGroup = BllAccount.GetIndvBy("id", data.UsersGroup.Id.ToString());
                    break;
                case "Administrator":
                    data.AdministratorGroup = BllAccount.GetAdministratorBy("id", data.UsersGroup.Id.ToString());
                    break;
                case "Editor":
                    data.EditorGroup = BllAccount.GetEditorBy("id", data.UsersGroup.Id.ToString());
                    break;
                case "Organization":
                    data.OrganizationGroup = BllAccount.GetOrganizationBy("id", data.UsersGroup.Id.ToString());
                    break;
            }
            return data;
        }
        public static JsonResponse UpdateFieldApi(string fieldname, string value, string id)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var message = UpdateField(fieldname, value, id);
            message.Extra = value;
            return message;
        }
        public static JsonResponse UpdatePurchaseApi(DocumentPurchaseVm model, IToastNotification notification)
        {
            var message = new JsonResponse()
            {
                Success = false
            };
            foreach (var iddoc in model.ListDoc.Where(iddoc => !ValidatePurchase(iddoc.ToString(), model.UserId.ToString()).Success))
            {
                message = AddPurchase(iddoc.ToString(), model.UserId.ToString());
                message.Message = "You brought it already";
            }
            if (message.Success)
            {
                notification.AddSuccessToastMessage("Purchase is successful");
            }
            else
            {
                notification.AddErrorToastMessage(message.Message);
            }
            return message;
        }
        public static IEnumerable<DocumentInfo> GetUserWishList(string id)
        {
            var Wishlst = GetWishListByUser(id);
            return Wishlst;
        }
        public static JsonResponse AddToWishListApi(string docId, string userId, IToastNotification notification)
        {
            var message = new JsonResponse()
            {
                Success = false
            };

            message = ValidateStateWishList(docId, userId);
            if (!message.Success)
            {
                message = AddToWishList(docId, userId);

                if (message.Success)
                {
                    notification.AddSuccessToastMessage("Added successfully");
                }
                else
                {
                    notification.AddErrorToastMessage(message.Message);
                }
            }
            else
            {
                notification.AddInfoToastMessage("Exist already");
            }

            return message;
        }
        public static JsonResponse RemoveFromWishListApi(string docId, string userId, IToastNotification notification)
        {
            var message = new JsonResponse()
            {
                Success = false
            };
            message = RemoveFromWishList(docId, userId);
            if (message.Success)
            {
                notification.AddSuccessToastMessage("Removed successfully");

            }
            else
            {
                notification.AddErrorToastMessage(message.Message);

            }

            return message;
        }


    }
}