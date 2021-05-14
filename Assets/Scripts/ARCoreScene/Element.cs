using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� ��� �������� ������� ������ � ������� ������.
/// </summary>
[CreateAssetMenu(fileName = "Element1", menuName = "Add my custom Element/Element")]
public class Element : ScriptableObject
{
    //������ ������.
    [SerializeField]
    private GameObject elementPrefab;
    //������-������ ������.
    [SerializeField]
    private Sprite elementSprite;

    /// <summary>
    /// �������� ��� ��������� � ���������� �������.
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
    /// �������� ��� ��������� � ���������� ������.
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
