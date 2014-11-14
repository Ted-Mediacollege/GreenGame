using UnityEngine;
public class GuiBarEnergy: GuiBar{
    void Update(){
        //Debug.Log(EnergyManager.energyLevel);
        //Debug.Log(EnergyManager.energyProducion);
        UpdateBar(EnergyManager.energyLevel, EnergyManager.energyProducion);
    }
}