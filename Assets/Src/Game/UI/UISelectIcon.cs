using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using UnityEngine.U2D;

namespace Dawn.Game.UI
{
    public delegate void OnSelectHeadIcon(string url);
    public class UISelectIcon : UGuiForm
    {
        class Item
        {
            public Image Image;
            public Button Btn;
        }

        public string HeadIconSpriteAltas;
        SpriteAtlas headIconSA;
        LoopGridView list;
        Sprite[] headSprites;
        OnSelectHeadIcon onSelectHeadIcon;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            list = GetGridView("Panel/content/list");
            list.InitGridView(0, (list, index, row, col) =>
            {
                if (index < 0)
                {
                    return null;
                }
                var itemNode = list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new Item()
                    {
                        Image = GetImage("icon", parent),
                        Btn = GetButton("", parent),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                var item = itemNode.UserObjectData as Item;
                item.Image.sprite = headSprites[index];
                OnClick(item.Btn, () =>
                {
                    if (onSelectHeadIcon != null)
                    {
                        string url = "headicon/" + headSprites[index].name;
                        onSelectHeadIcon(url);
                    }
                    CloseSelf();
                });
                return itemNode;
            });

            headIconSA = GameEntry.SpriteAltas.GetSpriteAtlas(HeadIconSpriteAltas);
            if (headIconSA == null)
            {
                Debug.LogError("not find SpriteAltas:" + HeadIconSpriteAltas);
                headSprites = new Sprite[0];
            }
            else
            {
                headSprites = new Sprite[headIconSA.spriteCount];
                headIconSA.GetSprites(headSprites);
                foreach (var sprite in headSprites)
                {
                    sprite.name = sprite.name.Replace("(Clone)", "");
                }
            }
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            list.SetListItemCount(headSprites.Length);

            if (userData is OnSelectHeadIcon)
            {
                onSelectHeadIcon = userData as OnSelectHeadIcon;
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

    }
}

