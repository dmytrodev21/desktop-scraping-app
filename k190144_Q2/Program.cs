using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml;
using System.Configuration;

namespace k190144_Q2
{
    internal class Program
    {
        private void ParseData(string path)
        {
            var doc = new HtmlDocument();
            doc.Load(path);

            var tables = doc.DocumentNode.SelectNodes("//div[@class='table-responsive']");

            for (int i = 0; i < tables.Count; i++)
            {
                var category = tables[i].SelectSingleNode(".//th[@colspan='8']//h4");

                string directory = @"D:\Assignment1\" + category.InnerText;
                directory = Regex.Replace(directory, "/", @"");
                Directory.CreateDirectory(directory);

                var scripts = tables[i].SelectNodes(".//td[@class='dataportal']");
                var currentPrice = tables[i].SelectNodes(".//tr[@class='red-text-td' or @class='green-text-td' or @class='blue-text-td']//td[6]");
                
                string xmlFileName = directory + @"\" + DateTime.Now.ToString("dd-MMM-yyyy HH-mm-ss") + ".xml";
                CreateXmlFile(scripts, currentPrice, xmlFileName);
            }
        }

        private void CreateXmlFile(HtmlNodeCollection scripts, HtmlNodeCollection currentPrice, string xmlFileName)
        {
            XmlTextWriter xmlWriter = new(xmlFileName, System.Text.Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("xml");

            for (int j = 0; j < scripts.Count; j++)
            {
                xmlWriter.WriteStartElement("Scripts");
                xmlWriter.WriteElementString("Script", scripts[j].InnerText);
                xmlWriter.WriteElementString("Price", currentPrice[j].InnerText);
                xmlWriter.WriteEndElement();

            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var path = appSettings["path"];

            DirectoryInfo dri = new(path);
            var directory = new DirectoryInfo(dri.ToString());
            var fileName = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            Program obj = new();
            obj.ParseData(fileName.ToString());
        }
    }
}