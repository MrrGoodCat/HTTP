using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UniTester
{
    class TesterXMLSerializer<T>
    {
        private string xmlFilePath;

        #region Constructors

        /// <summary>
        /// Default constructor that initiates TesterXMLSerializer for specified Type. 
        /// XML file must be specified later in the Serialize()\Deserialize() methods.
        /// </summary>
        public TesterXMLSerializer()
        {
            
        }

        /// <summary>
        /// Initiates TesterXMLSerializer for specified Type with the given xml file path.
        /// </summary>
        /// <param name="xmlPath">Full name of XML that will be used for serialization and deserialization.</param>
        public TesterXMLSerializer(string xmlPath)
        {
            xmlFilePath = xmlPath;
        }
        #endregion

        #region Serialization

        /// <summary>
        /// Serialize the given object to the XML. XML file has been specified during TesterXMLSerializer initiation.
        /// </summary>
        /// <param name="obj">Object that should be serialized.</param>
        /// <returns>True if success. False if XML folder doesn't exist of obj is null.</returns>
        public bool Serialize(object obj)
        {
            return Serialization(obj, this.xmlFilePath);
        }

        /// <summary>
        /// Serialize the given object to the specified XML file.
        /// </summary>
        /// <param name="obj">Object that should be serialized.</param>
        /// <param name="xmlFilePath">Full name of XML that obj is relialized in.</param>
        /// <returns>True if success. False if XML folder doesn't exist of obj is null.</returns>
        public bool Serialize(object obj, string xmlFilePath)
        {
            return Serialization(obj, xmlFilePath);
        }

        private bool Serialization(object obj, string xmlFilePath)
        {
            if (obj != null && Directory.Exists(Path.GetDirectoryName(xmlFilePath)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(xmlFilePath))
                {
                    serializer.Serialize(writer, obj);
                }
                return true;
            }
            return false;
        }
        #endregion


        #region Deserialization


        /// <summary>
        /// Deserialize XML file to the object of the given Type. XML file has been specified during TesterXMLSerializer initiation.
        /// </summary>
        /// <returns>Object of the specified Type (has specified during TesterXMLSerializer declaration).</returns>
        public T Deserialize()
        {
            return Deserialization(this.xmlFilePath);
        }

        /// <summary>
        /// Deserialize XML file to the object of the given Type.
        /// </summary>
        /// <param name="xmlFilePath">XML file that should be deserialized.</param>
        /// <returns>Object of the specified Type (has specified during TesterXMLSerializer declaration).</returns>
        public T Deserialize(string xmlFilePath)
        {
            return Deserialization(xmlFilePath);
        }

        private T Deserialization(string xmlFilePath)
        {
            object obj = null;
            if (File.Exists(xmlFilePath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StreamReader(xmlFilePath))
                {
                    obj = deserializer.Deserialize(reader);
                    return (T)obj;
                }
            }
            return (T)obj;
        }
        #endregion

    }
}
