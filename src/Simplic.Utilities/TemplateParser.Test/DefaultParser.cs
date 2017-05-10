using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TemplateParser.Test
{
    [TestClass]
    public class DefaultParser
    {        
        TemplateParser.Impl.DefaultParser defaultParser;

        public DefaultParser()
        {
            defaultParser = new TemplateParser.Impl.DefaultParser();
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
    }
}
