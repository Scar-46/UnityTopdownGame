using UnityEngine;
using System.Collections.Generic;

public class AfterImagePool : MonoBehaviour
{
    public GameObject afterImagePrefab;
    public int poolSize = 10;
    public float lifeTime = 0.5f;
    public float spawnRate = 0.05f;

    private List<GameObject> pool;

    void Awake()
    {
        // Initialize pool
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(afterImagePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public void SpawnAfterImage()
    {
        GameObject obj = GetPooledObject();
        if (obj == null) return;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        obj.SetActive(true);

        // Start fading coroutine
        StartCoroutine(FadeAndShrink(obj, sr));
    }

    GameObject GetPooledObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null; // all objects in use
    }

    System.Collections.IEnumerator FadeAndShrink(GameObject obj, SpriteRenderer sr)
    {
        float elapsed = 0f;
        Vector3 startScale = obj.transform.localScale;
        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lifeTime;

            // Fade out
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(0.5f, 0f, t));

            // Shrink
            obj.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }
        obj.SetActive(false);
        obj.transform.localScale = startScale; // reset
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); // reset
    }
}
