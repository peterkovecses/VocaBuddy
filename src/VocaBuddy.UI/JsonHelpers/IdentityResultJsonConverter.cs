using System.Text.Json;
using System.Text.Json.Serialization;

namespace VocaBuddy.UI.JsonHelpers;

public class IdentityResultJsonConverter : JsonConverter<IdentityResult>
{
    public const string ErrorProperty = "error";
    public const string ErrorMessageProperty = "errorMessage";
    public const string TokensProperty = "tokens";
    public const string AuthTokenProperty = "authToken";
    public const string RefreshTokenProperty = "refreshToken";

    private IdentityError? _error;
    private string? _errorMessage;
    private string? _authToken;
    private string? _refreshToken;

    public override IdentityResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ThrowIfNotStartObject(reader);
        MoveToNextJsonElement(ref reader);
        ProcessJsonProperties(ref reader, options);

        return CreateIdentityResult();

        static void ThrowIfNotStartObject(Utf8JsonReader reader)
        {
            if (!IsStartObject(reader.TokenType))
            {
                throw new JsonException();
            }
        }

        static bool IsStartObject(JsonTokenType tokenType)
            => tokenType == JsonTokenType.StartObject;

        static bool MoveToNextJsonElement(ref Utf8JsonReader reader)
            => reader.Read();

        static bool ReadJsonElementValue(ref Utf8JsonReader reader)
            => reader.Read();

        void ProcessJsonProperties(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            while (!EndObject(reader.TokenType))
            {
                StoreJsonPropertyValue(ref reader, options);
                MoveToNextJsonElement(ref reader);
            }
        }

        static bool EndObject(JsonTokenType tokenType)
            => tokenType == JsonTokenType.EndObject;

        void StoreJsonPropertyValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var propertyName = GetPropertyName(ref reader);
            ExtractValue(ref reader, options, propertyName);
        }

        void ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options, string propertyName)
        {
            ReadJsonElementValue(ref reader);

            if (IsErrorProperty(propertyName))
            {
                SetError(ref reader);
            }
            else if (IsErrorMessageProperty(propertyName))
            {
                _errorMessage = reader.GetString();
            }
            else if (IsTokenProperty(propertyName))
            {
                ProcessTokens(ref reader);
            }
        }

        void SetError(ref Utf8JsonReader reader)
        {
            try
            {
                reader.TryGetInt32(out var code);
                _error = (IdentityError)code;
            }
            catch { }
        }

        static string GetPropertyName(ref Utf8JsonReader reader)
        {
            ThrowIfNotPropertyName(reader.TokenType);

            return reader.GetString()!;
        }

        static void ThrowIfNotPropertyName(JsonTokenType tokenType)
        {
            if (!IsPropertyName(tokenType))
            {
                throw new JsonException();
            }            
        }

        static bool IsPropertyName(JsonTokenType tokenType)
            => tokenType == JsonTokenType.PropertyName;

        static bool IsErrorProperty(string propertyName)
            => propertyName.Equals(ErrorProperty, StringComparison.OrdinalIgnoreCase);

        static bool IsErrorMessageProperty(string propertyName)
            => propertyName.Equals(ErrorMessageProperty, StringComparison.OrdinalIgnoreCase);

        static bool IsTokenProperty(string propertyName)
            => propertyName.Equals(TokensProperty, StringComparison.OrdinalIgnoreCase);

        void ProcessTokens(ref Utf8JsonReader reader)
        {
            MoveToNextJsonElement(ref reader);

            ProcessTokenJsonProperties(ref reader);
        }

        void ProcessTokenJsonProperties(ref Utf8JsonReader reader)
        {
            while (!EndObject(reader.TokenType))
            {
                StoreTokenJsonPropertyValue(ref reader);
                MoveToNextJsonElement(ref reader);
            }
        }

        void StoreTokenJsonPropertyValue(ref Utf8JsonReader reader)
        {
            string propertyName = GetPropertyName(ref reader);
            ReadJsonElementValue(ref reader);

            if (IsAuthTokenProperty(propertyName))
            {
                _authToken = reader.GetString();
            }
            else if (IsRefreshTokenProperty(propertyName))
            {
                _refreshToken = reader.GetString();
            }
        }

        static bool IsAuthTokenProperty(string propertyName)
            => propertyName.Equals(AuthTokenProperty, StringComparison.OrdinalIgnoreCase);

        static bool IsRefreshTokenProperty(string propertyName)
            => propertyName.Equals(RefreshTokenProperty, StringComparison.OrdinalIgnoreCase);

        IdentityResult CreateIdentityResult()
        {
            if (_error is null)
            {
                return CreateSuccessResult();
            }

            return CreateErrorResult();

            IdentityResult CreateSuccessResult()
            {
                if (TokensHaveValue())
                {
                    return IdentityResult.Success(new TokenHolder { AuthToken = _authToken!, RefreshToken = _refreshToken! });
                }

                return IdentityResult.Success();
            }

            bool TokensHaveValue()
            {
                return _authToken is not null && _refreshToken is not null;
            }

            IdentityResult CreateErrorResult()
                => _error.Value switch
                {
                    IdentityError.UserExists => IdentityResult.FromError(IdentityError.UserExists, _errorMessage!),
                    IdentityError.InvalidUserRegistrationInput => IdentityResult.FromError(IdentityError.InvalidUserRegistrationInput, _errorMessage!),
                    IdentityError.InvalidCredentials => IdentityResult.FromError(IdentityError.InvalidCredentials, _errorMessage!),
                    IdentityError.UsedUpRefreshToken => IdentityResult.FromError(IdentityError.UsedUpRefreshToken, _errorMessage!),
                    IdentityError.RefreshTokenNotExists => IdentityResult.FromError(IdentityError.RefreshTokenNotExists, _errorMessage!),
                    IdentityError.NotExpiredToken => IdentityResult.FromError(IdentityError.NotExpiredToken, _errorMessage!),
                    IdentityError.JwtIdNotMatch => IdentityResult.FromError(IdentityError.JwtIdNotMatch, _errorMessage!),
                    IdentityError.InvalidatedRefreshToken => IdentityResult.FromError(IdentityError.InvalidatedRefreshToken, _errorMessage!),
                    IdentityError.ExpiredRefreshToken => IdentityResult.FromError(IdentityError.ExpiredRefreshToken, _errorMessage!),
                    IdentityError.InvalidJwt => IdentityResult.FromError(IdentityError.InvalidJwt, _errorMessage!),
                    _ => IdentityResult.ServerError()
                };
        }
    }    

    public override void Write(Utf8JsonWriter writer, IdentityResult result, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperties(writer, result, options);
        writer.WriteEndObject();

        static void WriteProperties(Utf8JsonWriter writer, IdentityResult result, JsonSerializerOptions options)
        {
            WriteError(writer, result);
            WriteErrorMessage(writer, result);
            WriteTokens(writer, result);
        }

        static void WriteError(Utf8JsonWriter writer, IdentityResult result)
        {
            if (result.Error is not null)
            {
                writer.WriteNumber(ErrorProperty, (int)result.Error);
            }
        }

        static void WriteErrorMessage(Utf8JsonWriter writer, IdentityResult result)
        {
            if (result.ErrorMessage is not null)
            {
                writer.WriteString(ErrorMessageProperty, result.ErrorMessage);
            }
        }

        static void WriteTokens(Utf8JsonWriter writer, IdentityResult result)
        {
            if (result.Tokens is not null)
            {
                writer.WriteStartObject(TokensProperty);
                writer.WriteString(AuthTokenProperty, result.Tokens.AuthToken);
                writer.WriteString(RefreshTokenProperty, result.Tokens.RefreshToken);
                writer.WriteEndObject();
            }
        }
    }
}