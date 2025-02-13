using Microsoft.Extensions.Localization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace HotelsSystemClient.Data;

public static class QuEx
{
    public static TextStyle _TextStyle = new TextStyle().FontFamily("PF Din Text Universal").FontSize(12).LineHeight(1).LetterSpacing(0);

    public static IContainer pageIsEnglish(this IContainer container)
    {
        if (CultureInfo.CurrentUICulture.Name.Equals("en-US", StringComparison.OrdinalIgnoreCase))
        {
            return container.ContentFromLeftToRight();
        }
        else
        {
            return container.ContentFromRightToLeft();
        }
    }
    public static async Task<byte[]> GetImage(NavigationManager nav, HttpClient httpClient, int id, string controller)
    {
        string apiUrl = nav.BaseUri + $"main/{controller}?id={id}";

        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();

        }
        else
        {
            return null!;
        }

    }
    public static void Ltext(this IContainer container, IStringLocalizer<HotelsSystem.App> L, string label, string? val)
    {
            container.pageIsEnglish().PaddingTop(5).Inlined(rr =>
            {
                rr.Item().Text(L[label]);
                rr.Item().Text(":");
                rr.Item().Text(".").FontColor(QuestPDF.Helpers.Colors.Transparent);
                    rr.Item().Text(val.ToEmptyOnNull()).Style(_TextStyle.Bold());

            });
    }
}
