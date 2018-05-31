using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Texture2D texture;
    Routine routine;
    public float restriction = 10;
    int i = 0;

    private void Awake()
    {

    }

    IEnumerator Starting()
    {
        print("Starting");
        yield return null;
    }

    void CalculateTexture()
    {
        for (int j = 0; j < 512; j++)
        {
            texture.SetPixel(i, j, Color.red* Perlin.Noise(i,j));
        }
        texture.Apply();
        i++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //routine.Start();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //routine.Reset();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //routine.Stop();
        }
    }
}
