using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig", order = 0)]
    public class CameraConfig : ScriptableObject
    {
        [FoldoutGroup("Keyboard PAN")] public float keyboardPanSpeed = 5;
        [FoldoutGroup("Keyboard PAN")] public float rotationSpeed = 0.05f;
        
        [FoldoutGroup("Mouse Zoom")] public float mouseZoomSpeed = 15f;
        [FoldoutGroup("Mouse Zoom")] public float zoomSpeed = 0.05f;
        [FoldoutGroup("Mouse Zoom")] public float minZoomDistance  = 7.5f;
        
        [FoldoutGroup("Mouse PAN")] public bool enableEdgePan = true;
        [FoldoutGroup("Mouse PAN")] public float mousePanSpeed = 5;
        [FoldoutGroup("Mouse PAN")] public float edgePanSize = 50;
        
    }
}