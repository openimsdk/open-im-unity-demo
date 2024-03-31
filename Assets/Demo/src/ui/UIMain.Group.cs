using UnityEngine;

public partial class UIMain
{
    Transform groupRoot;
    void InitGroupUI()
    {
        groupRoot = transform.Find("content/group");
    }
}