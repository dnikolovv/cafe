using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cafe.Api.OperationFilters
{
    /// <summary>
    /// This operation filter only works on <see cref="Optional.Option"/> types that are contained in a class.
    /// Optional values that are put as type parameters will not be properly displayed in Swagger, although they will work.
    /// </summary>
    public class OptionOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                if (parameter.Name.EndsWith(".HasValue") && parameter is NonBodyParameter nonBodyParameter)
                {
                    nonBodyParameter.Name = parameter.Name.Substring(0, parameter.Name.Length - ".HasValue".Length);
                    nonBodyParameter.Type = "string";
                }
            }
        }
    }
}
