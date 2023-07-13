using System.Collections.Generic;
using Bookzone.Extensions;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.User;
using Microsoft.AspNetCore.Hosting;
using NToastNotify;

namespace Bookzone.Models.BLL
{
    public static class BllAccount
    {
        private static JsonResponse CheckUserLogIn(Users u)
        {
            return DalAccount.CheckUserLogIn(u.Email, u.Password);
        }

        public static Users GetUserBy(string field, string value)
        {
            return DalAccount.GetUserBy(field, value);
        }

        public static Individual GetIndvBy(string field, string value)
        {
            return DalAccount.GetIndvBy(field, value);
        }

        public static Administrator GetAdministratorBy(string field, string value)
        {
            return DalAccount.GetAdministratorBy(field, value);
        }

        public static Organization GetOrganizationBy(string field, string value)
        {
            return DalAccount.GetOrganizationBy(field, value);
        }

        public static Editor GetEditorBy(string field, string value)
        {
            return DalAccount.GetEditorBy(field, value);
        }

        public static UserInfo GetUserEditorBy(string field, string value)
        {
            return new UserInfo()
            {
                UsersGroup = DalAccount.GetUserBy(field, value),
                EditorGroup = DalAccount.GetEditorBy(field, value)
            };
        }
        
        private static JsonResponse AddUser(Users u, bool dash)
        {
            return DalAccount.AddUser(u, dash);
        }

        private static JsonResponse NewUser(UserInfo u)
        {
            return DalAccount.NewUser(u);
        }

        public static IEnumerable<UserInfo> GetAllUsersGroupedBy(string type)
        {
            return DalAccount.GetAllUsersGroupedBy(type);
        }
        
        private static JsonResponse DeleteUserBy(int? id, string role)
        {
            return DalAccount.DeleteUserBy("Id", id.ToString(), role);
        }

        private static JsonResponse UpdateUser(UserInfo user)
        {
            return DalAccount.UpdateUser(user);
        }
        private static JsonResponse UpdateUserInfo(UserInfo user)
        {
            return DalAccount.UpdateUserProfile(user);
        }
        private static JsonResponse ActivateUser(string code)
        {
            return DalAccount.ActivateUser(code);
        }
        private static JsonResponse UpdateUserProfile(Individual indvusr, string id)
        {
            return DalAccount.UpdateUserProfile(indvusr, id);
        }
        public static JsonResponse ResetUserPassword(string code, string NewPassword)
        {
            return DalAccount.ResetUserPassword(code, NewPassword);
        }
        private static JsonResponse ResetUserPasswordCheck(string code)
        {
            return DalAccount.ResetUserPasswordCheck(code);

        }
        private static JsonResponse PrepareUserForReset(Users user)
        {
            return DalAccount.PrepareUserForReset(user);
        }
        
        public static JsonResponse ActivateApi(string code, IToastNotification notification)
        {
            var message = ActivateUser(code);
            if (message != null)
                if (message.Success)
                    notification.AddSuccessToastMessage(message.Message);
                else
                    notification.AddErrorToastMessage(message.Message);
            return message;
        }


        public static JsonResponse ProfileApi(Individual indvusr, string id, IToastNotification toastNotification)
        {
            var message = UpdateUserProfile(indvusr, id);

            if (message.Success)
                toastNotification.AddSuccessToastMessage(message.Message);
            else
                toastNotification.AddErrorToastMessage(message.Message);
            return message;
        }

        public static JsonResponse UpdateUserApi(UserInfo User, IToastNotification toastNotification)
        {
            var message = UpdateUserInfo(User);
            if (message.Success)
                toastNotification.AddSuccessToastMessage(message.Message);
            else
                toastNotification.AddErrorToastMessage(message.Message);
            return message;
        }

        #region AccountApi

        public static Users LoginApi(Users u, IToastNotification notification)
        {
            var message = CheckUserLogIn(u);
            if (message.Success)
            {
                var user = GetUserBy("Email", u.Email);
                if (user.EmailConfirmed) return user;
                message.Message = "Activate your account";
                notification.AddWarningToastMessage(message.Message);
                return null;
            }

            notification.AddErrorToastMessage(message.Message);
            return null;
        }

        public static JsonResponse SignupApi(Users u, IToastNotification notification)
        {
            var message = AddUser(u, false);

            if (message.Success)
                notification.AddWarningToastMessage(message.Message);
            else
                notification.AddErrorToastMessage(message.Message);
            return message;
        }

        public static JsonResponse DeleteApi(int? id, IToastNotification notification,
            IWebHostEnvironment webHostEnvironment)
        {
            var message = new JsonResponse();
            var userFromDb = GetUserBy("Id", id.ToString());
            if (userFromDb == null)
            {
                message.Success = false;
                message.Message = "Error while Deleting";
            }
            else
            {
                message = DeleteUserBy(id, userFromDb.Role);
                if (message.Success)
                {
                    if (userFromDb.Photo != "default.png") 
                        try
                        {
                            System.IO.File.Delete(webHostEnvironment.WebRootPath+$"/img/user/{userFromDb.Photo}");
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


        public static JsonResponse UpdateApi(UserInfo userInfo, IToastNotification notification)
        {
            var message = UpdateUser(userInfo);

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

        public static JsonResponse NewUserApi(UserInfo userInfo, IToastNotification notification)
        {
            var message = NewUser(userInfo);

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

        #endregion

        public static JsonResponse ResetApi(string email, IToastNotification notification)
        {
            var message = new JsonResponse()
            {
                Success = false
            };
            
            var operation = GetUserBy("Email", email);
            if (operation == null)
            {
                notification.AddErrorToastMessage("Email does not exist");
            }
            else
            {
                var result = PrepareUserForReset(operation);
                if (result.Success)
                {
                    notification.AddWarningToastMessage("Check your mailbox");
                    message.Success = true;
                }
                else
                {
                    notification.AddErrorToastMessage(result.Message);
                }

            }
            return message;
        }
        public static JsonResponse ResetPasswordApi(string code, IToastNotification notification)
        {
            var message = ResetUserPasswordCheck(code);
            if (!message.Success)
            {
                notification.AddErrorToastMessage(message.Message);
            }
            return message;
        }


    }
}