using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Give the object the ability to glitch (flash briefly)
/// </summary>
public class Glitch : MonoBehaviour
{
    /// Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// Gets or sets the amount of time (in seconds) the Glitch effect should flash if triggered.
    /// </summary>
    public float Flash_Time;

    #endregion

    /// Built-in Unity Functions
    #region Unity Functions

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player entrant = collider.GetComponent<Player>();

        if (entrant != null)
        {
            entrant.Glitchy = true;
            entrant.Glitch_Zone = gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Player entrant = collider.GetComponent<Player>();

        if (entrant != null)
        {
            entrant.Glitchy = false;
            entrant.Glitch_Zone = null;
        }
    }

    #endregion

    /// Glitch Effect
    #region Glitch Effect

    /// <summary>
    /// Causes the <c>gameObject</c> to flash briefly.
    /// </summary>
    public void ShowGlitchZone()
    {
        StartCoroutine(ColorFlash(Flash_Time));
    }
    
    /// <summary>
    /// A coroutine that toggles the opacity and then yields control.
    /// </summary>
    /// <param name="wait_time">
    /// The length of the flash. This is set by <see cref="Flash_Time"/>
    /// </param>
    IEnumerator ColorFlash(float wait_time)
    {
        Color color = gameObject.GetComponent<SpriteRenderer>().color;

        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.75f);
        yield return new WaitForSeconds(wait_time);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
    }
    #endregion

}
