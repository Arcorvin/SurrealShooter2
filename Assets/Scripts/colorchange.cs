using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorchange : MonoBehaviour
{
    // Public
    public GameObject cubePrefab;
    public int numberOfObjects = 20;
    public float radius = 5f;
    public float changeColorDelay = 2f;
    public GameObject cameraPivot;
    // Private
    private Renderer[] cubesRenderers;
    private float timer;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        cubesRenderers = new Renderer[numberOfObjects];
        GameObject obj;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;

            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            obj = Instantiate(cubePrefab, pos, Quaternion.identity);
            cubesRenderers[i] = obj.GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float[] Spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.Hamming);
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 previousScale = cubesRenderers[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, Spectrum[i] * 40, Time.deltaTime * 30);
            cubesRenderers[i].transform.localScale = previousScale;
        }
        cameraPivot.transform.Rotate(0, 0.5F, 0);

        DelayedRandomMaterialColor();
    }

    void DelayedRandomMaterialColor()
    {
        timer += Time.deltaTime;
        if (timer > changeColorDelay)
        {
            // Random Color 100% saturated and above 50% brightness(value)
            var randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            RendererColor(cubesRenderers, randomColor);
            timer = 0f;
        }
    }

    void RendererColor(Renderer[] renderers, Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            cubesRenderers[i].material.color = color;
        }
    }
}
