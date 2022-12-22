using DTO.Visuals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class CollectableRequirmentsDisplayForeground : MonoBehaviour
{

    public float spacing = 0.2f;

    public GameObject UICollectable;

    public void SpawnForeground(Inventory requirements)
    {

        float runningEndpoint = 0.2f;

        foreach(CollectableType type in requirements.typesRequired)
        {
            //i have no clue don't look at me
            GameObject collectableIcon = (GameObject) Instantiate(UICollectable, parent: transform, instantiateInWorldSpace: false);
            collectableIcon.transform.localPosition = new Vector3(runningEndpoint + 0.5f, 0);                           

            collectableIcon.GetComponent<SpriteRenderer>().color = ThemeHandler.collectableColor(ThemeHandler.theme(type));

            runningEndpoint += (1f + spacing);
        }

        transform.localPosition = new Vector3(-runningEndpoint / 2, 0);

    }

}
