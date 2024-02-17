using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MinimapController : MonoBehaviour
{
    public GameObject minimapCanvas;
    public GameObject minimapFullscreenCanvas;
    public Image overlayPanel;
    public Camera largeMinimapCamera;

    public float largeMaxFOV = 60f;
    public float largeMinFOV = 20f;
    public float smallFOV = 30f;
    public float scrollSensitivity = 10f;
    public float dragSpeed = 2f;
    public float zoomSmoothSpeed = 5f;
    public float zoomTransitionDuration = 0.5f;

    private bool isMinimapFullscreen = false;
    private Vector3 dragOrigin;
    private float scrollValue = 0f;
    private float targetFOV;

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleZoom();
        HandleDrag();
    }

    void HandleInput()
    {
        scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMinimapSize();
        }

        if (isMinimapFullscreen && Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            StopDragging();
        }

    }

    void ToggleMinimapSize()
    {
        isMinimapFullscreen = !isMinimapFullscreen;
        minimapCanvas.SetActive(!isMinimapFullscreen);
        minimapFullscreenCanvas.SetActive(isMinimapFullscreen);
        overlayPanel.enabled = isMinimapFullscreen;

        if (isMinimapFullscreen)
        {
            Time.timeScale = 0.5f;
            targetFOV = largeMaxFOV;
            largeMinimapCamera.transform.localPosition = Vector3.zero;
        }
        else
        {
            Time.timeScale = 1f;
            targetFOV = smallFOV;
        }
    }

    void StartDragging()
    {
        dragOrigin = Input.mousePosition;
        Cursor.visible = false;
    }

    void StopDragging()
    {
        Cursor.visible = true;
    }

    void HandleZoom()
    {
        if (isMinimapFullscreen && scrollValue != 0)
        {
            targetFOV = Mathf.Clamp(targetFOV + scrollValue * scrollSensitivity, largeMinFOV, largeMaxFOV);
        }

        float newFOV = Mathf.Lerp(largeMinimapCamera.fieldOfView, targetFOV, zoomSmoothSpeed * Time.deltaTime);
        largeMinimapCamera.fieldOfView = newFOV;
    }

    void HandleDrag()
    {
        if (isMinimapFullscreen && Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0);
            largeMinimapCamera.transform.Translate(move, Space.World);
        }
    }

}