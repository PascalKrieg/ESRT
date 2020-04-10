using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 ViewDirection { get; set; }
        public float FieldOfView { get; set; }

        public Camera(Vector3 position, Vector3 up, Vector3 viewDirection, float fieldOfView)
        {
            Position = position;
            Up = up;
            ViewDirection = viewDirection;
            FieldOfView = fieldOfView;
        }
    }
}
