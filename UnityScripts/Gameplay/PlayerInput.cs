namespace BindingsAEON
{ 
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using InControl;

    public class PlayerInput : MonoBehaviour
    {
        public CharacterMotor characterMotor;
        public GameObject[] touchControls;
        private Camera cam;
        public InventoryActivation inventoryActivation;
        public Vector3 CameraPositionDefault = new Vector3(0, 1.65f, -4.43f);
        public float CameraXRotationDefault = 0;
        public Vector3 CameraPositionLookUp = new Vector3(0, 1.39f, -0.55f);
        public Vector3 CameraPositionLookDown = new Vector3(0, 1.42f, -3.69f);
        private Vector2 inputCameraSensitivity = new Vector2(2f, 2f);
        private Vector2 inputCameraClamp = new Vector2(-60, 60);
        private float assistedCameraRotationSmoothDampTime = 0.05f;
        private float assistedCameraPositionSmoothDampTime = 0.05f;
        private float assistedCameraTimeout = 1.5f;
        private float assistedCameraSensitivity = 1.5f;
        private float assistedCameraResetTime = .7f;


        private Vector3 positionVelocity = Vector3.zero;
        private Vector3 rotationVelocity = Vector3.zero;
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private Vector3 cameraTargetPosition;
        private Vector3 cameraTargetRotation;
        private float inputCameraLastTime;
        private bool assistedCamera = true;
        private float assistedCameraLastTime = 0;

        [HideInInspector]
        public bool inInventory = false;

        [HideInInspector]
        public Vector2 inputCamera;
        [HideInInspector]
        public bool inputJump;
        [HideInInspector]
        public Vector3 inputMove;
        [HideInInspector]
        public float camYRotation;
        PlayerActionsAEON playerActions;
        string saveData;

        void OnEnable()
        {
            // See PlayerActions.cs for this setup.
            playerActions = PlayerActionsAEON.CreateWithDefaultBindings();
            //playerActions.Move.OnLastInputTypeChanged += ( lastInputType ) => Debug.Log( lastInputType );
            LoadBindings();
        }

        void OnDisable()
        {
            // This properly disposes of the action set and unsubscribes it from
            // update events so that it doesn't do additional processing unnecessarily.
            playerActions.Destroy();
        }

        void LoadBindings()
        {
            if (PlayerPrefs.HasKey("Bindings"))
            {
                saveData = PlayerPrefs.GetString("Bindings");
                playerActions.Load(saveData);
            }
        }

        void Start()
        {
            cam = Camera.main.GetComponent<Camera>();
            Input.simulateMouseWithTouches = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (Application.platform == RuntimePlatform.Android)
            {
                foreach (GameObject touchControl in touchControls)
                    touchControl.SetActive(true);
            }
            else
            {
                foreach (GameObject touchControl in touchControls)
                    touchControl.SetActive(false);
            }
        }

        public void TouchCamera()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if ((new Vector2(playerActions.Aim.X, -playerActions.Aim.Y).magnitude > 0))
                {
                    assistedCamera = false;
                    inputCameraLastTime = Time.time;
                }
            }
            else
            {
                if ((new Vector2(playerActions.Aim.X, -playerActions.Aim.Y).magnitude > 0) || new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0)
                {
                    assistedCamera = false;
                    inputCameraLastTime = Time.time;
                }
            }

            if (assistedCamera==false)
            {
                if (Time.time > inputCameraLastTime + assistedCameraTimeout)
                    assistedCamera = true;
            }

            inputMove = Vector2.ClampMagnitude(new Vector2(playerActions.Move.X, playerActions.Move.Y), 1);

            if (Application.platform == RuntimePlatform.Android)
                inputCamera = new Vector2(playerActions.Aim.X, -playerActions.Aim.Y);
            else
                inputCamera = new Vector2(Input.GetAxis("Mouse X") + playerActions.Aim.X, -Input.GetAxis("Mouse Y") - playerActions.Aim.Y);

            inputJump = false;
            camYRotation = cam.transform.eulerAngles.y;

            if (inInventory == false)
            {
                if (playerActions.Jump.WasPressed == true)
                {
                    inputJump = true;
                    characterMotor.InputJump();
                }
                characterMotor.camYRotation = camYRotation;
            }
            else
            {
                inputMove = new Vector2(0, 0);
            }
            characterMotor.inputMove = inputMove;

            if (playerActions.Inventory.WasPressed == true)
            {
                SwitchInventoryState();
            }

            if (inInventory == false)
            {
                if (assistedCamera == true)
                {
                    cameraTargetRotation += new Vector3(inputCamera.y * inputCameraSensitivity.y, (inputMove.x * assistedCameraSensitivity) + inputCamera.x * inputCameraSensitivity.x, 0);
                    //assistedCameraLastTime = Time.time;
                }
                else
                    cameraTargetRotation += new Vector3(inputCamera.y * inputCameraSensitivity.y, inputCamera.x * inputCameraSensitivity.x, 0);
            }

            var lockY = cameraTargetRotation;
            lockY.x = Mathf.Clamp(lockY.x, inputCameraClamp.x, inputCameraClamp.y);
            cameraTargetRotation = lockY;
            cameraTargetPosition = characterMotor.transform.position + Quaternion.AngleAxis(cameraTargetRotation.y, Vector3.up) * Quaternion.AngleAxis(cameraTargetRotation.x, Vector3.right) * VectorThreeLerpThree(CameraPositionLookDown, CameraPositionDefault, CameraPositionLookUp, map(cameraTargetRotation.x, inputCameraClamp.x, inputCameraClamp.y, 1, 0));

            if (assistedCamera == true)
            {
                assistedCameraLastTime = Time.time;
                var defaultRotX = cameraTargetRotation;
                defaultRotX.x = CameraXRotationDefault;
                cameraTargetRotation = defaultRotX;
            }

            float power = 1-Mathf.Clamp01((Time.time - assistedCameraLastTime) / assistedCameraResetTime);
            cameraPosition = Vector3.SmoothDamp(cam.transform.position, cameraTargetPosition, ref positionVelocity, assistedCameraPositionSmoothDampTime * power);
            float xAngle = Mathf.SmoothDampAngle(cam.transform.eulerAngles.x, cameraTargetRotation.x, ref rotationVelocity.x, assistedCameraRotationSmoothDampTime * power);
            float yAngle = Mathf.SmoothDampAngle(cam.transform.eulerAngles.y, cameraTargetRotation.y, ref rotationVelocity.y, assistedCameraRotationSmoothDampTime * power);
            float zAngle = Mathf.SmoothDampAngle(cam.transform.eulerAngles.z, cameraTargetRotation.z, ref rotationVelocity.z, assistedCameraRotationSmoothDampTime * power);
            cameraRotation = new Vector3(xAngle, yAngle, zAngle);
        }

        public void SwitchInventoryState()
        {
            if (inInventory == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                inInventory = false;

                if (Application.platform == RuntimePlatform.Android)
                {
                    foreach (GameObject touchControl in touchControls)
                        touchControl.SetActive(true);
                }
            }
            else if (inInventory == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                inInventory = true;
                if (Application.platform == RuntimePlatform.Android)
                {
                    foreach (GameObject touchControl in touchControls)
                        touchControl.SetActive(false);
                }
            }

            if (inventoryActivation != null)
                inventoryActivation.ActivateInventory();
        }

        private void LateUpdate()
        {
            cam.transform.position = cameraPosition;
            cam.transform.eulerAngles = cameraRotation;
        }

        private Vector3 VectorThreeLerpThree(Vector3 pointA, Vector3 pointB, Vector3 pointC, float lerp)
        {
            Vector3 positionA = Vector3.Lerp(pointA, pointB, lerp);
            Vector3 positionB = Vector3.Lerp(pointB, pointC, lerp);
            return Vector3.Lerp(positionA, positionB, lerp);
        }
        private float map(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
