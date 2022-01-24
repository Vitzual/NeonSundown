using UnityEngine;

namespace MK.Glow
{
    /// <summary>
    /// Pipeline Independent pass of necessary camera data
    /// </summary>
    internal abstract class CameraData
    {
        internal int width = 2, height = 2;
        internal bool stereoEnabled = false;
        internal float aspect = 1;
        internal Matrix4x4 worldToCameraMatrix = Matrix4x4.identity;
        internal bool overwriteDescriptor = false;
    }
}
