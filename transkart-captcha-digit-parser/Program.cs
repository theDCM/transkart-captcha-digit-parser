using System;
using System.IO;
using SkiaSharp;

namespace transkart_captcha_digit_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string Original = @"C:\Users\thedc\Desktop\Капча Транспортная карта\Чистые капчи\1.jpg";
            string Output = @"C:\Users\thedc\Desktop\Капча Транспортная карта\Цифры\";
            HandleImage(Original, Output);
        }

        static void HandleImage(string Original, string Output)
        {
            SKBitmap Picture = SKBitmap.Decode(Original);
            SKBitmap[] Digits = new SKBitmap[4];
            SKBitmap[] CroppedDigits = new SKBitmap[4];
            SKImage Image = SKImage.FromBitmap(Picture);
            for (int i = 0; i < 4; i++)
            {
                Digits[i] = SKBitmap.FromImage(Image.Subset(new SKRectI(i * Picture.Width / 4, 0, (i + 1) * Picture.Width / 4, Picture.Height)));
                CroppedDigits[i] = Crop(Digits[i]);
                SKData data = SKImage.FromBitmap(CroppedDigits[i]).Encode(SKEncodedImageFormat.Jpeg, 100);
                FileStream stream = new FileStream(Output + i.ToString() + ".jpg", FileMode.Create, FileAccess.Write);
                data.SaveTo(stream);
                stream.Close();
                stream.Dispose();
                data.Dispose();
                Digits[i].Dispose();
                CroppedDigits[i].Dispose();
            }
            Image.Dispose();
            Picture.Dispose();
            Console.Write("The work has been completed. Press any key to exit..");
            Console.ReadKey(true);

            SKBitmap Crop(SKBitmap bitmap)
            {
                int width = bitmap.Width, height = bitmap.Height;
                int Left = 0, Top = 0, Right = 0, Bottom = 0;
                // Left
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        if (bitmap.GetPixel(i, j) == SKColors.Black)
                        {
                            Left = i;
                            i = width;
                            break;
                        }
                // Top
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        if (bitmap.GetPixel(j, i) == SKColors.Black)
                        {
                            Top = i;
                            i = height;
                            break;
                        }
                // Right
                for (int i = width; i > 0; i--)
                    for (int j = 0; j < height; j++)
                        if (bitmap.GetPixel(i, j) == SKColors.Black)
                        {
                            Right = i;
                            i = 0;
                            break;
                        }
                // Bottom
                for (int i = height; i > 0; i--)
                    for (int j = 0; j < width; j++)
                        if (bitmap.GetPixel(j, i) == SKColors.Black)
                        {
                            Bottom = i;
                            i = 0;
                            break;
                        }
                return SKBitmap.FromImage(SKImage.FromBitmap(bitmap).Subset(new SKRectI(Left, Top, Right, Bottom)));
            }
        }
    }
}