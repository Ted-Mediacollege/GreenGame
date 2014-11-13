using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class xmlLevel {
	
	[XmlAttribute("id")]
	public int id;
	
	[XmlArray("waves")]
	[XmlArrayItem("wave")]
	public List<xmlWave> waves = new List<xmlWave>();
}
