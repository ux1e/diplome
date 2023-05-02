using Source.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.Data;
using Source.View.Pages;

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
            }).GeneratePdf($"{DataClass.UserInfo.Name}({DataClass.GetDateTime()}).pdf");
        }

        private void bSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }

        private void bSZVTD_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SZVTDPage());
        }

        private void bReadXML_Click(object sender, RoutedEventArgs e)
        {
            DataSet dt = new DataSet();
            dt.Clear();
            dt.ReadXml("D:\\ПФР_123-456-789000_СЗВ-ТД_20230502_c51456e0-b486-4397-b56e-b9f7df5aec96.XML");
            MessageBox.Show(dt.ToString());
        }
    }
}
