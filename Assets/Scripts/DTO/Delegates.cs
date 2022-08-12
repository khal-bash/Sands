using UnityEngine;

/// <summary>
/// Container for various types of delegate.
/// </summary>
namespace DTO.Delegates
{

    /// <summary>
    /// Delegate used for notification events.
    /// </summary>
    public delegate void Notify();

    /// <summary>
    /// Delagate passing an int.
    /// </summary>
    /// <param name="value">The value being passed.</param>
    public delegate void IntInputAction(int value);

    /// <summary>
    /// Delegate passing a <see cref="GameObject"/>.
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> being passed.</param>
    public delegate void GameObjectInputAction(GameObject gameObject);
}
