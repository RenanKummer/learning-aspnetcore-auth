using Serilog.Events;
using Serilog.Formatting.Json;

namespace Learning.AspNetCoreAuth.WebApi.Formatters;

/// <summary>
/// Serilog JSON formatter that skips null properties when destructuring objects in log messages.
/// </summary>
public class SerilogNullJsonValueFormatter() : JsonValueFormatter(TypePropertyName)
{
    private const string TypePropertyName = "$type";

    protected override bool VisitStructureValue(TextWriter state, StructureValue structure)
    {
        var filteredProperties = structure.Properties.Where(property => NonNullScalarValue(property.Value)).ToList();
        return base.VisitStructureValue(state, new StructureValue(filteredProperties, structure.TypeTag));
    }

    protected override bool VisitDictionaryValue(TextWriter state, DictionaryValue dictionary)
    {
        var filteredProperties = dictionary.Elements.Where(property => NonNullScalarValue(property.Value)).ToList();
        return base.VisitDictionaryValue(state, new DictionaryValue(filteredProperties));
    }

    private static bool NonNullScalarValue(LogEventPropertyValue? value) =>
        value is ScalarValue { Value: not null } or not ScalarValue;
}