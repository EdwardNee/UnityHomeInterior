using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс хранит массив Material для изменения цвета объекта.
/// </summary>
public class MaterialsColor : MonoBehaviour
{
    //Массив материалов.
    [SerializeField]
    private Material[] materials;

    /// <summary>
    /// Конструктор объекта класса.
    /// </summary>
    public static MaterialsColor Instance
    {
        get; private set;
    }

    /// <summary>
    /// Свойство для получения всего массива.
    /// </summary>
    public Material[] Materials
    {
        get
        {
            return materials;
        }
    }

    /// <summary>
    /// Индексатор для получения Material поиндексно.
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
