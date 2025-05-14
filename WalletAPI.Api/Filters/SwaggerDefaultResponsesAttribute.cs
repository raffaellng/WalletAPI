using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WalletAPI.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerDefaultResponsesAttribute : Attribute, IOperationFilter
    {
        private readonly Type _responseType;

    public SwaggerDefaultResponsesAttribute(Type responseType)
    {
        _responseType = responseType;
    }

    public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAttribute = context.MethodInfo.GetCustomAttributes<SwaggerDefaultResponsesAttribute>().Any();
        if (!hasAttribute) return;

        operation.Responses.TryAdd("200", new Microsoft.OpenApi.Models.OpenApiResponse
        {
            Description = "Registros encontrados.",
            Content = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiMediaType>
            {
                ["application/json"] = new Microsoft.OpenApi.Models.OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(_responseType, context.SchemaRepository)
                }
            }
        });

        operation.Responses.TryAdd("401", new Microsoft.OpenApi.Models.OpenApiResponse
        {
            Description = "Usuário não autorizado"
        });

        operation.Responses.TryAdd("500", new Microsoft.OpenApi.Models.OpenApiResponse
        {
            Description = "Oops! Ocorreu um problema interno e não foi possível processar a requisição."
        });
    }
}
}
