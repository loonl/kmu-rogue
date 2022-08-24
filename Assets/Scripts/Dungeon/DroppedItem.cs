using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Interact
{
    [SerializeField]
    private SpriteRenderer _renderer;

    private Item _item;
    public Item Item { get { return _item; } }

    public int _price = 0;
    // private Image _sprite;

    public void Set(Item item, int price = 0)
    {
        // 이미지 할당
        // Item 내에 sprite 이미지를 갖고 있으면 좋을듯
        //this._renderer.sprite = item.Image;

        this._price = price;
    }

    public override void InteractEvent()
    {
        if (this._price == 0)
        {
            // Get
            //GameManager.Instance.Player.Equip(this._item)
            // 만약 기존 장착한 아이템이 있으면
            // this.Set(unEquippedItem);
            // 없으면
            //Destroy(this.gameObject);
        }
        else
        {
            // Buy
            if (GameManager.Instance.Player.Inventory.Gold < this._price)
            {
                return;
            }

            // price를 0으로 바꾸어 Get으로 바뀌게 만듬
            GameManager.Instance.Player.Inventory.UpdateGold(-1 * this._price);
            this._price = 0;

            // !!! Sound (구매음 추가하기)
        }
    }
}
