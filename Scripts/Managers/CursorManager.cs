using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;
    private Vector2 cursorHotspot;
    void Awake()
    {
        cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);   
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }
    private void OnDestroy() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
