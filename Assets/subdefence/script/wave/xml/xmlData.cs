using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("data")]
public class xmlData {
	
	[XmlArray("levels")]
	[XmlArrayItem("level")]
	public List<xmlLevel> levels = new List<xmlLevel>();
}
