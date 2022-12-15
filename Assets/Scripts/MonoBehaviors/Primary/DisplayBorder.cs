using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBorder : MonoBehaviour
{

    /// <summary>
    /// The buffer to place on the lengthwise edges of the display. 
    /// </summary>
    public float lengthwiseEdgeBuffer;

    /// <summary>
    /// The buffer to place on the heightwise edges of the display. 
    /// </summary>
    public float heightwiseEdgeBuffer;

    /// <summary>
    /// Resizes the border to be able to house all the requirements
    /// </summary>
    /// <param name="requirements"></param>
    public void ShapeBorder(Inventory requirements)
    {
        float x = Mathf.Max((2 * requirements.dimension - 1) + (2 * lengthwiseEdgeBuffer), 0f);
        float y = 1 + (2 * heightwiseEdgeBuffer);

        gameObject.transform.localScale = new Vector3(x, y);
    }


}
