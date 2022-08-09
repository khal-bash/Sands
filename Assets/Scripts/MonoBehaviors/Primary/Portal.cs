using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    int frame = 0;

    public GameObject spawn;
    public int spawn_rate;

    void Update()
    {
        if (frame % spawn_rate == spawn_rate - 1)
        {
            Instantiate(spawn, transform);
        }
        frame++;
    }
}
