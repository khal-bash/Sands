using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Setup
{
    /// <summary>
    /// Class holding a floor's knowledge of its neighbors.
    /// </summary>
    public class Neighbors
    {
        //Properties set in code
        #region Code Properties

        /// <summary>
        /// Dictionary mapping a direction to whether or not the is a neighbor present there. 
        /// </summary>
        public Dictionary<Vector2, bool> neighbors { get; set; }

        #endregion

        //Class Construction
        #region Initialization

        public Neighbors()
        {
            neighbors = new Dictionary<Vector2, bool>() {
                {Vector2.up, false},
                {Vector2.left, false},
                {Vector2.down, false},
                {Vector2.right, false},
            };
        }

        #endregion

    }
}
