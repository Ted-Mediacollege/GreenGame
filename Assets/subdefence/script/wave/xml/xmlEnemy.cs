using System.Xml;
using System.Xml.Serialization;

public class xmlEnemy {
	
	[XmlAttribute("name")]
	public string name;
	
	[XmlAttribute("amount")]
	public int amount;
	
	[XmlAttribute("spawnpoint")]
	public string spawnpoint;
}
