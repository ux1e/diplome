using source.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace source.View.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void bMakeReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("MM-dd-yyyy HH.mm.ss");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Вау! Это что, ОТЧЁТ!??!!?")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium).FontFamily("Arial");

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf($"{DataClass.UserInfo.Name}({formattedDateTime}).pdf");
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
