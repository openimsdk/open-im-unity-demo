using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public static class UIExtension 
{

    public static void SetParent(GameObject go){
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                go.transform.SetParent(Selection.activeTransform,false);
            }
        }
        else
        {
            Transform p = GameObject.FindObjectOfType<Canvas>().transform;
            if (p != null)
            {
                go.transform.SetParent(p,false);
            }
        }
    }

    [MenuItem("GameObject/UI/_Image", false, 2002)]
    static void AddImage()
    {
        GameObject go = new GameObject("Image", typeof(Image));
        var rect = go.GetComponent<RectTransform>();
        SetParent(go);
        go.GetComponent<Image>().raycastTarget = false;
        rect.localPosition = Vector3.zero;
        rect.localEulerAngles = Vector3.zero;
        rect.localScale = Vector3.one;
        rect.anchoredPosition3D = Vector3.zero;
        rect.anchoredPosition = Vector3.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/UI/_Button", false, 2030)]
    public static void AddButton()
    {
        GameObject go = new GameObject("Button", typeof(Image));
        RectTransform rect = go.transform as RectTransform;
        rect.sizeDelta = new Vector2(160, 30);

        Button button = go.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        button.targetGraphic = go.GetComponent<Image>();

        GameObject textObj = new GameObject("Text", typeof(Text));
        RectTransform textRect = textObj.transform as RectTransform;
        textRect.SetParent(rect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.offsetMax = Vector3.zero;
        textRect.offsetMin = Vector3.zero;
        textObj.GetComponent<Text>().text = "Button";
        textObj.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        textObj.GetComponent<Text>().color = Color.black;
        textObj.GetComponent<Text>().raycastTarget = false;

        SetParent(go); 
        Selection.activeObject = go;
    }
    [MenuItem("GameObject/UI/EmptyRayCaster", false, 2000)]
    public static void AddEmptyRayCaster(){
        GameObject go = new GameObject("EmptyRayCaster",typeof(Dawn.EmptyRaycast));
        SetParent(go);
        var rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localEulerAngles = Vector3.zero;
        rect.localScale = Vector3.one;
        rect.anchoredPosition3D = Vector3.zero;
        rect.anchoredPosition = Vector3.zero;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
