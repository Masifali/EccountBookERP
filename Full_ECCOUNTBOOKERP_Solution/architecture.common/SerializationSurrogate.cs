using System;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable]
public class SerializationSurrogate : IXmlSerializable
{
	public object Instance { get; set; }

	public SerializationSurrogate()
	{
	}

	public SerializationSurrogate(object instance)
	{
		Instance = instance;
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		throw new NotSupportedException("Deserialization is not supported.");
	}

	public void WriteXml(XmlWriter writer)
	{
		PropertyInfo[] properties = Instance.GetType().GetProperties();
		foreach (PropertyInfo property in properties)
		{
			if (!property.GetGetMethod().IsVirtual)
			{
				object value = property.GetValue(Instance);
				if (value != null)
				{
					writer.WriteStartElement(property.Name);
					writer.WriteValue(value.ToString());
					writer.WriteEndElement();
				}
			}
		}
	}
}
