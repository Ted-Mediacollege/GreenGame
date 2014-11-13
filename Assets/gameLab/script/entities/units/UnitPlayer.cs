using UnityEngine;
using System.Collections;

public class UnitPlayer : Unit{
    [SerializeField]
    private Selectable selectable;

    private void OnEnable()
    {
        selectable.init(this);
        selectable.OnEnable();
    }

    private void OnDisable()
    {
        selectable.OnDisable();
    }
}
