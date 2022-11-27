// See https://aka.ms/new-console-template for more information
using ParPr_Lb7;

Console.WriteLine("Processing...");
var m = new Mandelbrot(5000, 5000, true);
m.Result.Save(@"C:\Users\Sasha\Desktop\Mandelbrot2.bmp");
Console.WriteLine($"Done in {m.ProcessingTime:F4} seconds");
