using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PropertEase
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.MethodInfo
                .GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(List<IFormFile>))
                .ToArray();

            if (fileParameters.Length == 0)
            {
                return;
            }

            operation.Parameters = operation.Parameters ?? new List<OpenApiParameter>();

            foreach (var parameter in fileParameters)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = parameter.Name,
                    In = ParameterLocation.Query,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                });
            }

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = fileParameters.ToDictionary(
                                parameter => parameter.Name,
                                parameter => new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            ),
                            Required = fileParameters.Select(parameter => parameter.Name).ToHashSet()
                        }
                    }
                }
            };
        }
    }

}