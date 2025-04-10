﻿using Microsoft.AspNetCore.Identity;

namespace CozyHouse.Core.Helpers
{
    public class ExtendedSignInResult
    {
        public SignInResult Result { get; set; }
        public string Message { get; set; }
        public ExtendedSignInResult(SignInResult result, string message)
        {
            Result = result;
            Message = message;
        }
    }
}
