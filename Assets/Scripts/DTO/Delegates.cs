using UnityEngine;

/// <summary>
/// Container for various delagates.
/// </summary>
namespace DTO.Delegates
{

    /// <summary>
    /// Delegate passing a <see cref="GameObject"/> and a <see cref="Color"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="color"></param>
    public delegate void ChangeGameObjectColorAction(GameObject gameObject, Color color);

    /// <summary>
    /// Delegate passing a <see cref="GameObject"/>.
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> being passed.</param>
    public delegate void GameObjectInputAction(GameObject gameObject);

    /// <summary>
    /// Delagate passing an int.
    /// </summary>
    /// <param name="value">The value being passed.</param>
    public delegate void IntInputAction(int value);

    /// <summary>
    /// Delegate used for notification events.
    /// </summary>
    public delegate void Notify();

}
