using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using open_im_sdk;
using UnityEngine.SocialPlatforms;

namespace Dawn.Game.UI
{
    public class UICreateGroup : UGuiForm
    {
        Button backBtn;
        TMP_InputField groupName;
        TMP_InputField groupIntroduction;
        Button createBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            groupName = GetInputField("Panel/content/center/name/input");
            groupIntroduction = GetInputField("Panel/content/center/introduction/input");
            createBtn = GetButton("Panel/content/center/create");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(createBtn, () =>
            {
                if (groupName.text == "")
                {
                    return;
                }
                var userId = IMSDK.GetLoginUser();
                IMSDK.CreateGroup((GroupInfo, err, errMsg) =>
                {
                    if (GroupInfo != null)
                    {
                        Debug.Log(GroupInfo);
                    }
                    else
                    {
                        Debug.LogError(errMsg);
                    }
                }, new CreateGroupReq()
                {
                    OwnerUserID = userId,
                    MemberUserIDs = new string[] { },
                    AdminUserIDs = new string[] { userId },
                    GroupInfo = new GroupInfo()
                    {
                        GroupName = groupName.text,
                        Introduction = groupIntroduction.text,
                        GroupType = (int)GroupType.Group,
                    }
                });
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

