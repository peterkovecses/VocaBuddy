using System.Text.Json.Serialization;
using System.Text.Json;

namespace VocaBuddy.UI.JsonHelpers;

public class TokenHolderJsonConverter : JsonConverter<TokenHolder>
{
    public override TokenHolder Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? authToken = null;
        string? refreshToken = null;

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

                if (propertyName.Equals("authToken", StringComparison.OrdinalIgnoreCase))
                {
                    authToken = reader.GetString();
                }
                else if (propertyName.Equals("refreshToken", StringComparison.OrdinalIgnoreCase))
                {
                    refreshToken = reader.GetString();
                }
            }
        }

        return new TokenHolder
        {
            AuthToken = authToken ?? throw new JsonException("The 'authToken' field is required."),
            RefreshToken = refreshToken ?? throw new JsonException("The 'refreshToken' field is required.")
        };
    }

    public override void Write(Utf8JsonWriter writer, TokenHolder value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("authToken", value.AuthToken);
        writer.WriteString("refreshToken", value.RefreshToken);
        writer.WriteEndObject();
    }
}
