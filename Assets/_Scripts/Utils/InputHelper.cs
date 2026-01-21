using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHelper
{
    private static InputMaster _input;

    public static InputMaster Input
    {
        get
        {
            if (_input != null) return _input;
            _input = new InputMaster();
            _input.Enable();
            return _input;
        }
    }

    public static Vector2 PressPosition => (GameUtils.IsAndroid() || GameUtils.IsIOS())
        ? Touchscreen.current.primaryTouch.position.ReadValue()
        : Mouse.current.position.ReadValue();
    
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {    
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == 5)
                return true;
        }

        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = PressPosition
        };
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}