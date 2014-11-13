using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager {
	public int level;
	public float time;

	public void init() {
		level = 0;
		time = 20;
	}

	public void update() {
		time -= Time.deltaTime;
		if(time < 0) {
			wave();

			level++;
			time = 20 * (1f - level / 100);
		}
	}

	public void wave() {

	}
}