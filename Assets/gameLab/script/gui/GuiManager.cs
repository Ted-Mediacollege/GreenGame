using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GuiManager
{
	[HideInInspector]
	public int currentActive = 0;
	
	[HideInInspector]
	public float scale = 1;
	
	private float currentScale = 1;

	[SerializeField]
	public GuiData[] guiData;
	
	[HideInInspector]
	[SerializeField]
	private List<GameObject> buttonGameObjects;
	
	[HideInInspector]
	[SerializeField]
	private float currentAspectRatio;
	
	[SerializeField]
	private int guiint;
	[SerializeField]
	private Camera guiCam;
	
	float screenHeight;
	float screenWidth;

    [HideInInspector]
    [SerializeField]
    private GuiAnchors guiAnchors;
	
	public void init(){
        if (guiAnchors == null)
        {
            guiAnchors = new GuiAnchors();
        }
        guiAnchors.init(guiCam);
        if (guiData != null)
        {
            guiAnchors.updateAnchors();
            BuildGui();
        }
	}

	public void BuildGui(){
        if (guiData != null)
        {
            guiint++;
            ClearGui();

            buttonGameObjects = new List<GameObject>();
            //Debug.Log ("[GUI] "+guiData.Length);
            //Debug.Log ("[GUI]buttons: "+guiData[currentActive].buttons.Length);

            for (int i = 0; i < guiData[currentActive].buttons.Length; i++)
            {
                string name = "guibutton " + i.ToString() + " " + guiint;
                GameObject button = (GameObject)GameObject.Instantiate(guiData[currentActive].buttons[i].gameObject, Vector3.zero, Quaternion.identity);
                //GameObject button = new GameObject(name);
                button.layer = LayerMask.NameToLayer("Gui");
                button.transform.parent = guiAnchors.GetAnchors( guiData[currentActive].buttons[i].parent).transform;
                button.transform.position = new Vector3(guiData[currentActive].buttons[i].x / 100.0f
                                                        , guiData[currentActive].buttons[i].y / 100.0f, 0);
                //button.AddComponent<SpriteRenderer>().sprite = guiData[currentActive].buttons[i].sprite;
                button.tag = "Button";

                buttonGameObjects.Add(button);
            }

            for (int i = 0; i < buttonGameObjects.Count; i++)
            {
                //Debug.Log("[GUI]::::::::"+buttonGameObjects[i].GetComponent<SpriteRenderer>().sprite.bounds);
            }
        }
	}
	
	private void ClearGui(){
		GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
		//for(int i = 0; i < buttonGameObjects.Count;i++){
		//	GameObject.DestroyImmediate(buttonGameObjects[i]);
		//	//Debug.Log("Destroy: "+i);
		//}
		for(int i = 0; i < buttons.Length;i++){
			GameObject.DestroyImmediate(buttons[i]);
			//Debug.Log("Destroy: "+i);
		}
	}
	
	public bool checkGuiInput(){
		bool overGui = false;
		Vector2 mousePos = IsoMath.getMouseWorldPosition();
		//Debug.Log (mousePos);
		for(int i = 0; i < buttonGameObjects.Count;i++){
			if(guiData[currentActive].buttons[i].isButton){
				Bounds buttonBuonds = buttonGameObjects[i].renderer.bounds;
				//Debug.Log("buttonBuonds: "+buttonBuonds);
				//Debug.Log("message: "+guiData[currentActive].buttons[i].sprite.textureRect);
				if((buttonBuonds.center.x+buttonBuonds.extents.x)>mousePos.x&&
				   (buttonBuonds.center.x-buttonBuonds.extents.x)<mousePos.x&&
				   (buttonBuonds.center.y+buttonBuonds.extents.y)>mousePos.y&&
				   (buttonBuonds.center.y-buttonBuonds.extents.y)<mousePos.y){
					overGui = true;
					if(Input.GetMouseButtonDown(0)){
						EventManager.callOnGuiInput(guiData[currentActive].buttons[i].message);
					}
				}else{
					
				}
			}
		}
		return overGui;
	}

	public void tick(){
        if (guiData != null)
        {
            if (currentAspectRatio != guiCam.aspect || currentScale != scale)
            {
                currentAspectRatio = guiCam.aspect;
                currentScale = scale;
                guiAnchors.updateAnchors();
            }
            //Debug.Log("currentScale"+currentScale);
            //Debug.Log("scale"+scale);
            //updateTransforms();
            for (int i = 0; i < buttonGameObjects.Count; i++)
            {
                buttonGameObjects[i].transform.parent = guiAnchors.GetAnchors(guiData[currentActive].buttons[i].parent).transform;
                buttonGameObjects[i].transform.localPosition = new Vector3((guiData[currentActive].buttons[i].x / 100.0f) * scale
                                                                      , (guiData[currentActive].buttons[i].y / 100.0f) * scale, 0);
                buttonGameObjects[i].transform.localScale = new Vector3(scale, scale, scale);
            }

            //Debug.Log("[GUI] tick "+buttonGameObjects.Count);
        }
	}
}

