using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Элемент для хранения префаба мебели и спрайта иконки.
/// </summary>
[CreateAssetMenu(fileName = "Element1", menuName = "Add my custom Element/Element")]
public class Element : ScriptableObject
{
    //Префаб мебели.
    [SerializeField]
    private GameObject elementPrefab;
    //Спрайт-иконка кнопки.
    [SerializeField]
    private Sprite elementSprite;

    /// <summary>
    /// Свойство для установки и считывания префаба.
    /// </summary>
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

    /// <summary>
    /// Свойство для установки и считывания иконки.
    /// </summary>
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
