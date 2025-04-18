using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButtons : MonoBehaviour
{

    void Awake()
    {

        // Get all Button components in the children of this GameObject
        Button[] buttons = GetComponentsInChildren<Button>();

        // Loop through each button and assign an OnClick listener
        foreach (Button button in buttons)
        {
            // Get the name of the button or some identifier to map it to a function
            string functionName = button.name.Replace("Button", "").Trim();

            // Dynamically assign the function from the target object
            button.onClick.AddListener(() =>
            {
                CallFunctionOnTarget(functionName);
            });
        }
    }

    private void CallFunctionOnTarget(string functionName)
    {
        var method = LevelLoader.Instance.GetComponent<MonoBehaviour>()?.GetType().GetMethod(functionName);

        if (method != null)
        {
            AudioManager.Instance.Play("Button");
            method.Invoke(LevelLoader.Instance.GetComponent<MonoBehaviour>(), null);
        }
        else
        {
            Debug.LogError($"Function {functionName} not found on target object!");
        }
    }
}
