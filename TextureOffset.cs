using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffset : MonoBehaviour
{
    [SerializeField] private float x_offset_speed;
    [SerializeField] private float y_offset_speed;
    private float x_offset_cur;
    private float y_offset_cur;
    
    private MeshRenderer mesh_renderer;
    

    private void Awake()
    {
        mesh_renderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        x_offset_cur += Time.deltaTime * x_offset_speed;
        y_offset_cur += Time.deltaTime * y_offset_speed;

        mesh_renderer.material.mainTextureOffset = new Vector3(x_offset_cur, y_offset_cur,0f);
    }
}