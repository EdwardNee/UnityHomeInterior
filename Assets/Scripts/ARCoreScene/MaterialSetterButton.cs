using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSetterButton : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private Color color;
    [SerializeField]
    private string text;
    private Button btn;
    private static Material settingMaterial;
    private ProgramManager programManagerScript;

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
        programManagerScript = FindObjectOfType<ProgramManager>();

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

    private void SetMaterialFunc()
    {
        settingMaterial = material;
        Debug.Log(Material);
        programManagerScript.colorChangedDel?.Invoke();
    }
}