using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseManager : MonoBehaviour
{
    RaycastHit hitinfo;
    public event Action<Vector3> MouseEventClick;
    public static MouseManager Instance = null;

    public Texture2D point, doorway, attack, target, arrow;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitinfo) )
        {
            // change texture of mouse
            switch(hitinfo.collider.gameObject.tag)
            {
                case "Ground" :
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitinfo.collider != null)
        {
            if (hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                MouseEventClick?.Invoke(hitinfo.point);
            }
        }
    }
}
