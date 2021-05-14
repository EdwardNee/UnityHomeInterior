using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ������ ������ Material ��� ��������� ����� �������.
/// </summary>
public class MaterialsColor : MonoBehaviour
{
    //������ ����������.
    [SerializeField]
    private Material[] materials;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static MaterialsColor Instance
    {
        get; private set;
    }

    /// <summary>
    /// �������� ��� ��������� ����� �������.
    /// </summary>
    public Material[] Materials
    {
        get
        {
            return materials;
        }
    }

    /// <summary>
    /// ���������� ��� ��������� Material ����������.
    /// </summary>
    public Material this[int index]
    {
        get
        {
            return materials[index];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Debug.Log(materials.Length);
    }
}
