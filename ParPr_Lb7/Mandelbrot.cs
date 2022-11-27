using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace ParPr_Lb7;

public record Mandelbrot
{
    const int MaxIter = 100;

    Complex centre;
    double width;
    bool isParallel;
    int outputWidth;
    int outputHeight;

    Color[] colors;
    Bitmap output;
    double processingTime;

    public Mandelbrot(
        double realCentre, 
        double imaginedCentre, 
        double width, 
        int outputWidth, 
        int outputHeight,
        bool isParallel)
    {
        centre = new Complex(realCentre, imaginedCentre);
        this.width = width;
        this.isParallel = isParallel;
        this.outputWidth = outputWidth;
        this.outputHeight = outputHeight;
       
        colors = new Color[outputWidth * outputHeight];
        output = new Bitmap(outputWidth, outputHeight);
        FillColors();
        FillOutput();
    }

    public Mandelbrot(int outputWidth, int outputHeight, bool isParallel) 
        : this(-0.5, 0, 4, outputWidth, outputHeight, isParallel)
    {
    }

    public Bitmap Result => output;
    public double ProcessingTime => processingTime;

    private void FillColors()
    {
        long start = Stopwatch.GetTimestamp();
        if (isParallel)
        {
            ParallelFillColors();
        }
        else
        {
            SequentalFillColors();
        }
        long end = Stopwatch.GetTimestamp();

        processingTime = (end - start) / (double)TimeSpan.TicksPerSecond;
    }

    private void FillOutput()
    {
        Rectangle rect = new Rectangle(0, 0, outputWidth, outputHeight);
        System.Drawing.Imaging.BitmapData bmpData = output.LockBits(
            rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, output.PixelFormat);

        IntPtr ptr = bmpData.Scan0;
        int bytes = Math.Abs(bmpData.Stride) * output.Height;
        byte[] rgbValues = new byte[bytes];
        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

        for (int i = 0; i < bytes; i += 4) 
        {
            rgbValues[i] = colors[i / 4].B;
            rgbValues[i + 1] = colors[i / 4].G;
            rgbValues[i + 2] = colors[i / 4].R;
            rgbValues[i + 3] = 255;
        }

        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
        output.UnlockBits(bmpData);
    }

    private void SequentalFillColors()
    {
        for (int i = 0; i < outputWidth; i++)
        {
            for (int j = 0; j < outputHeight; j++)
            {
                SetPointColor(i, j);
            }
        }
    }

    private void ParallelFillColors()
    {
        Parallel.For(0, outputWidth, (i) =>
        {
            for (int j = 0; j < outputHeight; j++)
            {
                SetPointColor(i, j);
            }
        });
    }

    private void SetPointColor(int xPixel, int yPixel)
    {
        Complex c = PixelToComplex(xPixel, yPixel);
        Complex z = Complex.Zero;
        int j = 1;

        for(; j < MaxIter; j++)
        {
            z = z * z + c;
            if (z.Magnitude >= 4.0)
            {
                break;
            }
        }

        Color color = Color.Black;
        
        if(j < MaxIter)
        {
            color = ColorFromHSV(
                (Math.Pow((j / (double)MaxIter) * 360, 1.5) % 360),
                1,
                1);
        }
        colors[yPixel * outputWidth + xPixel] = color;
    }

    private Color ColorFromHSV(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }

    private Complex PixelToComplex(int x, int y)
    {
        double xVec = (x / (double) outputWidth) - 0.5;
        double yVect = 0.5 - (y / (double) outputHeight);

        double r = centre.Real + xVec * (width / 2.0);
        double i = centre.Imaginary + yVect * (width / 2.0);
        return new Complex(r, i);
    }
}


