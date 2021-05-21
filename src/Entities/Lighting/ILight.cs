
namespace ESRT.Entities.Lighting
{
    /// <summary>
    /// Represents a light source.
    /// </summary>
    public interface ILight
    {
        /// <summary>
        /// The position of the light source.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Gets the intensity of the light.
        /// </summary>
        /// <param name="outDirection">The outgoing light direction.</param>
        /// <returns>Returns the light intensity.</returns>
        Color GetIntensity(Vector3 outDirection);
    }
}
