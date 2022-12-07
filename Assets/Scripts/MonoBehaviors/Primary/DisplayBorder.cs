using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBorder : MonoBehaviour
{

    public float lengthwiseEdgeBuffer;

    public float heightwiseEdgeBuffer;

    public void ShapeBorder(Inventory requirements, bool isVertical)
    {
        float x = (2 * requirements.dimension - 1) + (2 * lengthwiseEdgeBuffer);
        float y = 1 + (2 * heightwiseEdgeBuffer);

        if (isVertical)
        {
            var _x = x;
            x = y;
            y = _x;
        }

        gameObject.transform.localScale = new Vector3(x, y);
    }
}
