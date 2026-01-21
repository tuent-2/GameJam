using UnityEngine;

public static class CameraHelper
{
    private static Camera _camera;

    public static Camera GameCamera
    {
        get
        {
            if (!_camera) _camera = Camera.main;
            return _camera;
        }
    }

    private static Vector2 _leftBottomWorldPoint = Vector2.zero, _rightUpWorldPoint = Vector2.zero;

    public static Vector2 LeftBottomWorldPoint
    {
        get
        {
            if (_leftBottomWorldPoint == Vector2.zero)
            {
                _leftBottomWorldPoint = ScreenToWorldPoint(new Vector2(0, 0));
            }

            return _leftBottomWorldPoint;
        }
    }

    public static Vector2 RightUpWorldPoint
    {
        get
        {
            if (_rightUpWorldPoint == Vector2.zero)
            {
                _rightUpWorldPoint = ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            }

            return _rightUpWorldPoint;
        }
    }

    public static void UpdateCamera(Camera camera)
    {
        _camera = camera;
    }

    public static bool IsVisible(Vector3 position)
    {
        Vector3 screenPoint = WorldToViewportPoint(position);
        return screenPoint.x is >= 0 and <= 1 && screenPoint.y is >= 0 and <= 1;
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, GameCamera, out var result);
        return result;
    }

    public static Vector3 ScreenToWorldPoint(Vector3 position)
    {
        return GameCamera.ScreenToWorldPoint(position);
    }

    public static Vector3 WorldToScreenPoint(Vector3 position)
    {
        return GameCamera.WorldToScreenPoint(position);
    }

    public static Vector2 WorldToViewportPoint(Vector3 position)
    {
        return GameCamera.WorldToViewportPoint(position);
    }

    public static void SetOrthographicSize(float size)
    {
        GameCamera.orthographicSize = size;
    }

    public static Ray ScreenPointToRay(Vector3 pos)
    {
        return GameCamera.ScreenPointToRay(pos);
    }
}