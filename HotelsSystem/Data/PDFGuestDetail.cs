using Microsoft.AspNetCore;
using Microsoft.Extensions.Localization;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using static HotelsSystemClient.Data.QuEx;

namespace HotelsSystemClient.Data;

public class PDFGuestDetail
{
    public static async Task<byte[]> GenerateGuestDetailPDF(GuestDetailsInfo SelectedGuest, IEnumerable<AttachmentInfo> Attachments,IStringLocalizer<HotelsSystem.App> L,NavigationManager nav, HttpClient httpClient, IWebHostEnvironment webHost)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;


        List<byte[]> ItemsImages = new List<byte[]>();
            foreach (var itm in Attachments.Select((value, i) => new { i, value }))
            {
                var image = await QuEx.GetImage(nav, httpClient, itm.value.att_ID, "GetAttachment");
            if(image!=null)
                ItemsImages.Add(image);
            }
        var document = Document.Create(container =>
        {
            string fontPath = Path.Combine(webHost.WebRootPath, "font/PFDinTextUniversal-Regular.otf");
            string fontPath2 = Path.Combine(webHost.WebRootPath, "font/PFDinTextUniversal-Bold.otf");
            string logo = Path.Combine(webHost.WebRootPath, "img/KRI_TRIP.svg");

            FontManager.RegisterFont(File.OpenRead(fontPath));
            FontManager.RegisterFont(File.OpenRead(fontPath2));
            container.Page(page =>
            {
                int PaperMargin = 10;
                page.Size(PageSizes.A4);
                page.Margin(PaperMargin);
                page.DefaultTextStyle(_TextStyle);
                page.Header().Row(header =>
                {
                    //header.Spacing(2);
                    // Left section: English Text
                    header.RelativeItem().AlignLeft().AlignMiddle().Column(leftColumn =>
                    {
                        leftColumn.Item().PaddingBottom(2).Text("1").FontSize(14).Bold();
                        leftColumn.Item().PaddingBottom(2).Text("2").FontSize(14).Bold();
                        leftColumn.Item().PaddingBottom(2).Text("3").FontSize(14).Bold();
                    });

                    //// Middle section: Logo
                    header.ConstantItem(100).AlignCenter().Image(logo);

                    header.RelativeItem().AlignRight().AlignMiddle().ContentFromRightToLeft().Column(rightColumn =>
                    {
                        rightColumn.Item().PaddingBottom(2).Text("1").FontSize(14).Bold();
                        rightColumn.Item().PaddingBottom(2).Text("2").FontSize(14).Bold();
                        rightColumn.Item().PaddingBottom(2).Text("3").FontSize(14).Bold();
                    });
                });

                page.Content().pageIsEnglish().Column(column =>
                {
                    column.Item().PaddingVertical(10).LineHorizontal(0.5f);
                    column.Item().AlignCenter().Table(col =>
                    {
                        col.ColumnsDefinition(c => { c.RelativeColumn(1); c.RelativeColumn(1); });
                        if (!string.IsNullOrWhiteSpace(SelectedGuest.GD_Fullname))
                        {
                            col.Cell().Ltext(L, "full-name", SelectedGuest.GD_Fullname);
                        }

                        if (!string.IsNullOrWhiteSpace(SelectedGuest.GD_MotherName))
                        {
                            col.Cell().Ltext(L, "mother-name", SelectedGuest.GD_MotherName);
                        }

                        if (SelectedGuest.GD_DOB.HasValue)
                        {
                            col.Cell().Ltext(L, "birth-date", SelectedGuest.GD_DOB.ToddMMyyyy());
                        }

                        if (!string.IsNullOrWhiteSpace(SelectedGuest.nat_Name))
                        {
                            col.Cell().Ltext(L, "nationality", SelectedGuest.nat_Name);
                        }

                        if (!string.IsNullOrWhiteSpace(SelectedGuest.gen_Name))
                        {
                            col.Cell().Ltext(L, "gender", SelectedGuest.gen_Name);
                        }

                        if (!string.IsNullOrWhiteSpace(SelectedGuest.GD_Mobile))
                        {
                            col.Cell().Ltext(L, "mobile-no", SelectedGuest.GD_Mobile);
                        }

                        if (!string.IsNullOrWhiteSpace(SelectedGuest.GD_Email))
                        {
                            col.Cell().Ltext(L, "email", SelectedGuest.GD_Email);
                        }

                        if (!string.IsNullOrWhiteSpace(SelectedGuest.GD_IdNumber))
                        {
                            col.Cell().Ltext(L, "id-no", SelectedGuest.GD_IdNumber);
                        }


                    });
                    if (ItemsImages.Any())
                    {
                        column.Item().PaddingVertical(10).LineHorizontal(0.5f);


                        column.Item().Table(imgCol =>
                        {

                            imgCol.ColumnsDefinition(c => { c.RelativeColumn(1); c.RelativeColumn(1); });
                            imgCol.Header(e =>
                            {
                                foreach (var item in ItemsImages)
                                {
                                    e.Cell().AlignCenter().Padding(10).MaxHeight(100).MaxWidth(PageSizes.A4.Width/2-(PaperMargin*2)).Image(item);
                                }
                            });
                        });
                    }
                });

            });


        });
        // instead of the standard way of generating a PDF file
      return document.GeneratePdf();

        // use the following invocation
        //document.ShowInCompanion();
    }
    
}
