using System.Xml;

namespace SysmacStudioParameterEditorUserSelectionFileMaker.Core.Logic
{
    internal class UserSelectionSchemaValidator
    {
        public bool IsSchemaValid(XmlDocument xmlDoc)
        {
            try
            {
                CheckRequiredElement(xmlDoc, "userselectionlist");
                CheckRequiredAttribute(xmlDoc, "userselectionlist", "Version", false);
                CheckRequiredElement(xmlDoc, "Family");
                CheckRequiredAttribute(xmlDoc, "Family", "Name", false);
                CheckRequiredElement(xmlDoc, "Model");
                CheckRequiredAttribute(xmlDoc, "Model", "Name", true);
                CheckRequiredElement(xmlDoc, "information");
                CheckRequiredAttribute(xmlDoc, "information", "name", false);
                CheckRequiredAttribute(xmlDoc, "information", "comment", true);
                CheckRequiredElement(xmlDoc, "favourites");
                CheckParameterCount(xmlDoc, "parameter");
                CheckParameterIndexes(xmlDoc, "parameter");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static void CheckRequiredElement(XmlDocument xmlDoc, string elementName)
        {
            XmlNodeList elements = xmlDoc.GetElementsByTagName(elementName);
            if (elements.Count == 0)
            {
                throw new Exception($"Required element '{elementName}' is missing.");
            }
        }

        private static void CheckRequiredAttribute(XmlDocument xmlDoc, string elementName, string attributeName, bool allowEmpty)
        {
            XmlNodeList elements = xmlDoc.GetElementsByTagName(elementName);
            if (elements.Count > 0)
            {
                XmlAttribute attribute = elements[0].Attributes[attributeName];
                if (attribute == null)
                {
                    throw new Exception($"Required attribute '{attributeName}' of element '{elementName}' is missing.");
                }
                else if (!allowEmpty && string.IsNullOrEmpty(attribute.Value))
                {
                    throw new Exception($"Required attribute '{attributeName}' of element '{elementName}' is empty.");
                }
            }
            else
            {
                throw new Exception($"Element '{elementName}' not found to check required attribute '{attributeName}'.");
            }
        }

        private static void CheckParameterCount(XmlDocument xmlDoc, string parameterName)
        {
            XmlNodeList parameters = xmlDoc.GetElementsByTagName(parameterName);
            if (parameters.Count == 0)
            {
                throw new Exception($"At least one '{parameterName}' element is required.");
            }
        }

        private static void CheckParameterIndexes(XmlDocument xmlDoc, string parameterName)
        {
            XmlNodeList parameters = xmlDoc.GetElementsByTagName(parameterName);
            foreach (XmlNode parameter in parameters)
            {
                XmlAttribute indexAttribute = parameter.Attributes["index"];
                if (indexAttribute == null || string.IsNullOrEmpty(indexAttribute.Value))
                {
                    throw new Exception($"Each '{parameterName}' element must have its own 'index' attribute.");
                }
            }
        }
    }
}
