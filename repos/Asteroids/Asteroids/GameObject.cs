using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class GameObject
    {
        public float horizontalPosition;
        public float verticalPosition;
        public float rotationDegree;
        public float speed;
        public float size;

        public float verticalMin;
        public float verticalMax;
        public float horizontalMax;
        public float horizontalMin;

        public void BoundingVolume()
        {
            verticalMax = verticalPosition + size;
            verticalMin = verticalPosition;
            horizontalMin = horizontalPosition;
            horizontalMax = horizontalPosition + size;
        }

        public bool CompareVolumes(float vMin, float vMax, float hMin, float hMax)
        {
            if ((verticalMax > vMin && vMin > verticalMin) || (verticalMin < vMax && vMax < verticalMin))
            {
                if ((horizontalMax > hMin && hMin > horizontalMin) || (horizontalMin < hMax && hMax < horizontalMax))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
