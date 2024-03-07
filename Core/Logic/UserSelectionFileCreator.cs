using System.Text;
using System.Xml;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic
{
    public class UserSelectionFileCreator
    {
        private readonly IFileManagementService fileManagementService;

        public UserSelectionFileCreator(IFileManagementService fileManagementService)
        {
            ParameterChecker.IsNotNull(fileManagementService, nameof(fileManagementService));

            this.fileManagementService = fileManagementService;
        }

        public void CreateFile(UserSelection data, string path)
        {
            var xmlDoc = GenerateXML(data);
            var prettyString = GetPrettyXmlString(xmlDoc);
            this.fileManagementService.WriteAllText(path, prettyString);
        }

        private static string GetPrettyXmlString(XmlDocument xmlDoc)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace,
                    Encoding = Encoding.UTF8
                };

                using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
                {
                    xmlDoc.Save(writer);
                }

                var result = Encoding.UTF8.GetString(memoryStream.ToArray());
                return $"{result[1..]}";
            }
        }

        private static XmlDocument GenerateXML(UserSelection data)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(declaration);

            XmlElement root = doc.CreateElement("userselectionlist");
            root.SetAttribute("Version", "1.0");
            doc.AppendChild(root);

            root.AppendChild(CreateElementWithAttribute(doc, "Family", "Name", data.Family));
            root.AppendChild(CreateElementWithAttribute(doc, "Model", "Name", data.Model));
            root.AppendChild(CreateElementWithAttribute(doc, "information", "name", data.Title, "comment", data.Comment));

            XmlElement favourites = doc.CreateElement("favourites");
            root.AppendChild(favourites);

            foreach (var index in data.Indexes)
            {
                XmlElement parameter = doc.CreateElement("parameter");
                parameter.SetAttribute("index", index);
                favourites.AppendChild(parameter);
            }

            return doc;
        }

        private static XmlElement CreateElementWithAttribute(XmlDocument doc, string elementName, string attributeName, string attributeValue)
        {
            XmlElement element = doc.CreateElement(elementName);
            element.SetAttribute(attributeName, attributeValue);
            return element;
        }

        private static XmlElement CreateElementWithAttribute(XmlDocument doc, string elementName, string attributeName1, string attributeValue1, string attributeName2, string attributeValue2)
        {
            XmlElement element = doc.CreateElement(elementName);
            element.SetAttribute(attributeName1, attributeValue1);
            element.SetAttribute(attributeName2, attributeValue2);
            return element;
        }
    }
}
