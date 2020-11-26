using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpShh;

namespace SharpShh.Test
{
    [TestClass]
    public class SharpShhTest
    {

        public string InputFile => @"C:\Users\ElPatron\Desktop\amsi-test\encrypted.dll";
        public string Key => "+bvUFby/27DL17wrVwdQkqHWKXnMP4R26Xig9EOw7us=";
        public string IV => "sLcRV9rqVf1XVvkMyEQgoQ==";

        [TestMethod]
        public void TestMethod1()
        {
            global::SharpShh.Program.Load(new LoadOptions
            {
                InputFile = InputFile,
                Base64Key = Key,
                Base64IV = IV
            });

            global::SharpShh.Program.Execute(new ExecuteOptions
            {
                Assembly = "SharpSploit",
                Method = "Enumeration.Host.GetDirectoryListing",
                Parameters = new string[]
                {
                    @"C:\"
                }
            });

            global::SharpShh.Program.Execute(new ExecuteOptions
            {
                Assembly = "SharpSploit",
                Method = "Enumeration.Host.GetDirectoryListing",
                Parameters = new string[]
                {
                    @"C:\Windows"
                }
            });

            global::SharpShh.Program.Execute(new ExecuteOptions
            {
                Assembly = "SharpSploit",
                Method = "lateralmovement.scm.getservices",
                Parameters = new string[]
                {
                    @"localhost"
                }   
            });
        }
    }
}
