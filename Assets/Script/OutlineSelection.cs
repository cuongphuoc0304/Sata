// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class OutlineSelection : MonoBehaviour
// {
//     private Transform highlight;
//     private Transform selection;
//     private RaycastHit raycastHit;

//     public Texture2D normalCursor;
//     public Texture2D hoverCursor;
//     public Texture2D clickCursor;
//     private Vector2 hotspot = Vector2.zero;
//     private bool isHovering = false;

    
//     void Update()
// {
//     // Reset highlight
//     if (highlight != null)
//     {
//         var outline = highlight.GetComponent<Outline>();
//         if (outline != null) outline.enabled = false;
//         highlight = null;
//     }

//     isHovering = false;

//     // Raycast
//     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//     if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
//     {
//         highlight = raycastHit.transform;
//         if ((highlight.CompareTag("Apple") || highlight.CompareTag("Bread") || highlight.CompareTag("Egg") || highlight.CompareTag("Water") || highlight.CompareTag("Milk")) && highlight != selection)
//         {
//             isHovering = true;

//             Outline outline = highlight.GetComponent<Outline>();
//             if (outline == null)
//             {
//                 outline = highlight.gameObject.AddComponent<Outline>();
//                 outline.OutlineWidth = 7.0f;
//             }
//             outline.OutlineColor = Color.white;
//             outline.enabled = true;
//         }
//         else
//         {
//             highlight = null;
//         }
//     }

    
//     if (Input.GetMouseButtonDown(0) && isHovering)
//     {
//         Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
//     }
//     else if (Input.GetMouseButtonUp(0))
//     {
//         Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
//     }
//     else if (isHovering)
//     {
//         Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
//     }
//     else
//     {
//         Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
//     }

//     // Chọn đối tượng
//     if (Input.GetMouseButtonDown(0))
//     {
//         if (highlight)
//         {
//             if (selection != null)
//                 selection.GetComponent<Outline>().enabled = false;

//             selection = raycastHit.transform;
//             selection.GetComponent<Outline>().enabled = true;
//             highlight = null;
//         }
//         else
//         {
//             if (selection)
//             {
//                 selection.GetComponent<Outline>().enabled = false;
//                 selection = null;
//             }
//         }
//     }

//     // Bỏ chọn khi thả chuột
//     else if (Input.GetMouseButtonUp(0))
//     {
//         if (selection != null)
//         {
//             selection.GetComponent<Outline>().enabled = false;
//             selection = null;
//         }
//     }
// }

// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    public Texture2D normalCursor;
    public Texture2D hoverCursor;
    public Texture2D clickCursor;
    private Vector2 hotspot = Vector2.zero;

    private bool isHovering = false;
    private bool lastHoveringState = false; // So sánh để chỉ đổi Cursor khi cần

    void Update()
    {
        Transform newHighlight = null;
        isHovering = false;

        // Raycast kiểm tra object
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            Transform target = raycastHit.transform;
            if ((target.CompareTag("Apple") || target.CompareTag("Bread") || target.CompareTag("Egg") || target.CompareTag("Water") || target.CompareTag("Milk")) && target != selection)
            {
                newHighlight = target;
                isHovering = true;
            }
        }

        // Nếu highlight thay đổi
        if (highlight != newHighlight)
        {
            // Tắt cái cũ
            if (highlight != null)
            {
                Outline oldOutline = highlight.GetComponent<Outline>();
                if (oldOutline != null) oldOutline.enabled = false;
            }

            highlight = newHighlight;

            // Bật cái mới
            if (highlight != null)
            {
                Outline outline = highlight.GetComponent<Outline>();
                if (outline == null)
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                    outline.OutlineWidth = 7.0f;
                }
                outline.OutlineColor = Color.white;
                outline.enabled = true;
            }
        }

        //  Chỉ đổi cursor nếu trạng thái hover thay đổi
        if (Input.GetMouseButtonDown(0) && isHovering)
        {
            Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
        }
        else if (isHovering != lastHoveringState)
        {
            Cursor.SetCursor(isHovering ? hoverCursor : normalCursor, hotspot, CursorMode.Auto);
        }

        lastHoveringState = isHovering;

        // Chọn object
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                    selection.GetComponent<Outline>().enabled = false;

                selection = raycastHit.transform;
                selection.GetComponent<Outline>().enabled = true;
                highlight = null; // reset để không xung đột highlight
            }
            else if (selection)
            {
                selection.GetComponent<Outline>().enabled = false;
                selection = null;
            }
        }

        // Bỏ chọn khi thả
        else if (Input.GetMouseButtonUp(0))
        {
            if (selection != null)
            {
                selection.GetComponent<Outline>().enabled = false;
                selection = null;
            }
        }
    }
}
