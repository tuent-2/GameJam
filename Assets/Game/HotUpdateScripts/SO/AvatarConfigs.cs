using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarConfigs", menuName = "Data/Avatar")]
public class AvatarConfigs : ScriptableObject
{
    public List<Avatar> avatarList;
    public readonly Dictionary<int, Avatar> avatarDictionary = new();

    private void OnEnable()
    {
        avatarDictionary.Clear();
        foreach (var gift in avatarList)
        {
            avatarDictionary[gift.AvatarId] = gift;
        }
    }

    public Avatar GetAvatarById(int giftId)
    {
        avatarDictionary.TryGetValue(giftId, out Avatar avatar);
        return avatar;
    }
}

[Serializable]
public class Avatar
{
    [SerializeField] private int avatarId;
    public int AvatarId => avatarId;

    [SerializeField] private Sprite avatarIcon;
    public Sprite AvatarIcon => avatarIcon;
}