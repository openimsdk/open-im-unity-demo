using UnityEngine;

public partial class UIMain
{
    Transform channelRoot;

    void InitChannelUI()
    {
        channelRoot = transform.Find("content/channel");
    }
}