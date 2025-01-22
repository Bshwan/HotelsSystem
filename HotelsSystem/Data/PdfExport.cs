using Microsoft.Extensions.Localization;
using PdfReport;

namespace HotelsSystem.Data
{
    public class PdfExport
    {
        [Inject]
        protected IWebHostEnvironment webHost { get; set; } = default!;
        [Inject]
        protected HttpClient httpClient { get; set; } = default!;
        protected IToaster Toaster { get; set; }
        protected IStringLocalizer<App> L { get; set; }
        private ClS_Config config { get; set; }
        private NavigationManager nav { get; set; }
        bool isRTL = true;
        private string DefaultImagePath = "img/placeholder.png";


        public PdfExport(IToaster Toaster, IStringLocalizer<App> L, ClS_Config config, NavigationManager nav, bool isRTL = true)
        {
            this.config = config;
            this.nav = nav;
            this.isRTL = isRTL;
            this.Toaster = Toaster;
            this.L = L;
            var httpClientHandler = new HttpClientHandler();
            // Disable SSL certificate validation
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            httpClient = new HttpClient(httpClientHandler);

        }

        

        public async Task<Stream?> PdfTable(
     string ReportName,
     IEnumerable<object>? Items,
     List<PdfTotalInfo>? Totals,
     string[]? ColumnNames,
     int wp,
     List<PdfLegendInfo>? Legends = null,
     string LegendTitle = "",
     string[]? ColumnNamesNested1 = null,
     string[]? ColumnNamesNested2 = null)
        {
            

            if (Items == null)
                Items = Enumerable.Empty<object>();
            if (Totals == null)
                Totals = new List<PdfTotalInfo>();
            if (ColumnNames == null)
                ColumnNames = new string[] { };
            if (Legends == null)
                Legends = new List<PdfLegendInfo>();

            //List<string> titles = new List<string>();
            string compName = "", compJob = "", compAdd = "", phoneOne = "", phoneTwo = "", phoneThree = "", phoneFour = "", website = "", email = "";
            byte[]? TitleImage = null, RightImage = null, CenterImage = null, LeftImage = null;

            

            PdfCreaterQuest pdfcreater = new PdfCreaterQuest(
                //Titles: titles.ToArray(),
                IsHeaderImageTitle: false,
                //ImagePathTitle: TitleImage,
                //ImagePath1: RightImage,
                //ImagePath2: CenterImage,
                //ImagePath3: LeftImage,
                ImageHeight1: 150,
                ImageHeight2: 150,
                ImageHeight3: 150,
                ImageTitleHeight: 150,
                IsSheetRTL: isRTL,
                ReportName: ReportName
                //CssPath: "\\wwwroot\\css\\bootstrap\\pdfbootstrap.min.css"

                );

            string workingDirectory = Environment.CurrentDirectory;
            Stream? result = null;
            try
            {
                result = await pdfcreater.ExportPDF(
                    companyName: compName,
                    companyJob: compJob,
                    companyAddress: compAdd,
                    phoneOne: phoneOne,
                    phoneTwo: phoneTwo,
                    phoneThree: phoneThree,
                    phoneFour: phoneFour,
                    email: email,
                    website: website,
                items: new PdfTableWithTotal { Items = Items, Totals = Totals },
                Columns: ColumnNames,
                ColumnsNested1: ColumnNamesNested1,
                ColumnsNested2: ColumnNamesNested2,
                Legends: Legends,
                LegendHeaderText: LegendTitle,
                RegularFontPath: Path.Combine(workingDirectory, "wwwroot/font/PFDinTextUniversal-Regular.otf"),
                BoldFontPath: Path.Combine(workingDirectory, "wwwroot/font/PFDinTextUniversal-Bold.otf"),
                fontName: "PF Din Text Universal",
                imageTitle: TitleImage,
                image1: RightImage,
                image2: CenterImage,
                image3: LeftImage,
                PageThreshold: 1500

                );
            }
            catch (QuestPDF.Drawing.Exceptions.DocumentLayoutException ex)
            {
                Toaster.Error(".", L["number-page-more"]);
            }
            return result;
        }


    }
}
