using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT.Entities.Materials
{
    /// <summary>
    /// Abstract Material containing all parameters required for the phong shading model,
    /// </summary>
    public abstract class Material
    {
        /// <summary>
        /// Returns the color at the texture coordinates.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>Returns the color at the texture coordinates.</returns>
        public abstract Color Color(float u, float v);
        /// <summary>
        /// Returns the color at the texture coordinates.
        /// </summary>
        /// <param name="textureCoords">texture coordinates</param>
        /// <returns>Returns the color at the texture coordinates.</returns>
        public Color Color((float u, float v) textureCoords)
        {
            return Color(textureCoords.u, textureCoords.v);
        }

        /// <summary>
        /// The mode in which the object will be rendered.
        /// </summary>
        public Shading ShadingMode { get; set; }

        /// <summary>
        /// Returns the ambient parameter at the texture coordinates.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>Returns the ambient parameter at the texture coordinates.</returns>
        public abstract float Ambient(float u, float v);
        /// <summary>
        /// Returns the ambient parameter at the texture coordinates.
        /// </summary>
        /// <param name="textureCoords">texture coordinates</param>
        /// <returns>Returns the ambient parameter at the texture coordinates.</returns>
        public float Ambient((float u, float v) textureCoords)
        {
            return Ambient(textureCoords.u, textureCoords.v);
        }

        /// <summary>
        /// Returns the diffuse parameter at the texture coordinates.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>Returns the diffuse parameter at the texture coordinates.</returns>
        public abstract float Diffuse(float u, float v);
        /// <summary>
        /// Returns the diffuse parameter at the texture coordinates.
        /// </summary>
        /// <param name="textureCoords">texture coordinates</param>
        /// <returns>Returns the diffuse parameter at the texture coordinates.</returns>
        public float Diffuse((float u, float v) textureCoords)
        {
            return Diffuse(textureCoords.u, textureCoords.v);
        }

        /// <summary>
        /// Returns the specular parameter at the texture coordinates.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>Returns the specular parameter at the texture coordinates.</returns>
        public abstract float Specular(float u, float v);
        /// <summary>
        /// Returns the specular parameter at the texture coordinates.
        /// </summary>
        /// <param name="textureCoords">texture coordinates</param>
        /// <returns>Returns the specular parameter at the texture coordinates.</returns>
        public float Specular((float u, float v) textureCoords)
        {
            return Specular(textureCoords.u, textureCoords.v);
        }

        /// <summary>
        /// Returns the transmissive parameter at the texture coordinates.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>Returns the transmissive parameter at the texture coordinates.</returns>
        public abstract float Transmissive(float u, float v);
        /// <summary>
        /// Returns the transmissive parameter at the texture coordinates.
        /// </summary>
        /// <param name="textureCoords">texture coordinates</param>
        /// <returns>Returns the transmissive parameter at the texture coordinates.</returns>
        public float Transmissive((float u, float v) textureCoords)
        {
            return Transmissive(textureCoords.u, textureCoords.v);
        }

        /// <summary>
        /// Returns the reflectivity at the texture coordinates.
        /// </summary>
        /// <param name="u">u coordinate</param>
        /// <param name="v">v coordinate</param>
        /// <returns>Returns the reflectivity at the texture coordinates.</returns>
        public abstract float Reflective(float u, float v);
        /// <summary>
        /// Returns the reflectivity at the texture coordinates.
        /// </summary>
        /// <param name="textureCoords">texture coordinates</param>
        /// <returns>Returns the reflectivity at the texture coordinates.</returns>
        public float Reflective((float u, float v) textureCoords)
        {
            return Reflective(textureCoords.u, textureCoords.v);
        }

        protected Material(Shading shadingMode)
        {
            ShadingMode = shadingMode;
        }
    }
}
