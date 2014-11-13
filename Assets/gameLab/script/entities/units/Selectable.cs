using UnityEngine;
using System.Collections;

[System.Serializable]
public class Selectable {

	[SerializeField]
	private GameObject selectIcon;

    private MonoBehaviour creator;
    internal void init(MonoBehaviour _creator)
    {
        creator = _creator;
	}
	public void OnEnable()
	{
		EventManager.OnSelect += Select;
	}


    public void OnDisable()
	{
		EventManager.OnSelect -= Select;
	}
	
	private void Select(int[] id){
        //Debug.Log(creator.gameObject.GetInstanceID());
		if (id.Length > 0) {
			for (int i = 0; i <id.Length; i++) {
                if (creator.gameObject.GetInstanceID() == id[i])
                {
					selectIcon.SetActive (true);
					break;
				} else {
					selectIcon.SetActive (false);
				}
			}
		}else{
			selectIcon.SetActive (false);
		}
	}
}
