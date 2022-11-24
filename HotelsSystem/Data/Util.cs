using Microsoft.AspNetCore.Components.Web;

namespace HotelsSystem.Data
{
    public class Util
    {
        public const string NeutralCurFormat = "#,##0.##";
        public const string DollarSymbol = "$";
        public const string DinarSymbol = "IQD";
        public const string JSCloseOffcanvas = "HideOffcanvas";
        public const string JSShowOffcanvas = "ShowOffcanvas";
        public const string JSCloseModal = "HideModal";
        public const string JSShowModal = "ShowModal";
        public const string CookieName = "cookieName";
        public const string SecurityGuid = "b65d5f8d-be2a-41d4-a9c6-aa08e9154ba5";

        public static bool IsEnterPressed(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
                return true;
            else
                return false;
        }
        public static string ReturnEmptyOnZero(int val)
        {
            return val <= 0 ? "" : val.ToString();
        }

        public static string ReturnEmptyOnZero(decimal val)
        {
            return val <= decimal.Zero ? "" : val.ToString();
        }

        public static string ReturnEmptyOnMinusOne(int val)
        {
            return val == -1 ? "" : val.ToString();
        }
        public static string ValidateField(bool bool1, string? val)
        {
            if (bool1 && string.IsNullOrWhiteSpace(val))
                return "custom-validation";
            else
                return "";
        }

        public static string ValidateField(bool bool1, byte[]? val)
        {
            if (bool1 && (val == null || val.Length <= 0))
                return "custom-validation";
            else
                return "";
        }

        public static string ValidateField(bool bool1, bool val)
        {
            if (bool1 && val)
                return "custom-validation";
            else
                return "";
        }

        public static string ValidateField(bool bool1, DateTime? val)
        {
            if (bool1 && !val.HasValue)
                return "custom-validation";
            else
                return "";
        }

        public static string ValidateField(bool bool1, int val)
        {
            if (bool1 && val <= 0)
                return "custom-validation";
            else
                return "";
        }

        public static string ValidateFieldAccCode(bool bool1, int val)
        {
            if (bool1 && val <= -1)
                return "custom-validation";
            else
                return "";
        }

        public static string ValidateField(bool bool1, decimal val)
        {
            if (bool1 && val <= decimal.Zero)
                return "custom-validation";
            else
                return "";
        }
        public async Task<IEnumerable<T>> SearchAll<T>(string value, string ColumnName, IEnumerable<T> data)
        {
            var SearchedData = data.Where(x => x!.GetType()!.GetProperty(ColumnName)!.GetValue(x)!.ToString().ToEmptyOnNull().ContainsIgnoreCase(value.ToEmptyOnNull()));
            return await Task.FromResult(SearchedData);
        }
    }
}