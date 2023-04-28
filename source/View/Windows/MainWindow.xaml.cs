using source.Core;
using System.Windows;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System;

namespace source
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = $"Создание отчётов | {DataClass.UserInfo.Name}";
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
    }
}
