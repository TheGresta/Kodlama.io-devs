﻿namespace Kodlama.Io.Devs.Application.Features.Users.Constants
{
    public static class UserBusinessRulesMessages
    {
        public static string EmailDoesNotExist => "There is no any user with given email address in the system!";
        public static string IdDoesNotExist => "There is no any user with given Id in the system!";
        public static string ThereIsNoAnyDataInList => "There is no any user data in the system as required!";
        public static string EmailAlreadyExist => "There is already an user with same email address in the system!";
        public static string PasswordIsIncorrect => "Wrong password!";
    }
}
