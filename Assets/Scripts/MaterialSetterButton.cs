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
    private Material settingMaterial;
    private ProgramManager programManagerScript;

    public Material Material
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

    private void SetMaterialFunc()
    {
        settingMaterial = material;
        DialogColorChange dc = FindObjectOfType<DialogColorChange>();
        dc.Close();
        programManagerScript.colorChangedDel?.Invoke();
    }
}
