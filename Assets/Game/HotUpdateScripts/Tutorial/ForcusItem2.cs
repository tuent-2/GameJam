using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ForcusItem2 : MonoBehaviour
{
    [SerializeField] FocusItem focusItem;
    private void OnEnable()
    {
        StartCoroutine(TypeLine());
    }
    
    [SerializeField] private TextMeshProUGUI txtContent;
    [SerializeField] float textSpeed;
    private string[] lines;
    private int index;
    IEnumerator TypeLine()
    {
        lines = new string[]
        {
            "Điền Tên bạn vào đây",
            "Sau đó nhấn nút đăng nhập ở dưới để vào Edu Arena"
        };

        foreach (char c in lines[index].ToCharArray())
        {
            txtContent.text += c;
            yield return new WaitForSeconds(textSpeed / 2);
        }

        
        yield return new WaitForSeconds(0.5f);
        NextLine();
    }
    
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            txtContent.text += "\n";
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            focusItem.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.touchCount > 0|| Input.GetMouseButton(0))
        {
            gameObject.SetActive(false);
            focusItem.gameObject.SetActive(false);
        }
    }
}
