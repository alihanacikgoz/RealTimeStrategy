using System;
using Runtime.ScriptableObjects;
using Runtime.Singletons;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField]
        private Transform cameraTarget;

        [FoldoutGroup("References"), SerializeField]
        private CinemachineCamera cinemachineCamera;

        [FoldoutGroup("Configs"), SerializeField]
        private CameraConfig cameraConfig;

        [FoldoutGroup("References")] private InputSystem_Actions _inputSystemActions;


        #region Private Variables

        private float _defaultPanSpeed, _zoomStartTime, _rotationStartTime, _maxRotationAmount, _cameraZoomOnYAxis;
        private Vector3 _startingFollowOffset;
        private CinemachineFollow _cinemachineFollow;

        private InputAction.CallbackContext _jump;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            if (!cinemachineCamera.TryGetComponent(out _cinemachineFollow))
            {
                Debug.LogError("Cinemachine Camera did  not have a CinemachineFollow component");
            }

            _startingFollowOffset = _cinemachineFollow.FollowOffset;
            _maxRotationAmount = Mathf.Abs(_cinemachineFollow.FollowOffset.z);
        }

        private void Start()
        {
            _defaultPanSpeed = cameraConfig.keyboardPanSpeed;
        }

        void Update()
        {
            HandleKeyboardMovement();
            HandleSlerpZoom();
            HandleRotation();
        }

        #endregion

        #region Custom Methods

        private bool ShouldSetRotationStartTime()
        {
            return Keyboard.current.pageUpKey.wasPressedThisFrame || Keyboard.current.pageDownKey.wasPressedThisFrame ||
                   Keyboard.current.pageUpKey.wasReleasedThisFrame || Keyboard.current.pageDownKey.wasReleasedThisFrame;
        }

        private void HandleRotation()
        {
            if (ShouldSetRotationStartTime())
            {
                _rotationStartTime = Time.time;
            }

            float rotationTime = Mathf.Clamp01((Time.time - _rotationStartTime) * cameraConfig.rotationSpeed);

            Vector3 targetFollowOffset;

            if (Keyboard.current.pageDownKey.isPressed)
            {
                targetFollowOffset = new Vector3(_maxRotationAmount, _cinemachineFollow.FollowOffset.y, 0);
            }
            else if (Keyboard.current.pageUpKey.isPressed)
            {
                targetFollowOffset = new Vector3(-_maxRotationAmount, _cinemachineFollow.FollowOffset.y, 0);
            }
            else
            {
                targetFollowOffset = new Vector3(_startingFollowOffset.x, _cinemachineFollow.FollowOffset.y,
                    _startingFollowOffset.z);
            }

            _cinemachineFollow.FollowOffset =
                Vector3.Slerp(_cinemachineFollow.FollowOffset, targetFollowOffset, rotationTime);
            ;
        }

        private void HandleKeyboardMovement()
        {
            Vector2 moveAmount = Vector2.zero;
            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
            {
                moveAmount.y += cameraConfig.keyboardPanSpeed;
            }

            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            {
                moveAmount.x -= cameraConfig.keyboardPanSpeed;
            }

            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                moveAmount.x += cameraConfig.keyboardPanSpeed;
            }

            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
            {
                moveAmount.y -= cameraConfig.keyboardPanSpeed;
            }

            HandleMouseZoom(moveAmount);
        }

        private void HandleMouseZoom(Vector2 moveAmount)
        {
            if (Mouse.current.scroll.ReadValue().y != 0)
            {
                if (cameraTarget.position.y >= -5.0 || cameraTarget.position.y <= 15.0)
                {
                    _cameraZoomOnYAxis = Mathf.Clamp(Mouse.current.scroll.ReadValue().y, -5, 15) *
                                         (cameraConfig.mouseZoomSpeed * Time.deltaTime);
                    cameraTarget.position =
                        new Vector3(cameraTarget.position.x, _cameraZoomOnYAxis, cameraTarget.position.z);
                }
            }


            moveAmount *= Time.deltaTime;
            cameraTarget.position += new Vector3(moveAmount.x, 0, moveAmount.y);
        }

        private bool ShouldSetZoomStartTime()
        {
            return Keyboard.current.shiftKey.wasPressedThisFrame ||
                   Keyboard.current.shiftKey.wasReleasedThisFrame;
        }

        private void HandleSlerpZoom()
        {
            if (ShouldSetZoomStartTime())
            {
                _zoomStartTime = Time.time;
            }

            Vector3 targetFollowOffset;

            float zoomTime = Mathf.Clamp01((Time.time - _zoomStartTime) * cameraConfig.zoomSpeed);

            if (Keyboard.current.shiftKey.isPressed)
            {
                targetFollowOffset = new Vector3(
                    _cinemachineFollow.FollowOffset.x,
                    cameraConfig.minZoomDistance,
                    _cinemachineFollow.FollowOffset.z
                );
            }
            else
            {
                targetFollowOffset = new Vector3(
                    _cinemachineFollow.FollowOffset.x,
                    _startingFollowOffset.y,
                    _cinemachineFollow.FollowOffset.z
                );
            }

            _cinemachineFollow.FollowOffset =
                Vector3.Lerp(_cinemachineFollow.FollowOffset, targetFollowOffset, zoomTime);
        }

        #endregion
    }
}