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

        //Properties set in code
        #region Code Properties

        /// <summary>
        /// A 5x5 matrix in raw form (indexed from [0,0] -> [4,4])
        /// </summary>
        public Floor[,] rawMatrix { get; private set; }

        #endregion

        //Class Construction
        #region Initialization

        public LevelInitializationMatrix()
        {
            rawMatrix = new Floor[5, 5];
        }

        #endregion

        //Editing the matrix
        #region Editing

        /// <summary>
        /// Adds a floor to the <see cref="rawMatrix"/>.
        /// </summary>
        /// <param name="x">The x-value in the matrix from -2 -> 2.</param>
        /// <param name="y">The y-value in the matrix from -2 -> 2.</param>
        /// <param name="floor">The floor to be added.</param>
        public void AddFloorFromWorldCoordinates(int x, int y, Floor floor)
        {

            x += 3; y += 3;
            rawMatrix[x, y] = floor;
            floor.matrix_X = x; floor.matrix_Y = y;
        }

        #endregion

        //Querying the matrix
        #region Querying

        /// <summary>
        /// Takes in a vector2 in world coordinates (x, y \in [-2,2])
        /// and returns the floor at that location if it exists.
        /// </summary>
        /// <param name="index">The index of the floor in the matrix.</param>
        /// <returns></returns>
        public Floor IndexFromWorldCoordinatesVector(Vector2 index)
        {
            try
            {
                int x = (int) index.x + 3;
                int y = (int) index.y + 3;
                return rawMatrix[x, y];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            
        }

        #endregion

    }
}
