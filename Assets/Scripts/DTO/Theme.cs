using UnityEngine;

namespace DTO.Visuals
{

    /// <summary>
    /// The possible themes of floors.
    /// </summary>
    public enum Theme
    {
        unspecified,
        desert,
        ocean,
        grasslands,
        temple,
        snow
    }

    /// <summary>
    /// Class that determines the color to use for various objects in accordance with the theme.
    /// Overloads the Set Function to perform a wide variety of different conversions.
    /// </summary>
    public static class ThemeHandler
    {

        /// <summary>
        /// Gets the required color for an object to accord with the theme.
        /// </summary>
        /// <param name="theme">The theme being used.</param>
        /// <param name="targetType">The type of object whose color to set.
        /// <list>
        /// <item><term>"background"</term> <description>Set the background color of a floor.</description></item>
        /// <item><term>"collectable"</term> <description>Set the color of a collectable.</description></item>
        /// <item><term>"minimap"</term> <description>Set the color of a floor in the minimap.</description></item>
        /// </list>
        /// </param>
        public static Color Set(Theme theme, string targetType = "")
        {
            switch (targetType)
            {
                case "background":
                    return BackgroundColor(theme);
                case "collectable":
                    return CollectableColor(theme);
                case "minimap":
                    return MiniMapBackgroundColor(theme);
                default:
                    Debug.LogWarning("Could not set color - type not recognized.");
                    return Color.clear;
            }
        }

        /// <summary>
        /// Gets the required <see cref="Inventory.CollectableType"/> in accodrance with the theme.
        /// </summary>
        /// <param name="theme">The theme being used.</param>
        public static Inventory.CollectableType Set(Theme theme)
        {
            return CollectableType(theme);
        }

        /// <summary>
        /// Gets the required color from a given <see cref="Inventory.CollectableType"/>.
        /// Uses theme as an intermediary.
        /// </summary>
        /// <param name="type">The collecatable type to accord with.</param>
        /// <param name="targetType">The type of object whose color to set.
        /// <list>
        /// <item><term>"collectable"</term> <description>Set the color of a collectable.</description></item>
        /// </list></param>
        /// <returns></returns>
        public static Color Set(Inventory.CollectableType type, string targetType = "collectable")
        {
            switch (targetType)
            {
                case "collectable":
                    return CollectableColor(GetTheme(type));
                default:
                    Debug.LogWarning("Could not set color - type not recognized.");
                    return Color.clear;
            }
        }

        //This region is just switch statements.
        #region Setting Colors

        private static Color BackgroundColor(Theme theme)
        {
            return theme switch
            {
                Theme.desert => new Color32(220, 104, 75, 255),
                Theme.ocean => new Color32(60, 116, 189, 255),
                Theme.grasslands => new Color32(189, 242, 99, 255),
                Theme.temple => new Color32(45, 28, 20, 255),
                Theme.snow => new Color32(208, 216, 217, 255),
                _ => Color.clear,
            };
        }

        private static Color CollectableColor(Theme theme)
        {
            return theme switch
            {
                Theme.desert => new Color32(31, 161, 118, 255),
                Theme.ocean => new Color32(255, 190, 69, 255),
                Theme.grasslands => new Color32(132, 31, 161, 255),
                Theme.temple => new Color32(163, 32, 11, 255),
                Theme.snow => new Color32(0, 0, 0, 255),
                _ => Color.clear,
            };
        }

        private static Inventory.CollectableType CollectableType(Theme theme)
        {
            return theme switch
            {
                Theme.desert => Inventory.CollectableType.diamond,
                Theme.ocean => Inventory.CollectableType.seashell,
                Theme.grasslands => Inventory.CollectableType.lavender,
                Theme.temple => Inventory.CollectableType.ruby,
                Theme.snow => Inventory.CollectableType.coal,
                _ => Inventory.CollectableType.unspecified,
            };
        }

        private static Color MiniMapBackgroundColor(Theme theme)
        {
            return theme switch
            {
                Theme.desert => new Color32(194, 91, 66, 255),
                Theme.ocean => new Color32(60, 116, 189, 255),
                Theme.grasslands => new Color32(189, 242, 99, 255),
                Theme.temple => new Color32(45, 28, 20, 255),
                Theme.snow => new Color32(208, 216, 217, 255),
                _ => Color.clear,
            };
        }

        #endregion

        //Same thing here.
        #region Getting Theme

        private static Theme GetTheme(Inventory.CollectableType type)
        {
            return type switch
            {
                Inventory.CollectableType.diamond => Theme.desert,
                Inventory.CollectableType.seashell => Theme.ocean,
                Inventory.CollectableType.lavender => Theme.grasslands,
                Inventory.CollectableType.ruby => Theme.temple,
                Inventory.CollectableType.coal => Theme.snow,
                _ => Theme.unspecified,
            };
        }

        #endregion

    }
}
