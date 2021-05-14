using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ��������� �� ������ ��� ������ �����. � ������ �� ������ ���� ������ �����,
/// �� ������� ��� ���������� � MaterialsColor ����� ����������.
/// </summary>
public class MaterialSetterButton : MonoBehaviour
{
    // ���� ���������� ���������.
    [SerializeField]
    public int materialId;
    //���� ������ ��� ���������.
    [SerializeField]
    private Color color;
    //�������� ���������.
    [SerializeField]
    private string text;
    //������ ���������.
    private Button btn;

    //������������� ��������.
    private static Material settingMaterial;
    //������ ������� ProgramManager.
    private ProgramManager programManagerScript;
    //������ ���� ����������.
    private Material[] materials;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static MaterialSetterButton Instance
    {
        get; private set;
    }

    /// <summary>
    /// �������� ��� �������������� ���������.
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
        //�������� ������, �� ������� ������, ����� �������� ���������� ��������.
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
    /// ������� ��������� ������� ��������� � ������ ������� colorChangedDel.
    /// </summary>
    private void SetMaterialFunc()
    {
        settingMaterial = materials[materialId];
        PrefabController.current_mat = materialId;
        Debug.Log(materialId);
        programManagerScript.colorChangedDel?.Invoke();
    }
}
