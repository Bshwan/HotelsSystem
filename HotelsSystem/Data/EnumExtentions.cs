namespace HotelsSystem.Data;
public static class EnumExtentions
{
    public static async Task<IEnumerable<T>> SearchAll<T>(this IEnumerable<T> data, string value, string ColumnName)
    {
        var SearchedData = data.Where(x => x!.GetType()!.GetProperty(ColumnName)!.GetValue(x)!.ToString().ToEmptyOnNull().ContainsIgnoreCase(value.ToEmptyOnNull()));
        return await Task.FromResult(SearchedData);
    }
    public static T SelectByID<T>(this IEnumerable<T> data, int id, string FindByColumn)
    {
        var SearchedData = data.Where(x => x!.GetType()!.GetProperty(FindByColumn)!.GetValue(x)!.ToString().ToEmptyOnNull().Equals(id.ToString()));
        if (SearchedData.Any())
            return SearchedData.First();
        else
            return Activator.CreateInstance<T>();
    }
    public static string ValidateField<T>(this T val, string ValidateColumn)
        {
            if (val == null)
                return "required";

            var obj = val.GetType().GetProperty(ValidateColumn)!.GetValue(val);

            switch (Type.GetTypeCode(obj!.GetType()))
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return decimal.Parse(obj.ToString().ToEmptyOnNull()) <= decimal.Zero ? "required" : null!;
                case TypeCode.String:
                    return obj.ToString().IsStringNullOrWhiteSpace() ? "required" : null!;
                default:
                    return null!;
            }
        }
}