using UnityEngine;
using System.Collections;

public class Unit : MapObject
{
    [SerializeField]
	internal PathFollower pathFollower;

	public void FollowPath(VecInt[] path){
        
		pathFollower.Init (this);
		pathFollower.SetPath (path);
	}

    internal void FixedUpdate ()
	{
		if (pathFollower != null) {
			pathFollower.loop ();
		}
	}


}

