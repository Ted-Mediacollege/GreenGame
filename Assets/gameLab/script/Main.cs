using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Main : MonoBehaviour
{
	[SerializeField]
	public GuiManager gui;
	private Game game;
	
	private static Main _instance;
	public static Main instance
	{
		get
		{
			return _instance;
		}
	}
	
	void Start ()
	{
		//if(UnityEditor.EditorApplication.isPlaying){
			game = (Game)Game.instance;
		//}
		gui.init ();
	}
	void Update ()
	{
		gui.tick();
		//Debug.Log(Application.isEditor);
		bool overGui = gui.checkGuiInput();
		if(game != null&&!overGui){
			GameInput.updateInput();
			PathFind.Update();
		}
	}

    void OnEnable()
    {
        EventManager.OnGuiInput += GuiInput;
    }


    void OnDisable()
    {
        EventManager.OnGuiInput -= GuiInput;
    }

    private void GuiInput(string message)
    {
        Debug.Log("[Main]: Event Message:" + message);
        if (message == "StartGame")
        {
            Application.LoadLevel(Levels.kit);
        }
        else if (message == "Menu")
        {
            Application.LoadLevel(Levels.MainMenu);
        }
    }
}

