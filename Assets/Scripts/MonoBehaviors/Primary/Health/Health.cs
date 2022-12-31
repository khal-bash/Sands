using UnityEngine;
using DTO.Delegates;

/// <summary>
/// Tracks an object's health.
/// </summary>
public class Health : MonoBehaviour
{
    // Properties Set in Inspector
    #region Inspector Properties

    /// <summary>
    /// The current HP value.
    /// </summary>
    public int HP;

    #endregion

    // Properties Set in Code
    #region Code Properties

    /// <summary>
    /// An event notifying subscriber that the HP has been Updated.
    /// </summary>
    public event IntInputAction HPUpdated;

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    void Start()
    {
        ChangeHP(0);
    }

    #endregion

    // Event Handlers
    #region Event Handlers

    /// <summary>
    /// <see cref="HPUpdated"/> handler.
    /// </summary>
    protected virtual void OnHPUpdate()
    {
        HPUpdated?.Invoke(HP);
    }

    #endregion

    // Changing HP
    #region HP

    /// <summary>
    /// Change the <see cref="HP"/>.
    /// </summary>
    /// <param name="change">The amount to be added to the <see cref="HP"/></param>
    public void ChangeHP(int change)
    {
        HP += change;
        OnHPUpdate();
    }

    #endregion

}
