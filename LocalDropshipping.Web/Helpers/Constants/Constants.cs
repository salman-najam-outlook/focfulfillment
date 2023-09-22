namespace LocalDropshipping.Web.Helpers.Constants
{
    public static class Constants
    {
        public static class ErrorMessage
        {
            public const string EmailAlreadyRegister = "This email address is already register";
            public const string EnterRegisterEmail = "Please enter your register email";
            public const string VerifyEmail = "Please verify your email";
            public const string PasswordNotMatch = "Your Password does not match";
            public const string EnterSendCode = "Please Enter the code we have sent to your email";
            public const string CodeAlreadyRegister = "Your verification code is already verified";
            public const string VerificationCodeExpired = "Your verification code is expired";
            public const string EmailAlreadyVerified = "Your email is already verified";
            public const string LinkIsExpired = "Your link is expired. Please create a new link";
            public const string UnableToFindUser = "Unable to find user";
            public const string TryAgain = "Please try again";
            public const string InvalidCode = "Invalid code";
            public const string InvalidRequest = "Invalid Request";
            public const string _400 = "You have made Bad Request";
            public const string _404 = "resources not found";
            public const string _500 = "server error";
            public const string _401 = "You are not authorized";
            public const string ErrorDuringMigration = "An error occured during migration";
            public const string UserNotFound = "User Not Found!Please create new account";
            public const string FailedToUpdateOrder = "Failed to update order please try later...";
            public const string OrderNotFound = "Something went wrong in order list";
            public const string AddNewCategory = "Something went wrong while adding product";
        }

        public static class AdminActionMethods
        {
            public const string OrdersList = "OrdersList";
            public const string Dashboard = "Dashboard";
            public const string Admin = "Admin";


        }
        public static class AdminTempData
        {
            public const string NotificationMessage = "notificationMessage";
        }
        public static class ContentTypeFiles
        {
            public const string ApplicationJson = "application/json";
        }
    }
}
