using UnityEngine;
using System.Collections;

public class UnitEnemy : Unit {

    void Start()
    {
        pathFollower.Init(this);
        pathFollower.SetPath(PathFind.FindPath(
                new VecInt(this.pos.x, this.pos.y)
                , new VecInt(25, 25)
                , LevelData.CollsionData
                , true)
             );
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
