using UnityEngine;
using System.Collections;

[System.Serializable]
public class PathFollower
{
    [SerializeField]
    private RotationHandler rotationHandler;

	private Vector3 pos;
	private VecInt[] currentPath;
	private int pathProgress;
    private MonoBehaviour creator;
	private float lerp;
	private float loopInt = 0;
	private VecInt oldPos;
	private bool turnUnit;
	
	//private CheckNewPosition rotater;
	
	private VecInt startPos;
	private VecInt endPos;

	internal void Init(MonoBehaviour _creator){
        creator = _creator;
        //rotater = creator.transform.gameObject.GetComponent<CheckNewPosition>();
        //rotationHandler = new RotationHandler();
        rotationHandler.Init(_creator);
        pos = creator.transform.position;
        oldWorldPos = creator.transform.position;
	}

	private Vector3 oldWorldPos;
	
	internal void loop(){
		//transform.position = Vector3.Lerp(
		//trans.Translate(0.05f,0.05f,0);
		if (currentPath != null) {
			if(pathProgress < currentPath.Length){

				
			}
			loopInt += 1;
		}
	}
	
	private bool checkNextPosFree(VecInt next,VecInt current){
		//check collision array
		if (LevelData.GetMapObjects (next.x, next.y) !=null) {
			//set new path
			//Debug.Log("newPath!!!!!!!!!!!!!!");

			SetPath(PathFind.FindPath (
				new VecInt(current.x,current.y)
				, new VecInt(oldPos.x,oldPos.y)
				, LevelData.CollsionData
                ,true)
			 );
			return false;
		} else {
			//update collision array if free
			//Debug.Log(current.print+" Cur: "+LevelData.GroundVehicles [current.x, current.y]);
			//Debug.Log(next.print+" Next: "+LevelData.GroundVehicles [next.x, next.y]);

            /*
            LevelData.mapObjects[next.x, next.y] = LevelData.mapObjects[current.x, current.y];
            LevelData.mapObjects[next.x, next.y].pos = next;
            LevelData.mapObjects[current.x, current.y] = null;
            LevelData.objectData[current.x, current.y] = 0;
            LevelData.objectData[next.x, next.y] = 1;
            */

            LevelData.GetMapObjects(current.x, current.y).pos = next;
			LevelData.objectData [current.x, current.y] = 0;
			LevelData.objectData [next.x, next.y] = 1;

			LevelData.CollsionData[current.x,current.y] = false;
			LevelData.CollsionData[next.x,next.y] = true;

			//Debug.Log(next.print+" Next: "+LevelData.GroundVehicles [next.x, next.y]);
			if(next == endPos){
				//currentPath = null;
			}
			return true;
		}

	}
	
	internal void SetPath(VecInt[] path){
		startPos = path[0];
		
        /*
        Debug.Log ("[path] "+path[0].print);
		Debug.Log ("[path] "+path[1].print);
		Debug.Log ("[path] Lenght: "+path.Length);
        */
        
		oldPos = startPos;
		endPos = path[path.Length-1];
		currentPath = path;
		pathProgress = 0;
		loopInt = 0;
        oldWorldPos = creator.transform.position;
        Animate();
	}

    private Vector2 newPos;

    private void Animate()
    {
        if (pathProgress < currentPath.Length)
        {
           newPos = IsoMath.tileToWorld(currentPath[pathProgress + 1].x, currentPath[pathProgress + 1].y);
           //yield return new WaitForSeconds(0.02f);
           StartMove();
        }
    }

    private void StartMove()
    {
        if (pathProgress < currentPath.Length - 1)
        {
            if (checkNextPosFree(currentPath[pathProgress + 1], oldPos))
            {
                turnUnit = rotationHandler.CheckRotation(oldPos, currentPath[pathProgress + 1], 0.125f);
                oldPos = currentPath[pathProgress + 1];
                if (turnUnit)
                {
                    creator.StartCoroutine(RotateDelay());
                }
                else
                {
                    creator.StartCoroutine(UpdateMove());
                }
            }
        }
    }

    IEnumerator UpdateMove()
    {
        //Debug.Log("t" + loopInt);
        yield return new WaitForSeconds(0.02f);
        
        Vector3 newWorldPos = new Vector3(newPos.x, newPos.y, newPos.x * newPos.y / 40f + 5f);
        creator.transform.position = Vector3.Lerp(oldWorldPos, newWorldPos, (loopInt / 50));
        loopInt++;
        if (loopInt >= 50){
            //Debug.Log("move" + pathProgress);
            oldWorldPos = creator.transform.position;
            pathProgress += 1;
            creator.transform.position = new Vector3(newPos.x, newPos.y, newPos.x * newPos.y / 40f + 5f);
            loopInt = 0;
            StartMove();
        }
        else if (pathProgress < currentPath.Length-1)
        {
            newPos = IsoMath.tileToWorld(currentPath[pathProgress + 1].x, currentPath[pathProgress + 1].y);
            creator.StartCoroutine(UpdateMove());
        }
    }

    IEnumerator RotateDelay()
    {
        yield return new WaitForSeconds(0.25f);
        creator.StartCoroutine(UpdateMove());
    }
    
            
            
           /* if (loopInt % 50 == 0)
            {
                Debug.Log("[path progress]: " + pathProgress);
                oldWorldPos = trans.position;
                if (pathProgress > 0)
                {
                    turnUnit = rotater.CheckNewPos(oldPos, currentPath[pathProgress]);
                }
                //trans.position = new Vector3(newPos.x,newPos.y,zpos);
                oldPos = currentPath[pathProgress];
                pathProgress += 1;
                if (!turnUnit)
                {
                    if (checkNextPosFree(currentPath[pathProgress], oldPos))
                    {
                        trans.position = new Vector3(newPos.x, newPos.y, newPos.x * newPos.y / 40f + 5f);
                    }
                }
            }*/
}

