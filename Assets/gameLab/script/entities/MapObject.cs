using UnityEngine;

public class MapObject : MonoBehaviour
{
    public VecInt pos;
    public VecInt size;

    private void Awake(){
        pos = new VecInt(0,0);
        size = new VecInt(0,0);
    }
}
