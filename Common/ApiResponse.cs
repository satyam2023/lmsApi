using System.Text.Json.Serialization;
namespace lmsApi.ApiResponse;
public class ApiResponse<T>
{
    public int StatusCode { get; set; }

    [JsonPropertyName("isSuccess")]
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore] 
    public string? Error { get; set; }  

    [JsonIgnore] 
    public List<string>? Errors { get; set; } 

    [JsonPropertyName("errors")]
    public List<string>? SerializedErrors => !IsSuccess
        ? Errors ?? (Error != null ? new List<string> { Error } : null)
        : null;

    public ApiResponse() { }

    public ApiResponse(int statusCode, string error)
    {
        StatusCode = statusCode;
        Error = error;
    }

    public ApiResponse(int statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public ApiResponse(int statusCode, T? data = default, string? message = null)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message;
    }
}
