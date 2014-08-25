using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace okbrain.Services
{
    public interface ITaxonomy
    {
        List<string> GenerateTags(string content);
    }

    public class AlchemyTaxonomy : ITaxonomy
    {
        public List<string> GenerateTags(string content)
        {
            // Create an AlchemyAPI object.
            var alchemyObj = new AlchemyAPI.AlchemyAPI();

            // Load an API key from disk.
            alchemyObj.SetAPIKey("2d5cf3e2b75b88de5e96c37d87915f3ec0edb483");

            string textGetRankedTaxonomy = alchemyObj.TextGetRankedKeywords(content);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(textGetRankedTaxonomy);

            XmlNodeList keyWordNodes = xmlDocument.GetElementsByTagName("text");

            return (from XmlNode keyWordNode in keyWordNodes select keyWordNode.InnerText).ToList();
        }
    }

    public class LocalTaxonomyService : ITaxonomy
    {
        public List<string> GenerateTags(string content)
        {
            return new List<string>
                       {
                           "memory",
                           "step by step",
                           "formulas"
                       };
        }
    }
}