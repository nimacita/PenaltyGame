using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{

    [Header("HairSkins")]
    public Mesh[] hairsSkins;
    public Mesh defaultHair;

    [Header("GlassSkins")]
    public Mesh[] glassSkins;
    private Mesh defaultGlass = null;

    [Header("BackPackSkins")]
    public Mesh[] backPackSkins;
    private Mesh defaultBackPack = null;
    public GameObject pants;

    [Header("Components")]
    public SkinnedMeshRenderer hairMesh;
    public SkinnedMeshRenderer glassMesh;
    public SkinnedMeshRenderer backPackMesh;

    private GameSettings gameSettings;

    void Start()
    {
        gameSettings = GameSettings.instance;

        DefineCurrentSkin();
    }

    public void DefineCurrentSkin()
    {
        DefineHairSkin();

        DefineGlassSkin();

        DefineBackPackSkin();
    }

    private void DefineHairSkin()
    {
        if (gameSettings.HairSkinId == 0)
        {
            //дефолтные
            hairMesh.sharedMesh = defaultHair;
        }
        else
        {
            hairMesh.sharedMesh = hairsSkins[gameSettings.HairSkinId - 1];
        }
    }

    private void DefineGlassSkin()
    {
        if (gameSettings.GlassSkinId == 0)
        {
            //дефолтные
            glassMesh.sharedMesh = defaultGlass;
        }
        else
        {
            glassMesh.sharedMesh = glassSkins[gameSettings.GlassSkinId - 1];
        }
    }

    private void DefineBackPackSkin()
    {
        if (gameSettings.BackpackSkinId == 0)
        {
            //дефолтные
            backPackMesh.sharedMesh = defaultBackPack;
        }
        else
        {
            if (gameSettings.BackpackSkinId == 2 || gameSettings.BackpackSkinId == 3)
            {
                pants.SetActive(false);
            }
            else
            {
                pants.SetActive(true);
            }
            backPackMesh.sharedMesh = backPackSkins[gameSettings.BackpackSkinId - 1];
        }
    }
}
