using Model.Data;
using Microsoft.AspNetCore.Mvc;
namespace Reports.Controllers
{
    public class PermissionController : Controller
    {
        public static bool CheckUserPremissions([FromBody]UserDetails User, string action) // add by moshe blocking routing from url 
        {
            bool result = true;
            if (action == "Filler")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = false;
                        break;

                    case (int)UserEnumPremission.insert_content:
                        result = false;
                        break;

                    case (int)UserEnumPremission.staff_officer:
                        result = false;
                        break;

                    case (int)UserEnumPremission.unit_manager:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }
            else if (action == "Reports")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = true;
                        break;

                    case (int)UserEnumPremission.insert_content:
                        result = true;
                        break;

                    case (int)UserEnumPremission.staff_officer:
                        result = false;
                        break;

                    case (int)UserEnumPremission.unit_manager:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }
            else if (action == "DeleteReports")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = true;
                        break;

                    case (int)UserEnumPremission.insert_content:
                        result = true;
                        break;

                    case (int)UserEnumPremission.staff_officer:
                        result = false;
                        break;

                    case (int)UserEnumPremission.unit_manager:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }

            else if (action == "CreateActivityFiller" || action == "DeleteActivityFiller" || action == "ActivityFormsFiller")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = false;
                        break;

                    case (int)UserEnumPremission.insert_content:// need to check with alon - its should be by unit tree premission logic
                        result = false;
                        break;

                    case (int)UserEnumPremission.staff_officer:// need to check with alon - its should be by unit tree premission logic
                        result = false;
                        break;

                    case (int)UserEnumPremission.unit_manager:// need to check with alon - its should be by unit tree premission logic
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }
            else if (action == "SubmitCreateActivityFiller")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = false;
                        break;

                    case (int)UserEnumPremission.insert_content:
                        result = false;
                        break;

                    case (int)UserEnumPremission.staff_officer:
                        result = false;
                        break;

                    case (int)UserEnumPremission.unit_manager:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }

            else if (action == "ConfirmActivityFiller")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = true;
                        break;

                    case (int)UserEnumPremission.insert_content:// need to check with alon - its should be by unit tree premission logic
                        result = false;
                        break;

                    case (int)UserEnumPremission.staff_officer:// need to check with alon - its should be by unit tree premission logic
                        result = false;
                        break;

                    case (int)UserEnumPremission.unit_manager:// need to check with alon - its should be by unit tree premission logic
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }
            else if (action == "UpdateFormStatusToDraft")
            {
                switch (User.UserAdminPermission)
                {
                    case (int)UserEnumPremission.permission_manager:
                        result = true;
                        break;
                    case (int)UserEnumPremission.employee:
                        result = true;
                        break;

                    case (int)UserEnumPremission.insert_content:
                        result = true;
                        break;

                    case (int)UserEnumPremission.staff_officer:
                        result = true;
                        break;

                    case (int)UserEnumPremission.unit_manager:
                        result = true;
                        break;

                    case (int)UserEnumPremission.Content_client:
                        result = false;
                        break;

                    case (int)UserEnumPremission.Content_manager:
                        result = false;
                        break;
                    case (int)UserEnumPremission.system_administrator:
                        result = false;
                        break;
                    default:
                        result = false;
                        break;

                }
                return result;
            }
            return result;
        }
    }
}
