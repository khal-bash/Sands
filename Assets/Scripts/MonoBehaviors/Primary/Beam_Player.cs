using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam_Player : Beam
{

    private void Start()
    {
        LayerMask mask = LayerMask.GetMask("Collectable");
        MineObjects(mask);
    }

}
