using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam_Spawn : Beam
{

    public int Damage;

    private void Start()
    {
        Health health = GameObject.Find("Health Display").GetComponent<Health>();
        LayerMask mask = LayerMask.GetMask("Player");
        RaycastHit2D hit = DetectObjects(mask);
        if(hit.collider != null)
        {
            health.ChangeHP(-Damage);
        }
    }
}
