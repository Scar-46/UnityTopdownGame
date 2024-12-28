using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    // Static instance of the singleton
    public static DoNotDestroy Instance { get; private set; }

    private void Awake()
    {
        // Check if there is already an instance of this class
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading a new scene
        }
        else
        {
            // If an instance already exists, destroy the new one to enforce the singleton pattern
            Destroy(gameObject);
        }
    }
}
