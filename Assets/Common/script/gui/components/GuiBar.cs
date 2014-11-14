using UnityEngine;

public class GuiBar : MonoBehaviour 
{
    [SerializeField]
    private GameObject Bar;

    public void UpdateBar(float amount, float maxAmount){
        //Debug.Log((float)(amount / maxAmount));
        if (amount > 0 && maxAmount >0){
            Bar.transform.localScale = new Vector3((float)(amount/maxAmount),1.0f,1.0f);
        }else{
            Bar.transform.localScale = new Vector3(0, 1.0f, 1.0f);
        }
    }
}