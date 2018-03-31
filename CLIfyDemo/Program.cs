using System;
using CLIfy;
using System.Reflection;

namespace CLIfyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CLIApp app = new CLIApp("CLIfySample",false,CLIstyle.Unix);
            app.Register<string,string,bool>(test);
            app.RegisterFunction<int,int,CLIResult>(Add);

            app.Run(args);
        }


        static void test(string path,[CLIOptional("s")] string scheme ="default",[CLIOptional("v")] bool verify = false)
        {
            Console.WriteLine($"test runed path:{path} scheme={scheme} verify={verify}");
        }

        static CLIResult Add (int a,int b)
        {
            return CLIResult.Success(a + b);
        }
    }
}
