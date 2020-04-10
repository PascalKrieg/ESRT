using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities
{
    /// <summary>
    /// Represents a camera in the scene with a position, orientation and field of view.
    /// This contains only camera attributes, but no rendering logic.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// The position of the camera in the world.
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// The up vector of the camera.
        /// </summary>
        public Vector3 Up { get; set; }
        /// <summary>
        /// The view direction of the camera. This is the actual view direction in which the primary rays will be sent.
        /// </summary>
        public Vector3 ViewDirection { get; set; }
        /// <summary>
        /// The horizontal field of view of the camera.
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Constructs a new Camera object.
        /// </summary>
        /// <param name="position">The position of the camera in the world.</param>
        /// <param name="up">The up vector of the camera.</param>
        /// <param name="viewDirection">The view direction of the camera. 
        /// This is the actual view direction in which the primary rays will be sent.</param>
        /// <param name="fieldOfView">The horizontal field of view of the camera.</param>
        public Camera(Vector3 position, Vector3 up, Vector3 viewDirection, float fieldOfView)
        {
            Position = position;
            Up = up;
            ViewDirection = viewDirection;
            FieldOfView = fieldOfView;
        }
    }
}
