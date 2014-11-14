using UnityEngine;

public enum GuiAnchorID
{
    TopRight,
    TopCenter,
    TopLeft,
    MiddleRight,
    Center,
    MiddleLeft,
    BottomRight,
    BottomCenter,
    BottomLeft
}

[System.Serializable]
public class GuiAnchors
{
    [SerializeField]
    //[HideInInspector]
    private GuiAnchorMain[] anchors;

    private Camera centerCam;

    public void init(Camera _centerCam)
    {
        centerCam = _centerCam;
        if (anchors == null || anchors.Length < 1)
        {
            Debug.Log("new a");
            anchors = new GuiAnchorMain[9];
        }
    }

    public void updateAnchors()
    {
        float screenHeight = 2f * centerCam.orthographicSize;
        float screenWidth = screenHeight * centerCam.aspect;
        anchors[0] = SetAnchor(anchors[0], new Vector3(screenWidth / 2, screenHeight / 2, 10), GuiAnchorID.TopRight);
        anchors[1] = SetAnchor(anchors[1], new Vector3(0, screenHeight / 2, 10), GuiAnchorID.TopCenter);
        anchors[2] = SetAnchor(anchors[2], new Vector3(-screenWidth / 2, screenHeight / 2, 10), GuiAnchorID.TopLeft);
        anchors[3] = SetAnchor(anchors[3], new Vector3(screenWidth / 2, 0, 10), GuiAnchorID.MiddleRight);
        anchors[4] = SetAnchor(anchors[4], new Vector3(0, 0, 10), GuiAnchorID.Center);
        anchors[5] = SetAnchor(anchors[5], new Vector3(-screenWidth / 2, 0, 10), GuiAnchorID.MiddleLeft);
        anchors[6] = SetAnchor(anchors[6], new Vector3(screenWidth / 2, -screenHeight / 2, 10), GuiAnchorID.BottomRight);
        anchors[7] = SetAnchor(anchors[7], new Vector3(0, -screenHeight / 2, 10), GuiAnchorID.BottomCenter);
        anchors[8] = SetAnchor(anchors[8], new Vector3(-screenWidth / 2, -screenHeight / 2, 10), GuiAnchorID.BottomLeft);

        //TopR.localScale = new Vector3( scale,scale,scale);
        //BottomL.localScale = new Vector3( scale,scale,scale);
        //Center.localScale = new Vector3( scale,scale,scale);
    }

    private GuiAnchorMain SetAnchor(GuiAnchorMain anchor, Vector3 newPos, GuiAnchorID id)
    {
        if (anchor == null || anchor.transform == null)
        {
            //Debug.Log("new as");
            GameObject newGameobject = new GameObject(id.ToString());
            newGameobject.transform.parent = centerCam.transform;
            return new GuiAnchorMain(newGameobject.transform, id);
		}else{
            anchor.transform.localPosition = newPos;
            anchor.transform.parent = centerCam.transform;
            return anchor;
		}
	}

    public GuiAnchorMain GetAnchors(GuiAnchorID id)
    {
        for (int i = 0; i < anchors.Length; i++)
        {
            if (anchors[i].id == id)
            {
                return anchors[i];
            }
        }
        return new GuiAnchorMain( centerCam.transform,GuiAnchorID.Center);
    }
}

[System.Serializable]
public class GuiAnchorMain{
    public GuiAnchorMain(Transform _transform, GuiAnchorID _id)
    {
        transform = _transform;
        id = _id;
    }
    public Transform transform;
    public GuiAnchorID id;
}
