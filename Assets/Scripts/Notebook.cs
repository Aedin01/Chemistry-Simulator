using System.Collections;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    public float openAngle = 120f; // The angle to rotate to, typically 120 degrees for an open cover
    public float duration = 0.2f; // Duration of the rotation in seconds
    public Vector3 rotationAxis = Vector3.up; // Axis to rotate around, adjust as needed (usually Vector3.up for Y-axis)

    public Camera cameraToMove; // The camera to move and rotate

    private bool isAnimating = false;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Vector3 originalPosition;

    public Vector3 targetPosition; // The target position for the camera when the book is open
    public Vector3 targetEuler; // The target rotation in Euler angles when the book is open
    private Quaternion targetRotation; // The target rotation for the camera when the book is open

    private Vector3 initialCameraPosition; // Initial position of the camera
    private Quaternion initialCameraRotation; // Initial rotation of the camera
    public GameObject elements;
    public GameObject exit;
    public GameObject panel;
    public GameObject exitNotebook;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(rotationAxis * openAngle);
        originalPosition = transform.localPosition;
        targetRotation = Quaternion.Euler(targetEuler);

        // Store the initial position and rotation of the camera
        initialCameraPosition = cameraToMove.transform.position;
        initialCameraRotation = cameraToMove.transform.rotation;

        // Ensure panel is initially inactive
        panel.SetActive(false);
    }

    public void OnMouseDown()
    {
        if (!isAnimating && !isOpen)
        {
            StartCoroutine(OpenNotebook());
        }
    }

    public IEnumerator OpenNotebook()
    {
        elements.SetActive(false);
        exit.SetActive(false);
        isAnimating = true;
        yield return StartCoroutine(RotateCover(openRotation, duration));
        yield return StartCoroutine(DoCamera(targetPosition, targetRotation, duration, true));
        isOpen = true;
        isAnimating = false;
        exitNotebook.SetActive(true);
    }

    public IEnumerator CloseNotebook()
    {
        isAnimating = true;
        yield return StartCoroutine(DoCamera(initialCameraPosition, initialCameraRotation, duration, false));
        yield return StartCoroutine(RotateCover(closedRotation, duration));
        isOpen = false;
        isAnimating = false;
        exitNotebook.SetActive(false);
        elements.SetActive(true);
        exit.SetActive(true);
    }

    private IEnumerator RotateCover(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.localRotation;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;
    }

    private IEnumerator DoCamera(Vector3 newPosition, Quaternion newRotation, float duration, bool activatePanel)
    {
        Vector3 startPosition = cameraToMove.transform.position;
        Quaternion startRotation = cameraToMove.transform.rotation;
        float timeElapsed = 0;

        if (!activatePanel)
        {
            panel.SetActive(false);
        }

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            cameraToMove.transform.position = Vector3.Lerp(startPosition, newPosition, t);
            cameraToMove.transform.rotation = Quaternion.Slerp(startRotation, newRotation, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cameraToMove.transform.position = newPosition;
        cameraToMove.transform.rotation = newRotation;

        if (activatePanel)
        {
            panel.SetActive(true);
        }
    }
}