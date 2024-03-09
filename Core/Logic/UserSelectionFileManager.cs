using System.Text;
using System.Xml;

using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Common.Validation;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.DTOs;
using SysmacStudioParameterEditorUserSelectionFileMaker.Core.Services;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic
{
    public class UserSelectionFileManager
    {
        private readonly IFileManagementService fileManagementService;
        private readonly UserSelectionSchemaValidator userSelectionSchemaValidator;

        public UserSelectionFileManager(IFileManagementService fileManagementService)
        {
            ParameterChecker.IsNotNull(fileManagementService, nameof(fileManagementService));

            this.fileManagementService = fileManagementService;
            this.userSelectionSchemaValidator = new UserSelectionSchemaValidator();
        }

        public void CreateFile(UserSelection data, string filePath)
        {
            ParameterChecker.IsNotNull(data, nameof(data));
            ParameterChecker.IsNotNullOrEmpty(filePath, nameof(filePath));

            var xmlDoc = GenerateXML(data);
            var prettyString = GetPrettyXmlString(xmlDoc);
            fileManagementService.WriteAllText(filePath, prettyString);
        }

        public UserSelection LoadFile(string filePath)
        {
            ParameterChecker.IsNotNullOrEmpty(filePath, nameof(filePath));

            if (fileManagementService.FileExists(filePath))
            {
                return ReadXML(filePath);
            }
            else
            {
                return null;
            }
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

        private UserSelection ReadXML(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(fileManagementService.ReadAllText(filePath));
            }
            catch
            {
                return null;
            }

            var schemaIsValid = userSelectionSchemaValidator.IsSchemaValid(xmlDoc);

            if (schemaIsValid)
            {
                var data = new UserSelection();

                try
                {
                    XmlNode root = xmlDoc.DocumentElement;
                    data.Family = root.SelectSingleNode("Family").Attributes["Name"].Value;
                    data.Model = root.SelectSingleNode("Model").Attributes["Name"].Value;
                    data.Title = root.SelectSingleNode("information").Attributes["name"].Value;
                    data.Comment = root.SelectSingleNode("information").Attributes["comment"].Value;
                    data.Indexes = new List<string>();

                    XmlNodeList parameterNodes = root.SelectNodes("favourites/parameter");

                    foreach (XmlNode parameterNode in parameterNodes)
                    {
                        data.Indexes.Add(parameterNode.Attributes["index"].Value);
                    }
                }
                catch
                {
                    return null;
                }

                return data;
            }

            return null;
        }
    }
}
