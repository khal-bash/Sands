using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{

    /// <summary>
    /// Class containing various useful Vector functions
    /// </summary>
    public class Vector
    {

        // Vector Functions
        #region Vector Functions

        /// <summary>
        /// Gets <see cref="Quaternion.Euler(float, float, float)" from a Vector3/>
        /// </summary>
        /// <param name="vector">The vector to be converted.</param>
        /// <param name="convention">The adjustment required to meet the angle sign conventions</param>
        /// <returns>Vector3 of Euler Angles (x,y = 0) </returns>
        public static Vector3 Get2DRotationFromVector3(Vector3 vector, float convention = 90)
        {
            float euler_z = Mathf.Atan(vector.y / vector.x) * (180 / Mathf.PI);
            euler_z += convention;

            return new Vector3(0, 0, euler_z);
        }

        public static float GetAngleBetween(Vector3 vector1, Vector3 vector2)
        {
            float dot_product = Vector3.Dot(vector1, vector2);
            float theta = Mathf.Acos(dot_product / (vector1.magnitude * vector2.magnitude));
            return theta;
        }

        #endregion
    }

    /// <summary>
    /// Class containing various useful visual functions.
    /// </summary>
    public class Visual
    {

        /// <summary>
        /// Changes a color's opacity. This change is not applied to the reference color,
        /// rather a new color is outputted.
        /// </summary>
        /// <param name="old_color">The original color.</param>
        /// <param name="opacity">The desired opacity.</param>
        /// <returns>A new <see cref="Color"/> with the same RGB values and updated opacity.</returns>
        public static Color ChangeOpacity(Color old_color, float opacity)
        {
            return new Color(old_color.r, old_color.g, old_color.b, a: opacity);
        }

    }
}