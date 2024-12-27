using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MinimapController : MonoBehaviour
{
    public GameObject minimapCanvas;
    public GameObject minimapFullscreenCanvas;
    public Image overlayPanel;
    public Camera largeMinimapCamera;
    public GameObject slot;

    public float largeMaxFOV = 60f;
    public float largeMinFOV = 20f;
    public float smallFOV = 30f;
    public float scrollSensitivity = 10f;
    public float dragSpeed = 2f;
    public float zoomSmoothSpeed = 5f;

    private bool isMinimapFullscreen = false;
    private Vector3 dragOrigin;
    private float targetFOV;

    public static MinimapController Instance;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
        }
        else
        {
            MinimapController.Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event to avoid memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCamera();
    }

    private void FindCamera()
    {
        largeMinimapCamera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();
        if (largeMinimapCamera == null)
        {
            Debug.LogWarning("No Map Camera found in the scene.");
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleZoom();
        HandleDrag();
    }

    private void HandleInput()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMinimapSize();
            ToggleSlotVisibility();
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

    private void ToggleMinimapSize()
    {
        isMinimapFullscreen = !isMinimapFullscreen;
        minimapCanvas.SetActive(!isMinimapFullscreen);
        minimapFullscreenCanvas.SetActive(isMinimapFullscreen);
        overlayPanel.enabled = isMinimapFullscreen;

        Time.timeScale = isMinimapFullscreen ? 0.5f : 1f;
        targetFOV = isMinimapFullscreen ? largeMaxFOV : smallFOV;
        largeMinimapCamera.transform.localPosition = isMinimapFullscreen ? Vector3.zero : Vector3.back * 10f;
    }

    private void StartDragging()
    {
        dragOrigin = Input.mousePosition;
        Cursor.visible = false;
    }

    private void StopDragging()
    {
        Cursor.visible = true;
    }

    private void HandleZoom()
    {
        if (isMinimapFullscreen)
        {
            float scrollValue = Input.GetAxis("Mouse ScrollWheel");
            if (scrollValue != 0)
            {
                targetFOV = Mathf.Clamp(targetFOV + scrollValue * scrollSensitivity, largeMinFOV, largeMaxFOV);
            }

            float newFOV = Mathf.Lerp(largeMinimapCamera.fieldOfView, targetFOV, zoomSmoothSpeed * Time.deltaTime);
            largeMinimapCamera.fieldOfView = newFOV;
        }
    }

    private void HandleDrag()
    {
        if (isMinimapFullscreen && Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0);
            largeMinimapCamera.transform.Translate(move, Space.World);
        }
    }

    private void ToggleSlotVisibility()
    {
        slot.SetActive(!isMinimapFullscreen);
    }
}