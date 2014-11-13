using UnityEngine;
using System.Collections;

public class CameraFollow1 : MonoBehaviour
{
	private GameObject cameraTarget;
	
	public float smoothTime = 0.1f;
	public bool cameraFollowX = true;
	public bool cameraFollowY = true;
	
	private Vector2 velocity;
	private Vector3 oldPos;
    private Vector3 newPos;
	private float newXPos;
	private float newYPos;
	private Vector3 targetPos;
	
	public float maxXpos;
	public float minXpos;
	public float maxYpos;
	public float minYpos;

    private float camSizeInUnity;

    void Start()
    {
        cameraTarget = GameObject.Find("player");
        float a = 7.2f;
        float aSquare = a * a;
        float b = (7.2f* (16f / 9f));
        float bSquare = b * b;
        camSizeInUnity = Mathf.Sqrt(aSquare + bSquare);
    }
	
	void LateUpdate()
	{
        oldPos = transform.position;
        if (BuildTypeData.buildType == BuildType.PC)
        {
            //get input and mouse position
            Vector3 mouseScreenPos = Input.mousePosition;
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            if (mouseScreenPos.x < 0)
            {
                mouseScreenPos.x = 0;
            }
            else if (mouseScreenPos.x > screenWidth)
            {
                mouseScreenPos.x = screenWidth;
            }
            if (mouseScreenPos.y < 0)
            {
                mouseScreenPos.y = 0;
            }
            else if (mouseScreenPos.y > screenHeight)
            {
                mouseScreenPos.y = screenHeight;
            }
            Ray mouseRay = Camera.main.ScreenPointToRay(mouseScreenPos);
            Vector3 mousePosition = new Vector3(mouseRay.origin.x, mouseRay.origin.y, 0);
            Vector2 mousePos2D = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
            Vector3 distMouseTarget = mousePosition - cameraTarget.transform.position;
            targetPos = (cameraTarget.transform.position + (distMouseTarget / 3));
        }
        else if (BuildTypeData.buildType == BuildType.VITA)
        {
            if ((Input.GetAxisRaw("RHorizontal") > 0.1f || Input.GetAxis("RHorizontal") < -0.1f) ||
                Input.GetAxisRaw("RVertical") > 0.1f || Input.GetAxis("RVertical") < -0.1f)
            {
                Vector3 inputRightStick = new Vector3(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"), 0);
                inputRightStick.Scale(new Vector3(( camSizeInUnity / 5f), -((camSizeInUnity / 6f)*0.5625f),0));
                targetPos = (cameraTarget.transform.position + inputRightStick);
            }else{
                targetPos = cameraTarget.transform.position;
            }
        }


		if (cameraFollowX)
		{
			newXPos = Mathf.SmoothDamp(oldPos.x, targetPos.x, ref velocity.x, smoothTime);
		}
		if (cameraFollowY)
		{
			newYPos = Mathf.SmoothDamp(oldPos.y, targetPos.y, ref velocity.y, smoothTime);
		}
		if(newXPos>maxXpos){
			newXPos = maxXpos;
		}else if(newXPos<minXpos){
			newXPos = minXpos;
		}
		if(newYPos>maxYpos){
			newYPos = maxYpos;
		}else if(newYPos<minYpos){
			newYPos = minYpos;
		}
		//Update camera position
		newPos = new Vector3(newXPos,newYPos,oldPos.z);
        transform.position = newPos;
	}
}
