using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class xmlWave {
	
	[XmlAttribute("id")]
	public int id;

	[XmlAttribute("time")]
	public int time;

	[XmlAttribute("delay")]
	public int delay;

	[XmlAttribute("dead")]
	public bool dead;
	
	[XmlArray("enemies")]
	[XmlArrayItem("enemy")]
	public List<xmlEnemy> enemies = new List<xmlEnemy>();
}
