using UnityEngine;

public static class RandomExtension
{
    public static Vector2 OnUnitCircle()
    {
        var randomPos = Random.insideUnitCircle;
        if (randomPos == Vector2.zero)
        {
            randomPos = Vector2.one;
        }

        return randomPos.normalized;
    }
}