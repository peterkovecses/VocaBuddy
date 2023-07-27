using System.Text.Json.Serialization;
using System.Text.Json;

namespace VocaBuddy.UI.JsonHelpers;

public class IdentityResultJsonConverter : JsonConverter<IdentityResult>
{
    public override IdentityResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        IdentityResultStatus? status = null;
        string? errorMessage = null;
        TokenHolder? data = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();

                if (propertyName.Equals("status", StringComparison.OrdinalIgnoreCase))
                {
                    status = (IdentityResultStatus)reader.GetInt32();
                }
                else if (propertyName.Equals("errorMessage", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = reader.GetString();
                }
                else if (propertyName.Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    data = JsonSerializer.Deserialize<TokenHolder>(ref reader, options);
                }
            }
        }

        if (!status.HasValue)
        {
            throw new JsonException("The 'Status' field must be present in the IdentityResult JSON token as a required element.");
        }

        return status.Value switch
        {
            IdentityResultStatus.Success => data is not null ? IdentityResult.Success(data) : IdentityResult.Success(),
            IdentityResultStatus.UserExists => IdentityResult.UserExists(errorMessage!),
            IdentityResultStatus.InvalidUserRegistrationInput => IdentityResult.InvalidUserRegistrationInput(errorMessage!),
            IdentityResultStatus.InvalidCredentials => IdentityResult.InvalidCredentials(errorMessage!),
            IdentityResultStatus.UsedUpRefreshToken => IdentityResult.UsedUpRefreshToken(errorMessage!),
            IdentityResultStatus.RefreshTokenNotExists => IdentityResult.RefreshTokenNotExists(errorMessage!),
            IdentityResultStatus.NotExpiredToken => IdentityResult.NotExpiredToken(errorMessage!),
            IdentityResultStatus.JwtIdNotMatch => IdentityResult.JwtIdNotMatch(errorMessage!),
            IdentityResultStatus.InvalidatedRefreshToken => IdentityResult.InvalidatedRefreshToken(errorMessage!),
            IdentityResultStatus.ExpiredRefreshToken => IdentityResult.ExpiredRefreshToken(errorMessage!),
            IdentityResultStatus.InvalidJwt => IdentityResult.InvalidJwt(errorMessage!),
            _ => IdentityResult.Error(errorMessage ?? IdentityResult.DefaultErrorMessage),
        };
    }

    public override void Write(Utf8JsonWriter writer, IdentityResult value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("status", (int)value.Status);

        if (value.ErrorMessage != null)
        {
            writer.WriteString("errorMessage", value.ErrorMessage);
        }

        if (value.Data != null)
        {
            writer.WritePropertyName("data");
            JsonSerializer.Serialize(writer, value.Data, options);
        }

        writer.WriteEndObject();
    }
}
