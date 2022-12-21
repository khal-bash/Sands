using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Setup
{
    /// <summary>
    /// Class storing the location of the floors in the generated level.
    /// </summary>
    public class LevelInitializationMatrix
    {

        //Custom Indexers for convenience
        #region Custom Indexers

        /// <summary>
        /// World Coordinates Indexer. Centers the matrix at (2 , 2)
        /// </summary>
        /// <param name="x">The x value in range (-2, 2) </param>
        /// <param name="y">The y value in range (-2, 2)</param>
        /// <returns>The floor at the indexed location, if one exists.</returns>
        public Floor this[int x, int y] {
            get { return backgroundMatrix[x + 2, y + 2]; }
            set { backgroundMatrix[x + 2 , y + 2] = value; }
        }

        /// <summary>
        /// World Coordinates Indexer. Centers the matrix at (2 , 2)
        /// </summary>
        /// <param name="vector">The x, y values in range (-2, 2) as a vector.</param>
        /// <returns>The floor at the indexed location, if one exists.</returns>
        public Floor this[Vector2Int vector] {
            get { return this[vector.x, vector.y]; }
            set { this[vector.x, vector.y] = value; }
        }

        #endregion

        //Properties set in code
        #region Code Properties

        /// <summary>
        /// The background 5x5 matrix in raw form (indexed from [0,0] -> [4,4])
        /// </summary>
        private Floor[,] backgroundMatrix { get; set; }

        /// <summary>
        /// The origin of the matrix (i.e [0,0]).
        /// </summary>
        public Floor origin { get { return this[0, 0]; } }

        #endregion

        //Class Construction
        #region Initialization

        public LevelInitializationMatrix()
        {
            backgroundMatrix = new Floor[5, 5];
        }

        #endregion

        /// <summary>
        /// Checks to see if a member of this matrix has a neighbor in the given direction.
        /// </summary>
        /// <param name="location">The location of the floor.</param>
        /// <param name="direction">The direction of the neighbor.</param>
        /// <returns></returns>
        public bool CheckForNeighbor(Vector2Int location, Vector2Int direction)
        {
            try
            {
                if (this[location + direction] != null)
                {
                    return true;
                }
            }
            catch (IndexOutOfRangeException) { }
            return false;
        }

    }
}
