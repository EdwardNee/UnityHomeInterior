using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт находится на кнопке для выбора цвета. У каждой из кнопок есть индекс цвета,
/// по которой она обращается к MaterialsColor через индексатор.
/// </summary>
public class MaterialSetterButton : MonoBehaviour
{
    // Айди выбранного материала.
    [SerializeField]
    public int materialId;
    //Цвет кнопки для материала.
    [SerializeField]
    private Color color;
    //Название материала.
    [SerializeField]
    private string text;
    //Кнопка материала.
    private Button btn;

    //Установленный материал.
    private static Material settingMaterial;
    //Объект скрипта ProgramManager.
    private ProgramManager programManagerScript;
    //Массив всех материалов.
    private Material[] materials;

    /// <summary>
    /// Конструктор объекта класса.
    /// </summary>
    public static MaterialSetterButton Instance
    {
        get; private set;
    }

    /// <summary>
    /// Свойство для установленного материала.
    /// </summary>
    public static Material Material
    {
        get
        {
            return settingMaterial;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        programManagerScript = FindObjectOfType<ProgramManager>();
        MaterialsColor materialsColorScript = MaterialsColor.Instance;
        Debug.Log(materialsColorScript == null);
        materials = materialsColorScript.Materials;
        //Получаем кнопку, на которой скрипт, далее изменяем переданные свойства.
        btn = GetComponent<Button>();
        btn.GetComponentInChildren<Text>().text = text;
        btn.GetComponent<Image>().color = color;
        btn.onClick.AddListener(SetMaterialFunc);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Функция установки индекса материала и вызова события colorChangedDel.
    /// </summary>
    private void SetMaterialFunc()
    {
        settingMaterial = materials[materialId];
        PrefabController.current_mat = materialId;
        Debug.Log(materialId);
        programManagerScript.colorChangedDel?.Invoke();
    }
}
