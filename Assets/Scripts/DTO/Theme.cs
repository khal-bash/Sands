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

    public static class ThemeHandler
    {

        public static Color backgroundColor(Theme theme)
        {
            switch (theme)
            {
                case Theme.desert:
                    return new Color32(220, 104, 75, 255);
                case Theme.ocean:
                    return new Color32(60, 116, 189, 255);
                case Theme.grasslands:
                    return new Color32(189, 242, 99, 255);
                case Theme.temple:
                    return new Color32(45, 28, 20, 255);
                case Theme.snow:
                    return new Color32(208, 216, 217, 255);
                default:
                    return Color.clear;
            }
        }

        public static Color collectableColor(Theme theme)
        {
            switch (theme)
            {
                case Theme.desert:
                    return new Color32(31, 161, 118, 255);
                case Theme.ocean:
                    return new Color32(255, 190, 69, 255);
                case Theme.grasslands:
                    return new Color32(132, 31, 161, 255);
                case Theme.temple:
                    return new Color32(163, 32, 11, 255);
                case Theme.snow:
                    return new Color32(0, 0, 0, 255);
                default:
                    return Color.clear;
            }
        }

        public static Inventory.CollectableType collectableType(Theme theme)
        {
            switch (theme)
            {
                case Theme.desert:
                    return Inventory.CollectableType.diamond;
                case Theme.ocean:
                    return Inventory.CollectableType.seashell;
                case Theme.grasslands:
                    return Inventory.CollectableType.lavender;
                case Theme.temple:
                    return Inventory.CollectableType.ruby;
                case Theme.snow:
                    return Inventory.CollectableType.coal;
                default:
                    return Inventory.CollectableType.unspecified;
            }
        }

        public static Theme theme(Inventory.CollectableType type)
        {
            switch (type)
            {
                case Inventory.CollectableType.diamond:
                    return Theme.desert;
                case Inventory.CollectableType.seashell:
                    return Theme.ocean;
                case Inventory.CollectableType.lavender:
                    return Theme.grasslands;
                case Inventory.CollectableType.ruby:
                    return Theme.temple;
                case Inventory.CollectableType.coal:
                    return Theme.snow;
                default:
                    return Theme.unspecified;
            }
        }

    }
}
