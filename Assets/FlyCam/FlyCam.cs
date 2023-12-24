using UnityEngine;

namespace intervales.utils
{
    public class FlyCam : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public Camera firstCamera;
        public Camera secondCamera;

        private float xRotation = 0f;
        private float yRotation = 0f;

        void Start()
        {
            firstCamera.enabled = true;
            secondCamera.enabled = false;
            // Cache le curseur
            Cursor.visible = false;
            // Verrouille le curseur au centre de l'écran
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (secondCamera.enabled)
            {
                // Get mouse input
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                // Apply vertical rotation (pitch)
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                // Apply horizontal rotation (yaw)
                yRotation += mouseX;
                yRotation = yRotation % 360; // Optional: keeps the rotation value from overflowing

                // Apply the rotations to the camera
                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

                // Optionally, align the player body's y rotation with the camera's y rotation
                //playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(Vector3.forward * 5f * Time.deltaTime);
                }
            }

            if (Input.GetKeyDown(KeyCode.C)) // Change 'C' to your preferred key
            {
                // Toggle the enabled state of the cameras
                firstCamera.enabled = !firstCamera.enabled;
                secondCamera.enabled = !secondCamera.enabled;
            }
        }
    }
}
