using UnityEngine;
using Insolence.SaveUtility;
using Insolence.Core;
using System;
using System.Collections;

namespace Insolence.KinematicCharacterController
{
    public class KinematicPlayerControls : MonoBehaviour
    {
        [SerializeField] PlayerCharacterCamera OrbitCamera;
        [SerializeField] Transform CameraFollowPoint;
        [SerializeField] KineCharacterController Character;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] Inventory _inventory;
        [SerializeField][Range(.5f, 10f)] private float _speedMultiplier = 3f; //TODO: make this modifiable in the game settings

        private Vector2 _inputVector;
        float mouseLookAxisUp;
        float mouseLookAxisRight;
        bool _jumpTriggered = false;
        bool _crouchTriggered = false;
        bool _runTriggered = false;

        private bool _isRMBPressed = false;
        private bool _cameraMovementLock = false;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";


        private void OnEnable()
        {
            _inputReader.MoveEvent += OnMove;
            _inputReader.JumpEvent += OnJump;
            _inputReader.JumpCanceledEvent += OnJumpCanceled;
            _inputReader.CameraMoveEvent += OnCameraMove;
            _inputReader.EnableMouseControlCameraEvent += OnEnableMouseControlCamera;
            _inputReader.DisableMouseControlCameraEvent += OnDisableMouseControlCamera;
            _inputReader.CrouchEvent += OnCrouch;
            _inputReader.StartedRunning += OnStartedRunning;
            _inputReader.StoppedRunning += OnStoppedRunning;
            _inputReader.CycleWeaponEvent += OnCycleWeapon;
            _inputReader.DropWeaponEvent += OnDropWeapon;
            _inputReader.DualWieldOrShieldEquipEvent += OnDuaWieldOrShieldEquip;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent -= OnMove;
            _inputReader.JumpEvent -= OnJump;
            _inputReader.JumpCanceledEvent -= OnJumpCanceled;
            _inputReader.CameraMoveEvent -= OnCameraMove;
            _inputReader.EnableMouseControlCameraEvent -= OnEnableMouseControlCamera;
            _inputReader.DisableMouseControlCameraEvent -= OnDisableMouseControlCamera;
            _inputReader.CrouchEvent -= OnCrouch;
            _inputReader.StartedRunning -= OnStartedRunning;
            _inputReader.StoppedRunning -= OnStoppedRunning;
            _inputReader.CycleWeaponEvent -= OnCycleWeapon;
            _inputReader.DropWeaponEvent -= OnDropWeapon;
            _inputReader.DualWieldOrShieldEquipEvent -= OnDuaWieldOrShieldEquip;
        }
        private void Awake()
        {
            
            Cursor.lockState = CursorLockMode.None;
            
            OrbitCamera = GameObject.Find("OrbitCamera").GetComponent<PlayerCharacterCamera>();
            
            //Get Character GameObject with player tag at start
            Character = SaveUtils.GetPlayer().GetComponent<KineCharacterController>();

            // find CameraFollowPoint gameobject and set it's transform to CameraFollowPoint
            CameraFollowPoint = GameObject.Find("CameraFollowPoint").transform;

            // Tell camera to follow transform
            OrbitCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());

            _inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            if (OrbitCamera == null || CameraFollowPoint == null)
            {
                Debug.Log("resetting controls");

                Character = SaveUtils.GetPlayer().GetComponent<KineCharacterController>();

                OrbitCamera = Camera.main.GetComponent<PlayerCharacterCamera>();
                // find CameraFollowPoint gameobject and set it's transform to CameraFollowPoint
                CameraFollowPoint = GameObject.Find("CameraFollowPoint").transform;

                // Tell camera to follow transform
                OrbitCamera.SetFollowTransform(CameraFollowPoint);
            }
            if (_inventory == null) _inventory = SaveUtils.GetPlayer().GetComponent<Inventory>();



            HandleCharacterInput();
        }

        private void LateUpdate()
        {

            // Handle rotating the camera along with physics movers
            if (OrbitCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                OrbitCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * OrbitCamera.PlanarDirection;
                OrbitCamera.PlanarDirection = Vector3.ProjectOnPlane(OrbitCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            OrbitCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = _inputVector.y;
            characterInputs.MoveAxisRight = _inputVector.x;
            characterInputs.CameraRotation = OrbitCamera.Transform.rotation;
            characterInputs.JumpDown = _jumpTriggered;
            
            characterInputs.CrouchDown = _crouchTriggered;
            characterInputs.CrouchUp = !_crouchTriggered;
            characterInputs.DashDown = _runTriggered;
            characterInputs.DashUp = !_runTriggered;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }

        #region ----------- Event listeners ----------
        private void OnMove(Vector2 movement)
        {
            _inputVector = movement;
        }

        private void OnJump()
        {
            if (GetComponentInParent<CharacterInteraction>().HasInteractable())
            {
                return;
            }
            else
            {
                _jumpTriggered = true;
                _crouchTriggered = false;
            }

        }
        
        private void OnJumpCanceled()
        {
            _jumpTriggered = false;
        }

        private void OnCrouch()
        {
            _crouchTriggered = _crouchTriggered ? false : true;
        }

        private void OnStartedRunning()
        {
            _runTriggered = true;
            _crouchTriggered = false;
        }

        private void OnStoppedRunning()
        {
            _runTriggered = false;
        }
        private void OnEnableMouseControlCamera()
        {
            _isRMBPressed = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseControlForFrame());
        }

        IEnumerator DisableMouseControlForFrame()
        {
            _cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            _cameraMovementLock = false;
        }

        private void OnDisableMouseControlCamera()
        {
            _isRMBPressed = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // when mouse control is disabled, the input needs to be cleared
            // or the last frame's input will 'stick' until the action is invoked again
            mouseLookAxisUp = 0f;
            mouseLookAxisRight = 0f;

        }
        private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (_cameraMovementLock)
                return;

            if (isDeviceMouse && !_isRMBPressed)
                return;

            //Using a "fixed delta time" if the device is mouse,
            //since for the mouse we don't have to account for frame duration
            float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime + 3f;

            mouseLookAxisRight = cameraMovement.x * deviceMultiplier * _speedMultiplier;
            mouseLookAxisUp = cameraMovement.y * deviceMultiplier * _speedMultiplier;
        }
        
        private void OnCycleWeapon()
        {
            _inventory.CycleRightHandWeapons();
        }

        private void OnDropWeapon()
        {
            _inventory.DropItem(_inventory.equippedInRightHandSlot);
        }

        private void OnDuaWieldOrShieldEquip()
        {
            _inventory.DualWieldWeapons();
        }
        #endregion
    }
}