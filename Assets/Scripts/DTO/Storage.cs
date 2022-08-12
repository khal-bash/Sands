using UnityEngine;

namespace DTO.Storage
{
    
    /// <summary>
    /// A class that stores various important GameObjects.
    /// </summary>
    public static class StoredGameObjects
    {
        /// <summary>
        /// The Player <see cref="GameObject"/>.
        /// </summary>
        public static GameObject Player { get; set; }
    }

    /// <summary>
    /// A class that stores various important classes.
    /// </summary>
    public static class StoredClasses
    {
        /// <summary>
        /// The Player's <see cref="Health"/>.
        /// </summary>
        public static Health Player_HP { get; set; }

        /// <summary>
        /// The Player's <see cref="Inventory"/>.
        /// </summary>
        public static Inventory Player_Inventory { get; set; }

        /// <summary>
        /// The Game's <see cref="History"/>.
        /// </summary>
        public static History Player_History { get; set; }
        
    }

    /// <summary>
    /// A class that stores various important components.
    /// </summary>
    public static class StoredComponents
    {
        /// <summary>
        /// The Player's <see cref="Player"/> component.
        /// </summary>
        public static Player Player { get; set; }

        /// <summary>
        /// The Player's <see cref="Transform"/> component.
        /// </summary>
        public static Transform Player_Transform { get; set; }
    }

    /// <summary>
    /// A class that populates the other classes in <see cref="Storage"/>.
    /// </summary>
    public class Operations : MonoBehaviour
    {

        /// <summary>
        /// Populates the other classes in <see cref="Storage"/>.
        /// </summary>
        public static void Fill_Storage()
        {
            MakeFindCalls();
            MakeComponentCalls();
        }

        /// <summary>
        /// Makes all of the necessary <see cref="GameObject.Find(string)"/> calls.
        /// </summary>
        private static void MakeFindCalls()
        {
            StoredGameObjects.Player = GameObject.Find("Player");
        }

        /// <summary>
        /// Makes all of the necessary <see cref="GameObject.GetComponent{T}"/> calls.
        /// Also gets objects from classes
        /// </summary>
        private static void MakeComponentCalls()
        {
            StoredComponents.Player = StoredGameObjects.Player.GetComponent<Player>();
            StoredComponents.Player_Transform = StoredGameObjects.Player.transform;

            StoredClasses.Player_HP = StoredComponents.Player.HP;
            StoredClasses.Player_Inventory = StoredComponents.Player.Inventory;
            StoredClasses.Player_History = StoredComponents.Player.History;

        }

        
    }

}
