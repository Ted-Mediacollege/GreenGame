using UnityEngine;
using System.Collections;


public class GuiData:MonoBehaviour
{
	[SerializeField]
	private int id;
	[SerializeField]
	private string name;
	public GuiButton[] buttons;
		//public GuiData(){

	//}
}

[System.Serializable]
public class HitBox{
	[SerializeField]
	internal int x,y;
}

[System.Serializable]
public class GuiButton:HitBox
{
	//[SerializeField]
	//internal Sprite sprite;
	[SerializeField]
	internal bool isButton = true;
	[SerializeField]
	internal string message;
	[SerializeField]
	internal GuiAnchorID parent;
	[SerializeField]
	internal GameObject gameObject;
}

