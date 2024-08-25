#define USING_NEW_INPUT_SYSTEM
using System;
using DesignPatterns;
using UnityEngine;
using UnityEngine.InputSystem;

// Requires an Action Map 
namespace TurnBasedShooter.Input_Manager
{
    public class InputManager : PersistentSingleton<InputManager>
    {
#if USING_NEW_INPUT_SYSTEM
        private PlayerInputMap _playerInput;
        public Vector2 MousePosition => Mouse.current.position.ReadValue();
        public float MouseScrollDeltaY => Mouse.current.scroll.y.ReadValue();
        public bool IsLeftMouseClicked => Mouse.current.leftButton.wasPressedThisFrame;
#else
        public Vector2 MousePosition => Input.mousePosition;
        public float MouseScrollDeltaY => Input.mouseScrollDelta.y;
        public bool IsLeftMouseClicked => Input.GetKeyDown(KeyCode.Mouse0);
#endif

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            if (_playerInput == null)
                _playerInput = new PlayerInputMap();
            
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        public Vector2 GetCardinalDirection()
        {
#if USING_NEW_INPUT_SYSTEM
            return _playerInput.Player.CameraMovement.ReadValue<Vector2>();
#else
            Vector2 cameraAdjustments = new Vector2();
            
            if (Input.GetKey(KeyCode.W))
                cameraAdjustments.y += 1f;

            if (Input.GetKey(KeyCode.S))
                cameraAdjustments.y -= 1f;

            if (Input.GetKey(KeyCode.A))
                cameraAdjustments.x -= 1f;

            if (Input.GetKey(KeyCode.D))
                cameraAdjustments.x += 1f;

            return cameraAdjustments;
#endif
        }

        public Vector2 GetRotationDirection()
        {
#if USING_NEW_INPUT_SYSTEM
            return _playerInput.Player.CameraRotation.ReadValue<Vector2>(); // Keybind on Map are inverted to function properly
#else
            Vector2 cameraRotationAdjustments = new Vector2();
            
            if (Input.GetKey(KeyCode.E))
                cameraRotationAdjustments.y += 1f;

            if (Input.GetKey(KeyCode.Q))
                cameraRotationAdjustments.y -= 1f;

            if (Input.GetKey(KeyCode.G))
                cameraRotationAdjustments.x += 1f;

            if (Input.GetKey(KeyCode.T))
                cameraRotationAdjustments.x -= 1f;

            return cameraRotationAdjustments;
#endif
        }
    }
}
