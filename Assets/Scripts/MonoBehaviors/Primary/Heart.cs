using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{

    public int ID;
    public SpriteRenderer sr { get; private set; }

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        transform.parent.gameObject.GetComponent<Health>().HPUpdated += Health_Changed;
    }

    public void Health_Changed(int new_HP)
    {
        if (ID >= new_HP)
        {
            Color new_color = Utilities.Visual.ChangeOpacity(sr.color, 0f);
            sr.color = new_color;
        }
    }
}
