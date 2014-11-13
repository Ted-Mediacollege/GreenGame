using UnityEngine;

public class EnergyManager
{
    private static int privateEnergyUsage = 0;
    private static int privateEnergyProduction = 0;
    private static int privateEnergyLevel = 0;
    private static bool pENERGY = true;

    public static bool ENERGY{
        get
        {
            return pENERGY;
        }
    }

    public static void calculateEnegy()
    {
        privateEnergyUsage = 0;
        privateEnergyProduction = 0;
        privateEnergyLevel = 0;

        Building[] buildings = LevelData.GetAllBuildings();
        int buildingLength = buildings.Length;
        for (int i = 0; i < buildingLength; i++) {
            privateEnergyUsage += buildings[i].getEnergyUsage();
            privateEnergyProduction += buildings[i].getEnergyProduction();
            privateEnergyLevel = privateEnergyUsage+privateEnergyProduction;
        }

        if (privateEnergyLevel > -1 && !ENERGY){
            pENERGY = true;
            onEnergyStateChange();
        }
        else if (privateEnergyLevel < 0 && ENERGY) {
            pENERGY = false;
            onEnergyStateChange();
        }
        Debug.Log("[EnergyManager]: on: " + ENERGY);
        Debug.Log("[EnergyManager]: level: " + privateEnergyLevel);
    }

    private static void onEnergyStateChange() {
        Debug.Log("[ change]: on: " + ENERGY);
        Debug.Log("[ change]: level: " + energyLevel);
        Building[] buildings = LevelData.GetAllBuildings();
        int buildingLength = buildings.Length;
        Debug.Log("[ change]: buildingLength: " + buildingLength);
        for (int i = buildingLength - 1; i > -1; i--)
        {
            buildings[i].onEnergyStateChange(ENERGY);
        }
    }

    public static int energyUsage {
        get {
            return privateEnergyUsage;
        }
    }

    public static int energyProducion {
        get {
            return privateEnergyProduction;
        }
    }

    public static int energyLevel {
        get {
            return privateEnergyLevel;
        }
    }
}