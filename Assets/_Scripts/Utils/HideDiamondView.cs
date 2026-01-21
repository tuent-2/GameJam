using System.Collections.Generic;
using UnityEngine;

public class HideDiamondView : MonoBehaviour
{
    [SerializeField] private List<GameObject> showGos;
    [SerializeField] private List<GameObject> hideGos;

    private void OnEnable()
    {
        // if (UserModel.Instance.IsUseDiamond) return;
        // foreach (var go in showGos)
        // {
        //     go.Show();
        // }
        //
        // foreach (var go in hideGos)
        // {
        //     go.Hide();
        // }
    }
}