public class EventManager {
	public delegate void Select(int[] selectedIds);
	public static event Select OnSelect;
	
	public static void CallOnSelect(int[] selectedID){
        if (OnSelect != null){
            OnSelect(selectedID);
        }
	}

	public delegate void GuiInput(string message);
	public static event GuiInput OnGuiInput;

	public static void callOnGuiInput(string message){
		if(OnGuiInput != null){
			OnGuiInput (message);
		}
	}
}
