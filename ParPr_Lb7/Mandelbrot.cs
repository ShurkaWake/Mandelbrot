using System.Drawing;
using System.Numerics;

namespace ParPr_Lb7;

public record Mandelbrot
{
    const int MaxIter = 100;

    Complex centre;
    double width;
    Bitmap output;

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
        output = new Bitmap(outputWidth, outputHeight);
        FillOutput();
    }

    public Mandelbrot(int outputWidth, int outputHeight, bool isParallel) 
        : this(-0.5, 0, 4, outputWidth, outputHeight, isParallel)
    {
    }

    public Bitmap Result => output;

    private void FillOutput()
    {
        for (int i = 0; i < output.Width; i++)
        {
            for (int j = 0; j < output.Height; j++)
            {
                SetPointColor(i, j);
            }
        }
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

        Color color = ColorFromHSV(
            Math.Pow((j / (double)MaxIter) * 360, 1.5) % 360,
            100,
            (j / (double)MaxIter) * 100);
        output.SetPixel(xPixel, yPixel, color);
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
        double xVec = (x / (double) output.Width) - 0.5;
        double yVect = 0.5 - (y / (double) output.Height);

        double r = centre.Real + xVec * (width / 2.0);
        double i = centre.Imaginary + yVect * (width / 2.0);
        return new Complex(r, i);
    }
}


