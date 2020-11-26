using CommandLine;
using SharpShh.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShh
{
    public class BaseOptions
    {
        [Option('f', Required = true, HelpText = "Path to input file")]
        public string InputFile { get; set; }

        [Option('k', Required = false, HelpText = "Key to use for decryption. Encryption will always be random")]
        public string Base64Key { get; set; }

        [Option('i', Required = false, HelpText = "IV to use for decryption. Encryption will always be random")]
        public string Base64IV { get; set; }

        public byte[] Key => System.Convert.FromBase64String(Base64Key);
        public byte[] IV => System.Convert.FromBase64String(Base64IV);
    }

    [Verb("encrypt", HelpText = "Encrypt blob and save it to out file (-o)")]
    public class EncryptOptions : BaseOptions
    {
        [Option('o', Required = true, HelpText = "Path to output file")]
        public string OutputFile { get; set; }
    }

    [Verb("load", HelpText = "Decrypt and load assembly")]
    public class LoadOptions : BaseOptions
    {
    }

    [Verb("execute", HelpText = "Execute method in assembly")]
    public class ExecuteOptions
    {
        [Option('a', Required = true, HelpText = "Assembly to execute from")]
        public string Assembly { get; set; }

        [Option('m', Required = true, HelpText = "Method to execute")]
        public string Method { get; set; }

        public string Type
        {
            get
            {
                var split = Method.Split('.');

                return String.Join(".", split.Take(split.Length - 1));
            }
        }

        [Option('p', HelpText = "Comma separated list of input parameters")]
        public IEnumerable<string> Parameters { get; set; }
    }

    [Verb("exit")]
    public class ExitOptions
    {

    }
}
