using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Player : GameObject
    {
        public int health = 10;

        public Player()
        {
            verticalPosition = 0;
            horizontalPosition = 0;
            rotationDegree = 0;
        }
    }
}
