// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using ParPr_Lb7;

BenchmarkRunner.Run<MandelbrotBenchmarks>();
Console.ReadLine();