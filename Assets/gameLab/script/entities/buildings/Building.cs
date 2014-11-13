using UnityEngine;
using System.Collections;

public class Building : MapObject {
    [SerializeField]
    private int width = 2;
    [SerializeField]
    private int height = 3;
    [SerializeField]
	private int buyAmount;
    [SerializeField]
    private int sellAmount;
    [SerializeField]
    private int energyUsage;
    [SerializeField]
    private int eneryProduction;
    [SerializeField]
    private int maxHealth;
    //[SerializeField]
    private int health;

	//public int buildingWidth;
	//public int buildingHeight;
	
	public SpriteRenderer spriterenderer;

    private void Start()
    {
        health = maxHealth;
        size.x = width;
        size.y = height;
        spriterenderer = GetComponent<SpriteRenderer>();
        LevelData.mapObjects.Add(this);
        EnergyManager.calculateEnegy();
        onEnergyStateChange(EnergyManager.ENERGY);
        transform.position = IsoMath.addSizeToPosition(transform.position, getBuildingWidth(), getBuildingHeight(), LevelData.size);	
    }

	public bool damage(int amount) {
		health -= amount;
		if(health < 0)
		{
			return true;
		}
		return false;
	}
	
	public void onEnergyStateChange(bool state) {
		if(state) { spriterenderer.color = Color.white; } else { spriterenderer.color = Color.gray; }
	}

	//======================================
	
	public int getHealth() {
		return health;
	}
	
	public int getEnergyUsage() {
		return energyUsage;
	}

    public int getEnergyProduction()
    {
        return eneryProduction;
    }
	
	public int getBuyAmount() {
		return buyAmount;
	}
	
	public int getSellAmount() {
		return sellAmount;
	}
	
	public int getBuildingWidth() {
		return size.x;
	}
	
	public int getBuildingHeight() {
		return size.y;
	}
}