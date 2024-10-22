using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class CameraManager : MonoBehaviour
{
    public Camera mainCamera; // Assign the main camera in the Inspector or via code

    // Define the position and rotation for position1
    private Vector3 position1 = new Vector3(0, 0, -300);
    private Vector3 position2 = new Vector3(1000, 0, -300);
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);

    public float transitionSpeed = 2.0f; // Speed of camera movement

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // If not assigned, get the main camera
        }
    }

    // Function to move the camera to position1
    public void MoveToPosition1()
    {
        StartCoroutine(MoveCamera(position1, rotation));
    }
    public void MoveToPosition2()
    {
        StartCoroutine(MoveCamera(position2, rotation));
    }
    // Coroutine to move the camera smoothly to the desired position and rotation
    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f)
        {
            // Smoothly move the camera to the target position
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * transitionSpeed);

            // Smoothly rotate the camera to the target rotation
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);

            yield return null; // Wait for the next frame
        }

        // Snap to the exact position and rotation after finishing the smooth transition
        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }
}
