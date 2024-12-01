using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{

    enum SkinItem
    {
        hairSkin,
        glassSkin,
        backpackSkin
    }

    [Header("Item Settings")]
    [SerializeField] SkinItem skinItem;
    [Tooltip("Айди скина для сохранения")]
    [Range(1, 3)]
    [SerializeField] private int currentSkinId;

    [Header("Coin Item")]
    [SerializeField] private int coinPrice;

    [Header("To Item Image")]
    public Sprite itemIconSprite;
    [SerializeField] private string skinName;

    [Space]
    [Header("Components")]
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private TMPro.TMP_Text itemName;
    [SerializeField] private GameObject shopBtn;
    [SerializeField] private TMPro.TMP_Text shopTxt;
    [SerializeField] private TMPro.TMP_Text coinTxt;
    [SerializeField] private GameObject coinBg;
    [SerializeField] private GameObject equiped;
    [SerializeField] private ShopController shopController;

    void Start()
    {
        itemIcon.GetComponent<Image>().sprite = itemIconSprite;
        coinTxt.text = $"{coinPrice}";
        shopBtn.GetComponent<Button>().interactable = true;
        shopBtn.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //сохраненное значение куплен ли товар
    private bool IsSkinItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsSkinItemPurchased{currentSkinId}{skinItem}"))
            {
                if (PlayerPrefs.GetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}", 0);
                return false;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}{skinItem}", 0);
            }
        }
    }

    //определяем вид кнопки
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);
        itemName.text = skinName;

        //проверить куплено ли
        if (!IsSkinItemPurchased)
        {
            //если не куплен
            shopTxt.text = "Buy";
            equiped.SetActive(false);
            coinBg.SetActive(true);
            //проверить хватает ли денег
            if (GameSettings.instance.Coins < coinPrice)
            {
                shopBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                shopBtn.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            //если куплен
            equiped.SetActive(IsEquiped());
            coinBg.SetActive(false);
            shopTxt.text = "Select";
        }

    }

    //проверяем если скин экипирован
    private bool IsEquiped()
    {
        bool isEquiped = false;

        switch (skinItem)
        {
            case SkinItem.hairSkin:
                {
                    if (GameSettings.instance.HairSkinId == currentSkinId)
                    {
                        isEquiped = true;
                    }
                    break;
                }
            case SkinItem.glassSkin:
                {
                    if (GameSettings.instance.GlassSkinId == currentSkinId)
                    {
                        isEquiped = true;
                    }
                    break;
                }
            case SkinItem.backpackSkin:
                {
                    if (GameSettings.instance.BackpackSkinId == currentSkinId)
                    {
                        isEquiped = true;
                    }
                    break;
                }
        }
        return isEquiped;
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsSkinItemPurchased)
        {
            //если не куплено - покупаем
            if (GameSettings.instance.Coins < coinPrice)
            {
                //не можем купить

            }
            else
            {
                //можем купить
                GameSettings.instance.Coins -= coinPrice;
                SoundController.instance.PlayAddCoins();
                IsSkinItemPurchased = true;
                if (shopController != null) shopController.UpdateCurrency();
            }
        }
        else
        {
            //иначе по нажатию - экипируем
            EquipedSelectProduct();
        }
    }

    //экипируем или снимаем
    private void EquipedSelectProduct()
    {
        switch (skinItem)
        {
            case SkinItem.hairSkin:
                {
                    if (GameSettings.instance.HairSkinId == currentSkinId)
                    {
                        GameSettings.instance.HairSkinId = 0;
                    }
                    else
                    {
                        //экипируем
                        SoundController.instance.PlayEquipSound();
                        GameSettings.instance.HairSkinId = currentSkinId;
                    }
                    break;
                }
            case SkinItem.glassSkin:
                {
                    if (GameSettings.instance.GlassSkinId == currentSkinId)
                    {
                        GameSettings.instance.GlassSkinId = 0;
                    }
                    else
                    {
                        //экипируем
                        SoundController.instance.PlayEquipSound();
                        GameSettings.instance.GlassSkinId = currentSkinId;
                    }
                    break;
                }
            case SkinItem.backpackSkin:
                {
                    if (GameSettings.instance.BackpackSkinId == currentSkinId)
                    {
                        GameSettings.instance.BackpackSkinId = 0;
                    }
                    else
                    {
                        //экипируем
                        SoundController.instance.PlayEquipSound();
                        GameSettings.instance.BackpackSkinId = currentSkinId;
                    }
                    break;
                }
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
