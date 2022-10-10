using PDFTester.Mock;
using PDFWriter;
using System.Drawing;
using System.Text;

Console.WriteLine("********** PDF Helper Library V1.0 *****************");
Console.WriteLine();

//Uncommend the below line to test simple PDF Write
//CreateSimplePdfFile();

CreateAdvancedPdfFile();

/*CreatePdfFile - Generates the PDF formated file based on the given specifications and content, text and image.
 * */
static void CreateSimplePdfFile()
{
    //variable declarations  
    var textContent = "PDF Writer/Reader Library V1.0";
    var imagePath = @""; // Example - C:\Mohan\Mohan.jpeg
    var fileName = "Demo_{0}.pdf";

    try
    {
        //TODO: Validte the input content before create the content
        using (PdfDocument document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, String.Format(fileName, System.Guid.NewGuid().ToString())))
        {
            // add new page
            PdfPage Page = new PdfPage(document);

            // add contents to page
            PdfContents Contents = new PdfContents(Page);

            // create text content font
            PdfFont ArialNormal = PdfFont.CreatePdfFont(document, "Arial", FontStyle.Regular, true);
            PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 18.0);

            // load text content
            if (!string.IsNullOrEmpty(textContent))
            {
                TextCtrl.Justify = TextJustify.Left;
                Contents.DrawText(TextCtrl, 1, 7, textContent);
            }

            // load image content
            if (!string.IsNullOrEmpty(imagePath))
            {
                PdfImage Image = new PdfImage(document);
                Image.LoadImage(imagePath);

                // draw image
                PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
                DrawCtrl.Paint = DrawPaint.Fill;
                DrawCtrl.BackgroundTexture = Image;
                Contents.DrawGraphics(DrawCtrl, new PdfRectangle(3.5, 4.8, 5.5, 6.8));
            }
            // create pdf file
            document.CreateFile();
            Console.WriteLine("********** PDF Document Created Successfully *****************");
            Console.WriteLine("Please find the document in the path ../PDFTester/bin/Debug/net6.0");
        }
    }
    catch (Exception ex)
    {
        //TODO: log the exception and retruns the custom error with error code. This is required for future tracking with errorcode
        Console.WriteLine(ex.Message);
    }
    Console.Read();
}

/*CreatePdfFile - Generates the PDF formated file with advanced options including table and watermark based on the given specifications and content.
 * */
static void CreateAdvancedPdfFile()
{
    var fileName = "DemoAdvanced_{0}.pdf";
    try
    {
        using (PdfDocument document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, String.Format(fileName, System.Guid.NewGuid().ToString())))
        {
            var pdflink = "http://mohankitusamy.com";          
            var imagelink = @""; // Example - C:\Mohan\Mohan.jpeg

            // Define font resources
            // Arguments: PdfDocument class, font family name, font style, embed flag
            // Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
            // Embed font. If true, the font file will be embedded in the PDF file.
            // If false, the font will not be embedded
            var FontName1 = "Arial";
            var FontName2 = "Times New Roman";

            var ArialNormal = PdfFont.CreatePdfFont(document, FontName1, FontStyle.Regular, true);
            var ArialBold = PdfFont.CreatePdfFont(document, FontName1, FontStyle.Bold, true);
            var ArialItalic = PdfFont.CreatePdfFont(document, FontName1, FontStyle.Italic, true);
            var ArialBoldItalic = PdfFont.CreatePdfFont(document, FontName1, FontStyle.Bold | FontStyle.Italic, true);
            var TimesNormal = PdfFont.CreatePdfFont(document, FontName2, FontStyle.Regular, true);
            var Comic = PdfFont.CreatePdfFont(document, "Comic Sans MS", FontStyle.Bold, true);


            // create empty tiling pattern
            var WaterMark = new PdfTilingPattern(document);

            // the pattern will be PdfFileWriter laied out in brick pattern
            string Mark = "PdfFileWriter II";

            // text width and height for Arial bold size 18 points
            PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 18.0);
            double TextWidth = TextCtrl.TextWidth(Mark);
            double TextHeight = TextCtrl.LineSpacing;

            // text base line
            double BaseLine = TextCtrl.TextDescent;

            // the overall pattern box (we add text height value as left and right text margin)
            double BoxWidth = TextWidth + 2 * TextHeight;
            double BoxHeight = 4 * TextHeight;
            WaterMark.SetTileBox(BoxWidth, BoxHeight);

            // fill the pattern box with background light blue color
            PdfRectangle Rect = new PdfRectangle(0, 0, BoxWidth, BoxHeight);
            PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
            DrawCtrl.Paint = DrawPaint.Fill;
            DrawCtrl.BackgroundTexture = Color.FromArgb(230, 244, 255);
            WaterMark.DrawGraphics(DrawCtrl, Rect);

            // draw PdfFileWriter at the bottom center of the box
            TextCtrl.TextColor = Color.White;
            TextCtrl.Justify = TextJustify.Center;
            WaterMark.DrawText(TextCtrl, BoxWidth / 2, BaseLine, Mark);

            // adjust base line upward by half height
            BaseLine += BoxHeight / 2;

            // draw the right half of PdfFileWriter shifted left by half width
            WaterMark.DrawText(TextCtrl, 0.0, BaseLine, Mark);

            // draw the left half of PdfFileWriter shifted right by half width
            WaterMark.DrawText(TextCtrl, BoxWidth, BaseLine, Mark);

            // Step 3: Add new page
            PdfPage Page = new PdfPage(document);

            // Step 4: Add contents to page
            PdfContents Contents = new PdfContents(Page);

            // Step 5: add graphices and text contents to the contents object

            // rectangle position: x=1.0", y=1.0", width=6.5", height=9.0"
            PdfRectangle Rect1 = new PdfRectangle(1, 1, 7.5, 10);
            PdfDrawCtrl DrawCtrl1 = new PdfDrawCtrl();
            DrawCtrl1.Paint = DrawPaint.BorderAndFill;
            DrawCtrl1.BorderWidth = 0.02;
            DrawCtrl1.BorderColor = Color.DarkBlue;
            DrawCtrl1.BackgroundTexture = WaterMark;
            Contents.DrawGraphics(DrawCtrl1, Rect1);

            // draw article name under the frame
            // Note: the \u00a4 is character 164 that was substituted during Font resource definition
            // this character is a solid circle it is normally Unicode 9679 or \u25cf in the Arial family
            PdfDrawTextCtrl TextCtrl1 = new PdfDrawTextCtrl(ArialNormal, 9);
            Contents.DrawText(TextCtrl1, 1.1, 0.85, "PDF WRITER Version 1.0 \u25cf PDF Writer C# Class Library \u25cf Author: Mohan");

            // draw web link to the article
            TextCtrl1.Justify = TextJustify.Right;
            TextCtrl1.Annotation = new PdfAnnotWebLink(document, pdflink);
            Contents.DrawText(TextCtrl1, 7.4, 0.85, "Click to view article");


            PdfDrawTextCtrl TextCtrl2 = new PdfDrawTextCtrl(Comic, 40);
            TextCtrl2.Justify = TextJustify.Center;
            TextCtrl2.TextColor = Color.FromArgb(255, 0, 128);
            Contents.DrawText(TextCtrl2, 4.25, 9.25, 0.02, Color.FromArgb(128, 0, 255), "PDF WRITER V 1.0");

            // Draw second line of heading text
            // arguments: Handwriting font, Font size 30 point, Position X=4.25", Y=9.0"
            // Text Justify: Center (text center will be at X position)
            TextCtrl2.FontSize = 30;
            TextCtrl2.TextColor = Color.Purple;
            Contents.DrawText(TextCtrl2, 4.25, 8.75, "Coding Example");

            // define local image resources
            // resolution 96 pixels per inch, image quality 50%
            if (!String.IsNullOrEmpty(imagelink))
            {
                PdfImage Image1 = new PdfImage(document);
                Image1.Resolution = 96.0;
                Image1.ImageQuality = 50;
                Image1.LoadImage(imagelink);

                // adjust image size and preserve aspect ratio
                PdfRectangle ImageArea = new PdfRectangle(3.6, 4.8, 5.55, 6.5);
                PdfRectangle NewArea = PdfImageSizePos.ImageArea(Image1, ImageArea, ContentAlignment.MiddleCenter);

                // clipping path
                PdfDrawCtrl DrawCtrl2 = new PdfDrawCtrl();
                DrawCtrl2.Shape = DrawShapes.Oval;
                DrawCtrl2.Paint = DrawPaint.Border;
                DrawCtrl2.BorderWidth = 0.04;
                DrawCtrl2.BorderColor = Color.DarkBlue;
                DrawCtrl2.BackgroundTexture = Image1;

                // draw image
                Contents.DrawGraphics(DrawCtrl2, NewArea);

                // save graphics state
                Contents.SaveGraphicsState();

                // translate origin to PosX=1.1" and PosY=1.1" this is the bottom left corner of the text box example
                Contents.Translate(1.1, 1.1);
            }

            // Define constants
            // Box width 3.25"
            // Box height is 3.65"
            // Normal font size is 9.0 points.
            const double Width = 3.15;
            const double Height = 3.65;
            const double FontSize = 9.0;

            // Create text box object width 3.25"
            // First line indent of 0.25"
            PdfTextBox Box = new PdfTextBox(Width, 0.25);

            PdfDrawTextCtrl TextCtrl3 = new PdfDrawTextCtrl(ArialNormal, FontSize);

            // add text to the text box
            Box.AddText(TextCtrl3,
                "Develop a C# .NET 6 Class Library  \n " +
                "Create a Console Application to test the functionality  \n " +
                "Design Considerations: \n " +
                "Should not use any external libraries  \n " );
                      

            // Draw the text box
            // Text left edge is at zero (note: origin was translated to 1.1") 
            // The top text base line is at Height less first line ascent.
            // Text drawing is limited to vertical coordinate of zero.
            // First line to be drawn is line zero.
            // After each line add extra 0.015".
            // After each paragraph add extra 0.05"
            // Stretch all lines to make smooth right edge at box width of 3.15"
            // After all lines are drawn, PosY will be set to the next text line after the box's last paragraph
            double PosY = Height;
            Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, Box);

            // restore graphics state
            Contents.RestoreGraphicsState();

            // Define constants to make the code readable
            const double Left = 4.35;
            const double Top = 4.65;
            const double Bottom = 1.1;
            const double Right = 7.4;
            const double MarginHor = 0.04;
            const double MarginVer = 0.04;
            const double FrameWidth = 0.015;
            const double GridWidth = 0.01;

            // normal text control
            const double FontSize4 = 9.0;
            PdfDrawTextCtrl TextCtrl4 = new PdfDrawTextCtrl(ArialNormal, FontSize4);

            // column widths
            double ColWidthPrice = TextCtrl4.TextWidth("9999.99") + 2.0 * MarginHor;
            double ColWidthQty = TextCtrl4.TextWidth("Qty") + 2.0 * MarginHor;
            double ColWidthDesc = Right - Left - FrameWidth - 3 * GridWidth - 2 * ColWidthPrice - ColWidthQty;

            // define table
            PdfTable Table = new PdfTable(Page, Contents, TextCtrl4);
            Table.TableArea = new PdfRectangle(Left, Bottom, Right, Top);
            Table.SetColumnWidth(new double[] { ColWidthDesc, ColWidthPrice, ColWidthQty, ColWidthPrice });

            // define borders
            Table.Borders.SetAllBorders(FrameWidth, GridWidth);

            // margin
            PdfRectangle Margin = new PdfRectangle(MarginHor, MarginVer);

            // default header style
            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.FromArgb(255, 196, 255);
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;

            // private header style for description
            Table.Header[0].Style = Table.HeaderStyle;
            Table.Header[0].Style.Alignment = ContentAlignment.MiddleLeft;

            // table heading
            Table.Header[2].Value = "Sno";
            Table.Header[0].Value = "Task";
            Table.Header[1].Value = "Desc";
            Table.Header[3].Value = " Hrs";

            // default style
            Table.DefaultCellStyle.Margin = Margin;

            // description column style
            Table.Cell[0].Style = Table.CellStyle;
            Table.Cell[0].Style.MultiLineText = true;

            // qty column style
            Table.Cell[2].Style = Table.CellStyle;
            Table.Cell[2].Style.Alignment = ContentAlignment.BottomRight;

            Table.DefaultCellStyle.Format = "#,##0.00";
            Table.DefaultCellStyle.Alignment = ContentAlignment.BottomRight;

            PdfDrawTextCtrl TextCtrlBold = new PdfDrawTextCtrl(ArialBold, FontSize4);
            TextCtrlBold.Justify = TextJustify.Center;
            TextCtrlBold.TextColor = Color.Purple;
            Contents.DrawText(TextCtrlBold, 0.5 * (Left + Right), Top + MarginVer + Table.DefaultCellStyle.TextDescent, "Example of PdfTable support");

            // reset order total
            double Total = 0;

            // loop for all items in the order
            // Order class is a atabase simulation for this example
            foreach (Milestone Task in Milestone.TaskList)
            {
                Table.Cell[0].Value = Task.WorkItem;
                Table.Cell[1].Value = Task.Description;
                Table.Cell[2].Value = Task.Sno;
                Table.Cell[3].Value = Task.Hrs;
                Table.DrawRow();

                // accumulate total
                Total += Task.Hrs;
            }
            Table.Close();

            // save graphics state
            Contents.SaveGraphicsState();

            // form line width 0.01"
            Contents.SetLineWidth(FrameWidth);
            Contents.SetLineCap(PdfLineCap.Square);

            // draw total before tax
            double[] ColumnPosition = Table.ColumnPosition;
            double TotalDesc = ColumnPosition[3] - MarginHor;
            double TotalValue = ColumnPosition[4] - MarginHor;
            double PosY4 = Table.RowTopPosition - 2.0 * MarginVer - Table.DefaultCellStyle.TextAscent;

            // draw total line
            PosY4 -= Table.DefaultCellStyle.TextDescent + 0.5 * MarginVer;
            LineD TotalLine = new LineD(ColumnPosition[3], PosY4, ColumnPosition[4], PosY4);
            Contents.DrawLine(TotalLine);

            // draw final total
            PosY4 -= Table.DefaultCellStyle.TextAscent + 0.5 * MarginVer;
            Contents.DrawText(TextCtrl4, TotalDesc, PosY4, "Hrs");          
            Contents.DrawText(TextCtrl4, TotalValue, PosY4, Total.ToString("#.00"));

            PosY4 -= Table.DefaultCellStyle.TextDescent + MarginVer;
            LineD VertLine1 = new LineD(ColumnPosition[0], Table.RowTopPosition, ColumnPosition[0], PosY4);
            Contents.DrawLine(VertLine1);
            LineD HorLine = new LineD(ColumnPosition[0], PosY4, ColumnPosition[4], PosY4);
            Contents.DrawLine(HorLine);
            LineD VertLine2 = new LineD(ColumnPosition[4], Table.RowTopPosition, ColumnPosition[4], PosY4);
            Contents.DrawLine(VertLine2);

            // restore graphics state
            Contents.RestoreGraphicsState();

            // Step 6: create pdf file
            document.CreateFile();
            Console.WriteLine("********** PDF Document Created Successfully *****************");
            Console.WriteLine("Please find the document in the path ../PDFTester/bin/Debug/net6.0");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    Console.Read();
}
