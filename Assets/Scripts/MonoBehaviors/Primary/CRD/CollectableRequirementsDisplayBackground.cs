using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class governing the behavior of the Collectable Requirements Display Background
/// </summary>
public class CollectableRequirementsDisplayBackground : MonoBehaviour
{

    /// <summary>
    /// Resizes the border to be able to house all the requirements
    /// </summary>
    /// <param name="requirements"></param>
    public void ShapeBorder(Inventory requirements)
    {

        var CRD = transform.parent.GetComponent<CollectableRequirementsDisplay>();

        float edgeBuffers = 2 * CRD.lengthwiseEdgeBuffer;
        float inBetweenSpacers = CRD.Spacing * (requirements.Dimension - 1);

        float x = Mathf.Max(edgeBuffers + requirements.Dimension + inBetweenSpacers, 0f);
        float y = 1 + (2 * CRD.heightwiseEdgeBuffer);

        gameObject.transform.localScale = new Vector3(x, y);
    }


}
