using UnityEngine;
using System.Collections;

[System.Serializable]
public class RotationHandler
{
    [SerializeField]
    private Sprite Up, RightUp, Right, RightDown, Down, LeftDown, Left, LeftUp;
    [SerializeField]
    private SpriteRenderer sprite;

    private int currentRotation;
    private int oldRotationStateNumber = 10;
    private int newRotationStateNumber = 1;

    private MonoBehaviour creator;
    public void Init(MonoBehaviour _creator)
    {
        creator = _creator;
        currentRotation = 2;
        sprite.sprite = RightUp;
    }

    public void UpdateSprite(int nextRotationState)
    {
        int state = nextRotationState;
        if (currentRotation == 0) { currentRotation += 8; }
        else if (currentRotation == 9) { currentRotation -= 8; }

        if (state == 1) { sprite.sprite = Up; }
        else if (state == 2) { sprite.sprite = RightUp; }
        else if (state == 3) { sprite.sprite = Right; }
        else if (state == 4) { sprite.sprite = RightDown; }
        else if (state == 5) { sprite.sprite = Down; }
        else if (state == 6) { sprite.sprite = LeftDown; }
        else if (state == 7) { sprite.sprite = Left; }
        else if (state == 8) { sprite.sprite = LeftUp; }
    }

	public bool CheckRotation (VecInt oldPos, VecInt newPos,float delay) {
        if(oldPos.x < newPos.x){
			if(oldPos.y > newPos.y){
				newRotationStateNumber = 5;
			}else if(oldPos.y < newPos.y){
				newRotationStateNumber = 3;
			}else{
				newRotationStateNumber = 4;
			}
		}else if(oldPos.x > newPos.x){
			if(oldPos.y > newPos.y){
				newRotationStateNumber = 7;
			}else if(oldPos.y < newPos.y){
				newRotationStateNumber = 1;
			}else{
				newRotationStateNumber = 8;
			}
		}else{
			if(oldPos.y > newPos.y){
				newRotationStateNumber = 6;
			}else {
				newRotationStateNumber = 2;
			}
		}
        UpdateSprite(newRotationStateNumber);
        bool turning = false;
        if (newRotationStateNumber != oldRotationStateNumber) { turning = true; }

        creator.StartCoroutine(Rotate(delay));
        return turning;
    }

    IEnumerator Rotate(float delay)
    {
        yield return new WaitForSeconds(delay);
        oldRotationStateNumber = newRotationStateNumber;
    }
}
