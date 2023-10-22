using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventVector3: UnityEvent<Vector3> 
{

}
public class MouseManager : MonoBehaviour
{
    RaycastHit hitinfo;
    public EventVector3 onMouseClicked;
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitinfo.collider != null)
        {
            if (hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                onMouseClicked?.Invoke(hitinfo.point);
            }
        }
    }
}
