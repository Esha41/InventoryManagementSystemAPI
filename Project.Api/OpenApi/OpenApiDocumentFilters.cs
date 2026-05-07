namespace Ettad.Api.OpenApi;

public sealed class EnumAsStringSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema? schema, SchemaFilterContext context)
    {
        try
        {
            if (context.Type is { IsEnum: true } && schema != null)
            {
                var enumValues = Enum.GetValues(context.Type);
                if (enumValues is { Length: > 0 })
                {
                    schema.Type = "string";
                    schema.Format = null;
                    schema.Enum = new List<IOpenApiAny>();
                    foreach (var enumValue in enumValues)
                    {
                        if (enumValue != null)
                            schema.Enum.Add(new OpenApiString(enumValue.ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"EnumAsStringSchemaFilter error: {ex.Message}");
        }
    }
}

public sealed class EnumAsStringParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter? parameter, ParameterFilterContext context)
    {
        try
        {
            if (context.ParameterInfo?.ParameterType is not { } paramType)
                return;

            if (paramType == typeof(IFormFile) || paramType == typeof(List<IFormFile>))
                return;

            if (paramType.IsEnum && parameter != null)
            {
                var enumValues = Enum.GetValues(paramType);
                if (enumValues is { Length: > 0 })
                {
                    var enumList = new List<IOpenApiAny>();
                    foreach (var enumValue in enumValues)
                    {
                        if (enumValue != null)
                            enumList.Add(new OpenApiString(enumValue.ToString()));
                    }

                    parameter.Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = enumList
                    };
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"EnumAsStringParameterFilter error: {ex.Message}");
        }
    }
}

public sealed class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) ||
                        p.ParameterType == typeof(List<IFormFile>) ||
                        (p.ParameterType.IsGenericType &&
                         p.ParameterType.GetGenericTypeDefinition() == typeof(List<>) &&
                         p.ParameterType.GetGenericArguments()[0] == typeof(IFormFile)))
            .ToList();

        var otherFormParameters = context.MethodInfo.GetParameters()
            .Where(p => p.GetCustomAttributes(typeof(FromFormAttribute), false).Length != 0 &&
                        p.ParameterType != typeof(IFormFile) &&
                        p.ParameterType != typeof(List<IFormFile>) &&
                        !(p.ParameterType.IsGenericType &&
                          p.ParameterType.GetGenericTypeDefinition() == typeof(List<>) &&
                          p.ParameterType.GetGenericArguments()[0] == typeof(IFormFile)))
            .ToList();

        if (!fileParameters.Any() && !otherFormParameters.Any())
            return;

        if (operation.Parameters != null)
        {
            var allFormParams = fileParameters.Concat(otherFormParameters).ToList();
            operation.Parameters = operation.Parameters
                .Where(p => !allFormParams.Any(fp =>
                    string.Equals(fp.Name, p.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        operation.RequestBody ??= new OpenApiRequestBody();
        operation.RequestBody.Content ??= new Dictionary<string, OpenApiMediaType>();

        if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
        {
            operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>(),
                    Required = new HashSet<string>()
                }
            };
        }

        var formDataSchema = operation.RequestBody.Content["multipart/form-data"].Schema!;
        formDataSchema.Properties ??= new Dictionary<string, OpenApiSchema>();
        formDataSchema.Required ??= new HashSet<string>();

        foreach (var param in fileParameters)
        {
            var isList = param.ParameterType == typeof(List<IFormFile>) ||
                         (param.ParameterType.IsGenericType &&
                          param.ParameterType.GetGenericTypeDefinition() == typeof(List<>));

            var schema = isList
                ? new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema { Type = "string", Format = "binary" }
                }
                : new OpenApiSchema { Type = "string", Format = "binary" };

            formDataSchema.Properties[param.Name] = schema;

            if (!param.IsOptional)
                formDataSchema.Required.Add(param.Name);
        }

        foreach (var param in otherFormParameters)
        {
            var schema = CreateSchemaForFormParameter(param.ParameterType);
            formDataSchema.Properties[param.Name] = schema;

            if (!param.IsOptional && !IsNullableType(param.ParameterType))
                formDataSchema.Required.Add(param.Name);
        }
    }

    private static OpenApiSchema CreateSchemaForFormParameter(Type parameterType)
    {
        if (parameterType == typeof(string))
            return new OpenApiSchema { Type = "string" };
        if (parameterType == typeof(int) || parameterType == typeof(long))
            return new OpenApiSchema { Type = "integer", Format = parameterType == typeof(long) ? "int64" : "int32" };
        if (parameterType == typeof(bool))
            return new OpenApiSchema { Type = "boolean" };
        if (parameterType == typeof(DateTime) || parameterType == typeof(DateTime?))
            return new OpenApiSchema { Type = "string", Format = "date-time" };
        return new OpenApiSchema { Type = "object" };
    }

    private static bool IsNullableType(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
}
