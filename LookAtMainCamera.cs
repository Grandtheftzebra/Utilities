using SpaghettiStudios.ExtensionMethods;
using UnityEngine;

namespace SpaghettiStudios.CameraUtility
{
    /// <summary>
    /// Any object with this script attached and the _invertView bool active will rotate towards the mainCamera 
    /// </summary>
    public class LookAtMainCamera : MonoBehaviour
    {
        [SerializeField] private bool _invertView;
        
        private Transform _cameraTransform;

        private void Start() => _cameraTransform = Camera.main.transform;
        private void LateUpdate() => HandleCameraLookAt();

        private void HandleCameraLookAt()
        {
            if (_invertView)
            {
                Vector3 camLookAtDir = (_cameraTransform.position - transform.position).normalized;
                transform.LookAt(transform.position.Offset(-camLookAtDir)); // Offset is an extension method written by me, it simply adds two vectors!
            }
            else
                transform.LookAt(_cameraTransform);
        }
    }
}
