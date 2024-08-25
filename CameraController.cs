using TurnBasedShooter.Input_Manager;
using Unity.Cinemachine;
using UnityEngine;

namespace TurnBasedShooter.CameraSystem
{
    public class CameraController : MonoBehaviour 
    {
        private const int MINIMUM_ZOOM = 0;
        private const int MAXIMUM_ZOOM = 10; 
        private const float MINIMUM_ZOOMSCALE = 0.1f;
        private const float MAXIMUM_ZOOMSCALE = 2f;

        [SerializeField] private CinemachineFollow _cinemachineFollow;
        [SerializeField] private CinemachineRecomposer _cinemachineRecomposer;
        [SerializeField] private CinemachineImpulseListener _cinemachineImpulseListener;
 
        [SerializeField] private float _cameraMoveSpeed = 5f;
        [SerializeField] private float _cameraRotationSpeed = 100f;
        [SerializeField] private float _cameraZoomAmount = 0.5f;
        [SerializeField] private float _cameraZoomScaleAmount = 0.1f;
        [SerializeField] private float _cameraTiltAmount = 0.5f;
        [SerializeField] private float _zoomLerpSpeed = 5f;  // New field for lerp speed

        private Vector3 _cameraMoveDir = Vector3.zero;
        private Vector3 _cameraRotationAngle = Vector3.zero;
        private float _targetZoom;
        private float _targetZoomScale;
        private float _targetTilt;

        private void Start()
        {
            _targetZoom = _cinemachineFollow.FollowOffset.y;
            _targetZoomScale = _cinemachineRecomposer.ZoomScale;
            _targetTilt = _cinemachineRecomposer.Tilt;
        }

        private void Update()
        {
            HandleDirectionInput();
            HandleRotationInput();
            HandleCameraZoom();
        }

        private void HandleDirectionInput()
        {
            _cameraMoveDir = InputManager.Instance.GetCardinalDirection();

            Vector3 cameraAlignment = (transform.forward * _cameraMoveDir.y) + (transform.right * _cameraMoveDir.x);
            transform.position += cameraAlignment * (_cameraMoveSpeed * Time.deltaTime);
        }

        private void HandleRotationInput()
        {
            _cameraRotationAngle = InputManager.Instance.GetRotationDirection();

            transform.eulerAngles += _cameraRotationAngle * (_cameraRotationSpeed * Time.deltaTime);
        }

        private void HandleCameraZoom()
        {
            if (InputManager.Instance.MouseScrollDeltaY < 0 && (_targetZoom < MAXIMUM_ZOOM && _targetZoomScale < MAXIMUM_ZOOMSCALE))
            {
                _targetZoom += _cameraZoomAmount;
                _targetZoomScale += _cameraZoomScaleAmount;
                _targetTilt += _cameraTiltAmount;
            }

            if (InputManager.Instance.MouseScrollDeltaY > 0 && (_targetZoom > MINIMUM_ZOOM && _targetZoomScale > MINIMUM_ZOOMSCALE))
            {
                _targetZoom -= _cameraZoomAmount;
                _targetZoomScale -= _cameraZoomScaleAmount;
                _targetTilt -= _cameraTiltAmount;
            }

            _cinemachineFollow.FollowOffset.y = Mathf.Lerp(_cinemachineFollow.FollowOffset.y, _targetZoom, _zoomLerpSpeed * Time.deltaTime);
            _cinemachineRecomposer.ZoomScale = Mathf.Lerp(_cinemachineRecomposer.ZoomScale, _targetZoomScale, _zoomLerpSpeed * Time.deltaTime);
            _cinemachineRecomposer.Tilt = Mathf.Lerp(_cinemachineRecomposer.Tilt, _targetTilt, _zoomLerpSpeed * Time.deltaTime);
        }
    }
}
