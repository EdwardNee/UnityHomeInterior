using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element1", menuName = "Add my custom Element/Element")]
public class Element : ScriptableObject
{
    [SerializeField]
    private GameObject elementPrefab;
    [SerializeField]
    private Sprite elementSprite;

    public GameObject ElementPrefab
    {
        get
        {
            return elementPrefab;
        }
        set
        {
            elementPrefab = value;
        }
    }

    public Sprite ElementSprite
    {
        get
        {
            return elementSprite;
        }
        set
        {
            elementSprite = value;
        }
    }
}
