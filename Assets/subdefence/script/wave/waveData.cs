using System.Collections.Generic;

public class waveData {
	public int id;
	public float time;
	public float timeTotal;
	public float delay;
	public int total = 0;
	public int left = 0;
	public float floatleft = 0;
	public float spawn = 0;
	public bool dead = false;

	public List<waveDataQueue> enemies = new List<waveDataQueue>();
	public List<string> spawninglist = new List<string>();
}
