using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Simplic.Utilities.TemplateParser.Test
{
    [TestClass]
    public class DefaultParser
    {        
        ITemplateParser defaultParser;

        public DefaultParser()
        {
            defaultParser = new TemplateParser.DefaultParser();
        }

        [TestMethod]
        public void ParseTemplate_ObjectArrayTest()
        {         
            var rawString = "{Name} from {Address.City}, {Address.State}";
            var addrList = new[] { new { Name = "Jim", Address = new { City = "New York", State = "NY" } } };

            var expectedResult = "Jim from New York, NY";
            var actualResult = defaultParser.ParseTemplate(rawString, addrList);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void ParseTemplate_DictionaryTest()
        {
            var rawString = "{Name} from {City}, {State}";

            Dictionary<string, string> dicToTest = new Dictionary<string, string>{
               { "Name" , "Jim" },
               { "City" , "New York" },
               { "State" , "NY" }
           };

            var expectedResult = "Jim from New York, NY";
            var actualResult = defaultParser.ParseTemplate(rawString, dicToTest);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void ParseTemplate_StripOldSyntaxTest()
        {
            var rawString = "${Name} from ${City}, ${State}";

            Dictionary<string, string> dicToTest = new Dictionary<string, string>{
               { "Name" , "Jim" },
               { "City" , "New York" },
               { "State" , "NY" }
           };

            var expectedResult = "Jim from New York, NY";
            var actualResult = defaultParser.ParseTemplate(rawString, dicToTest);

            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void Empty_Input_Test()
        {
            var rawString = "";

            Dictionary<string, string> dicToTest = new Dictionary<string, string>{
               { "Name" , "Jim" },
               { "City" , "New York" },
               { "State" , "NY" }
           };

            var expectedResult = "";
            var actualResult = defaultParser.ParseTemplate(rawString, dicToTest);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Null_Values_Test()
        {
            var rawString = "Jim from New York, NY";

            object nullObj = null;

            var expectedResult = "Jim from New York, NY";
            var actualResult = defaultParser.ParseTemplate(rawString, new[] { nullObj });

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Null_Input_Test()
        {            
            object nullObj = null;

            var expectedResult = string.Empty;
            var actualResult = defaultParser.ParseTemplate(null, new[] { nullObj });

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
