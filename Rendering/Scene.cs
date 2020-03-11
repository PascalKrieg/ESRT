using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRT.Entities;
using ESRT.Entities.Lighting;

namespace ESRT.Rendering
{
    public class Scene
    {
        public Color SkyColor { get; set; }
        public Camera MainCamera { get; set; }
        public List<IIntersectable> ObjectList { get; set; }
        public List<ILight> LightList { get; set; }

        public Scene(Color skyColor, Camera mainCamera)
        {
            SkyColor = skyColor;
            MainCamera = mainCamera;
            ObjectList = new List<IIntersectable>();
            LightList = new List<ILight>();
        }
    }
}
