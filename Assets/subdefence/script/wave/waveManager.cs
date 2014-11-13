using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class waveManager : MonoBehaviour {

	public int levelID = 0;
	
	private xmlData xmldata;
	public List<waveData> wavedata;

	public bool spawing;
	public bool waiting;
	public bool paused;
	public bool deadwait;
	public bool done;
	public int waveNumber = 0;
	public int waveMax = 0;

	private EnemyManager enemyMngr;
	private GameManager gameMngr;
	private minimap map;

	[SerializeField]
	private TextAsset xmlFile;

	[SerializeField]
	private TextMesh waveText;
	public GameObject waveTextHolder;

	[SerializeField]
	private TextMesh waveAnounce;
	public GameObject waveAnounceHolder;
	private float anounceTimer;
	
	[SerializeField]
	private TextMesh waveDelayText;
	public GameObject waveDelayTextHolder;

	void Start () {
		gameMngr = gameObject.GetComponent<GameManager>() as GameManager;
		enemyMngr = GameObject.Find("gameManager").GetComponent<EnemyManager>() as EnemyManager;
		map = GameObject.Find("gameManager").GetComponent<minimap>() as minimap;

		waveTextHolder.SetActive(false);
		waveAnounceHolder.SetActive(false);
		waveDelayTextHolder.SetActive(false);

		//LOAD WAVE DATA
		xmldata = loadXML();
		//LOAD DATA FOR LEVEL
		setupLevel();
	}

	xmlData loadXML() {
		XmlSerializer serializer = new XmlSerializer(typeof(xmlData));
		StringReader xmlstring = new StringReader(xmlFile.text);
		XmlTextReader xmlReader = new XmlTextReader(xmlstring);
		return serializer.Deserialize(xmlReader) as xmlData;
	}
		
	void setupLevel () {
		List<xmlWave> waves = xmldata.levels[findLevel(levelID)].waves;
		int wavesLength = waves.Count;

		wavedata = new List<waveData>();
		for(int i = 0; i < wavesLength; i++) {
			int enemiesLength = waves[i].enemies.Count;
			waveData wd = new waveData();

			for(int j = 0; j < enemiesLength; j++) {
				waveDataQueue wdq = new waveDataQueue();
				wdq.amount = waves[i].enemies[j].amount;
				wdq.name = waves[i].enemies[j].name;
				wdq.spawnpoint = waves[i].enemies[j].spawnpoint;
				wd.enemies.Add(wdq);
				
				if(wd.spawninglist.Count < 1) {
					wd.spawninglist.Add(wdq.spawnpoint);
				} else {
					for(int l = 0; l < wd.spawninglist.Count; l++) {
						if(wd.spawninglist[l] == wdq.spawnpoint) {
							break;
						}

						if(l == wd.spawninglist.Count - 1) {
							wd.spawninglist.Add(wdq.spawnpoint);
						}
					}
				}

				wd.total += wdq.amount;
			}

			wd.left = wd.total;
			wd.floatleft = wd.total;
			wd.id = waves[i].id;
			wd.time = waves[i].time;
			wd.timeTotal = waves[i].time;
			wd.delay = waves[i].delay;
			wd.dead = waves[i].dead;
			wd.spawn = wd.total / wd.timeTotal;

			wavedata.Add(wd);
		}

		spawing = false;
		waiting = false;
		paused = true;

		waveMax = wavedata.Count;
		
		map.setBlinks(wavedata[0].spawninglist);

		nextText(true);
	}

	int findLevel(int id) {
		int levelLength = xmldata.levels.Count;
		for(int i = 0; i < levelLength; i++) {
			if(xmldata.levels[i].id == id) {
				return i;
			}
		}
		return 0;
	}

	void displayWaveAnounce() {
		if(waveNumber == waveMax) {
			waveAnounce.text = "FINAL WAVE";
		} else {
			waveAnounce.text = "WAVE " + waveNumber;
		}
		waveAnounceHolder.SetActive(true);
		anounceTimer = 3F;
	}

	void FixedUpdate() {
		if(anounceTimer > 0F) {
			anounceTimer -= Time.deltaTime;

			if(anounceTimer <= 0F) {
				waveAnounceHolder.SetActive(false);
			}
		}

		if(!paused) {
			if(spawing) {
				if(wavedata[0].time < 0F) {
					if(wavedata.Count > 1) {
						if(wavedata[0].dead) {
							//DEADWAIT
							setStates(false, false, false, true);
						} else if(wavedata[0].delay < 0F) {
							//PAUSED
							map.setBlinks(wavedata[1].spawninglist);
							setStates(true, false, false, false);
							nextText(false);
						} else {
							//DELAY WAIT
							setStates(false, false, true, false);
							waveDelayTextHolder.SetActive(true);
						}
					} else if(enemyMngr.enemies.Count < 1) {
						//DONE 
						gameMngr.GameWon();
						waveTextHolder.SetActive(false);
						done = true;
					}
				} else {
					int oldtime = (int) Mathf.Floor(wavedata[0].time);
					wavedata[0].time -= Time.deltaTime;

					int oldleft = (int) Mathf.Floor(wavedata[0].floatleft);
					wavedata[0].floatleft -= wavedata[0].spawn * Time.deltaTime;

					if(wavedata[0].left > 0) {
						if(oldleft != (int) Mathf.Floor(wavedata[0].floatleft)) {	
							int newleft = (int) Mathf.Floor(wavedata[0].floatleft);
							for(int s = oldleft - newleft; s > 0; s--) {
								if(wavedata[0].left > 0) {
									wavedata[0].left--;
									spawnNextEnemy();
								}
							}
						}
					}
				}
			} else if(waiting) {
				if(wavedata[0].delay < 0F) {
					wavedata.RemoveAt(0);
					nextWave();
				} else {
					wavedata[0].delay -= Time.deltaTime;
					waveDelayText.text = "Next wave in " + ((int) Mathf.FloorToInt(wavedata[0].delay + 1F)) + " seconds";
				}
			} else if(deadwait) {
				if(enemyMngr.enemies.Count < 1) {
					if(wavedata[0].delay < 0F) {
						//PAUSED
						map.setBlinks(wavedata[1].spawninglist);
						setStates(true, false, false, false);
						nextText(false);
					} else {
						//DELAY
						map.setBlinks(wavedata[1].spawninglist);
						setStates(false, false, true, false);
						waveDelayTextHolder.SetActive(true);
					}
				}
			}
		} else {
			if(Input.GetKey(KeyCode.G)||Input.GetKey(KeyCode.JoystickButton0)) {
				if(waveNumber != 0) { wavedata.RemoveAt(0); }
				nextWave();
			}
		}
	}

	void setStates(bool pau, bool spa, bool wai, bool dea) {
		paused = pau;
		spawing = spa;
		waiting = wai;
		deadwait = dea;
	}

	void nextWave() {
		waveNumber++;
		waveTextHolder.SetActive(false);
		waveDelayTextHolder.SetActive(false);
		displayWaveAnounce();
		setStates(false, true, false, false);
	}

	void nextText(bool first) {
		waveTextHolder.SetActive(true);
		#if UNITY_WEBPLAYER || UNITY_EDITOR
		if(first) {
			waveText.text = "Press G to start wave";
		} else {
			waveText.text = "Press G for next wave";
		}
		#elif UNITY_PSM
		if(first) {
			waveText.text = "Press X to start wave";
		} else {
			waveText.text = "Press X for next wave";
		}
		#endif
	}

	void spawnNextEnemy() {
		if(wavedata[0].enemies.Count > 0) {
			int r = Random.Range(0, wavedata[0].enemies.Count);

			if(wavedata[0].enemies[r].amount > 0) {
				wavedata[0].enemies[r].amount--;
				enemyMngr.spawnEnemy(wavedata[0].enemies[r].name, wavedata[0].enemies[r].spawnpoint);
			} else {
				wavedata[0].enemies.RemoveAt(r);
				spawnNextEnemy();
			}
		}
	}
}
