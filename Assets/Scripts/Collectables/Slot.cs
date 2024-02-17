using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot : MonoBehaviour
{
    public bool isOccupied = false;
    private GameObject currentImage;

    public void setItem(GameObject image)
    {
        if (!isOccupied)
        {
            isOccupied = true;
            currentImage = Instantiate(image, gameObject.transform);
        }
        else
        {
            currentImage.GetComponent<Item>().drop = true;
            Destroy(currentImage);
            currentImage = Instantiate(image, gameObject.transform);
        }
    }
}
