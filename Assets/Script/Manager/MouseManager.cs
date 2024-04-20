using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseManager : MonoBehaviour
{
    RaycastHit hitinfo;
    public event Action<Vector3> MouseEventClickGround;
    public event Action<GameObject> MouseEventClickEnemy;
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
        MouseControl();
        
    }

    private void FixedUpdate()
    {
        SetCursorTexture();
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
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
        if (hitinfo.collider == null)
        {
            // Debug.Log("hisNothing");
        }
    }

    void MouseControl()
    {
        var isMouseClick = Input.GetMouseButtonDown(0);
        if (isMouseClick && hitinfo.collider != null)
        {
            if (hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                MouseEventClickGround?.Invoke(hitinfo.point);
            } 
            if (hitinfo.collider.gameObject.CompareTag("Enemy"))
            {
                MouseEventClickEnemy?.Invoke(hitinfo.collider.gameObject);
            }
        }
    }
}
