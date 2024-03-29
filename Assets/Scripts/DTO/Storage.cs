using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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

        public static GameObject Generator { get; set; }
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

        public static Inventory Player_Inventory { get => StoredComponents.Player.Inventory; set => StoredComponents.Player.Inventory = value; }

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

        /// <summary>
        /// The parameters used to generate the level.
        /// </summary>
        public static LevelMetaData LevelMetaData { get; set; }

        /// <summary>
        /// The wizard that sets up the level.
        /// </summary>
        public static LevelSetupWizard LevelSetupWizard { get; set; }
    }

    /// <summary>
    /// A class that stores miscellaneous constants.
    /// </summary>
    public static class StoredConstants 
    {

        /// <summary>
        /// A list containing the Vector2s Up, Down, Left and Right
        /// </summary>
        public static List<Vector2Int> UDLR = new List<Vector2Int> { Vector2Int.up,
                                                                     Vector2Int.down,
                                                                     Vector2Int.left,
                                                                     Vector2Int.right };

        /// <summary>
        /// A list containing the Vector2s UL, DL, UR, and DR. These are, critically, NOT unit vectors.
        /// </summary>
        public static List<Vector2Int> DiagonalAdjacencies = new List<Vector2Int> { Vector2Int.up + Vector2Int.left,
                                                                                    Vector2Int.down + Vector2Int.left,
                                                                                    Vector2Int.up + Vector2Int.right,
                                                                                    Vector2Int.down + Vector2Int.right };
    
        /// <summary>
        /// A list containing all king's move from a square.
        /// </summary>
        public static List<Vector2Int> Adjacencies {
            get
            {
                return Enumerable.Concat<Vector2Int>(UDLR, DiagonalAdjacencies).ToList();
            }
        }
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
            StoredGameObjects.Generator = GameObject.Find("Generator");
        }

        /// <summary>
        /// Makes all of the necessary <see cref="GameObject.GetComponent{T}"/> calls.
        /// Also gets objects from classes
        /// </summary>
        private static void MakeComponentCalls()
        {
            StoredComponents.Player = StoredGameObjects.Player.GetComponent<Player>();
            StoredComponents.Player_Transform = StoredGameObjects.Player.transform;
            StoredComponents.LevelMetaData = StoredGameObjects.Generator.GetComponent<LevelMetaData>();
            StoredComponents.LevelSetupWizard = StoredGameObjects.Generator.GetComponent<LevelSetupWizard>();

            StoredClasses.Player_HP = StoredComponents.Player.HP;
            StoredClasses.Player_History = StoredComponents.Player.History;

        }

        
    }

}
