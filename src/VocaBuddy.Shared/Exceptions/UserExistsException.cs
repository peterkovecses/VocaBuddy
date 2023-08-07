﻿using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Shared.Exceptions;

public class UserExistsException : ApplicationExceptionBase
{
    public UserExistsException() : base("User with this e-mail address already exists.") 
    {
        ErrorCode = IdentityErrorCode.UserExists;
    }
}
