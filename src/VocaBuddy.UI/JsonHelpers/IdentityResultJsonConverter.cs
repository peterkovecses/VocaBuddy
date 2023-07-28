using System.Text.Json;
using System.Text.Json.Serialization;

namespace VocaBuddy.UI.JsonHelpers;

public class IdentityResultJsonConverter : JsonConverter<IdentityResult>
{
    public const string StatusProperty = "status";
    public const string ErrorMessageProperty = "errorMessage";
    public const string TokensProperty = "tokens";
    public const string AuthTokenProperty = "authToken";
    public const string RefreshTokenProperty = "refreshToken";

    private IdentityResultStatus? _status;
    private string? _errorMessage;
    private string? _authToken;
    private string? _refreshToken;

    public override IdentityResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ThrowIfNotStartObject(reader);
        MoveToNextJsonElement(ref reader);
        ProcessJsonProperties(ref reader, options);
        ValidateStatus(_status);

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

            if (IsStatusProperty(propertyName))
            {
                _status = (IdentityResultStatus)reader.GetInt32();
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

        static bool IsStatusProperty(string propertyName)
            => propertyName.Equals(StatusProperty, StringComparison.OrdinalIgnoreCase);

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

        static void ValidateStatus(IdentityResultStatus? status)
        {
            if (status is null)
            {
                throw new JsonException($"The '{StatusProperty}' field is required.");
            }
        }

        IdentityResult CreateIdentityResult()
            => _status!.Value switch
            {
                IdentityResultStatus.Success => IdentityResult.Success(GetTokenHolder()),
                IdentityResultStatus.UserExists => IdentityResult.UserExists(_errorMessage),
                IdentityResultStatus.InvalidUserRegistrationInput => IdentityResult.InvalidUserRegistrationInput(_errorMessage),
                IdentityResultStatus.InvalidCredentials => IdentityResult.InvalidCredentials(_errorMessage),
                IdentityResultStatus.UsedUpRefreshToken => IdentityResult.UsedUpRefreshToken(_errorMessage),
                IdentityResultStatus.RefreshTokenNotExists => IdentityResult.RefreshTokenNotExists(_errorMessage),
                IdentityResultStatus.NotExpiredToken => IdentityResult.NotExpiredToken(_errorMessage),
                IdentityResultStatus.JwtIdNotMatch => IdentityResult.JwtIdNotMatch(_errorMessage),
                IdentityResultStatus.InvalidatedRefreshToken => IdentityResult.InvalidatedRefreshToken(_errorMessage),
                IdentityResultStatus.ExpiredRefreshToken => IdentityResult.ExpiredRefreshToken(_errorMessage),
                IdentityResultStatus.InvalidJwt => IdentityResult.InvalidJwt(_errorMessage),
                _ => IdentityResult.Error(_errorMessage ?? IdentityResult.DefaultErrorMessage),
            };

        TokenHolder? GetTokenHolder()
            => _authToken is not null && _refreshToken is not null
                ? new TokenHolder { AuthToken = _authToken, RefreshToken = _refreshToken }
                : null;
    }    

    public override void Write(Utf8JsonWriter writer, IdentityResult value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperties(writer, value, options);
        writer.WriteEndObject();

        static void WriteProperties(Utf8JsonWriter writer, IdentityResult value, JsonSerializerOptions options)
        {
            WriteStatus(writer, value);
            WriteErrorMessage(writer, value);
            WriteTokens(writer, value);
        }

        static void WriteStatus(Utf8JsonWriter writer, IdentityResult value)
        {
            writer.WriteNumber(StatusProperty, (int)value.Status);
        }

        static void WriteErrorMessage(Utf8JsonWriter writer, IdentityResult value)
        {
            if (value.ErrorMessage is not null)
            {
                writer.WriteString(ErrorMessageProperty, value.ErrorMessage);
            }
        }

        static void WriteTokens(Utf8JsonWriter writer, IdentityResult value)
        {
            if (value.Tokens is not null)
            {
                writer.WriteStartObject(TokensProperty);
                writer.WriteString(AuthTokenProperty, value.Tokens.AuthToken);
                writer.WriteString(RefreshTokenProperty, value.Tokens.RefreshToken);
                writer.WriteEndObject();
            }
        }
    }
}