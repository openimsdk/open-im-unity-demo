using UnityEngine;

public partial class UIMain
{
    Transform worldRoot;

    void InitWorldUI()
    {
        worldRoot = transform.Find("content/world");
    }
}