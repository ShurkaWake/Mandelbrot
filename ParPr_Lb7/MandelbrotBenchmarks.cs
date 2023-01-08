using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParPr_Lb7
{
    public class MandelbrotBenchmarks
    {
        [Params(100, 1_000, 5_000)]
        public int length { get; set; }

        [Benchmark(Description = "Sequental Mandelbrot")]
        public Bitmap SequentalMandelbrot()
        {
            Mandelbrot mandelbrot = new Mandelbrot(length, length, Mode.Sequental);
            return mandelbrot.Result;
        }

        [Benchmark(Description = "Parallel thread Mandelbrot")]
        public Bitmap ParallelThreadMandelbrot()
        {
            Mandelbrot mandelbrot = new Mandelbrot(length, length, Mode.ParallelThreads);
            return mandelbrot.Result;
        }

        [Benchmark(Description = "Parallel for Mandelbrot")]
        public Bitmap ParallelForMandelbrot()
        {
            Mandelbrot mandelbrot = new Mandelbrot(length, length, Mode.ParallelFor);
            return mandelbrot.Result;
        }
    }
}
