using UnityEngine;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig", order = 0)]
    public class CameraConfig : ScriptableObject
    {
        public float keyboardPanSpeed = 5;
        public float mouseZoomSpeed = 15f;
        public float zoomSpeed = 0.05f;
        public float minZoomDistance  = 7.5f;
        public float rotationSpeed = 0.05f;
    }
}