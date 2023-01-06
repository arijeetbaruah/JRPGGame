using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverheadPopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textArea;

    public void ShowText(string text)
    {
        textArea.SetText(text);
        transform.localPosition = Vector3.zero;
        transform.DOMoveY(transform.position.y + 5, 3).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
