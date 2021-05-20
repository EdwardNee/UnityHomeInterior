using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

/// <summary>
/// ����� �������� ������ � Addressable �� ������.
/// </summary>
public class LoadAssetsData : MonoBehaviour
{
    //����� �������� ������ ���������.
    [SerializeField]
    private GameObject contentHandler;
    //������, �� ������� ���������� �������� ���������.
    [SerializeField]
    private Button click;
    //������ ������ ��� ��������� ������.
    [SerializeField]
    private GameObject prefab;
    //��������� ��� ���.
    private bool isLoaded;

    private HintsScript hintsScript;

    //�����, �� ������� ����� ������ ��������.
    [SerializeField]
    private string _label;
    //��������� ����������� ���������.
    private List<Element> loadingElements;


    public Text txt;

    //Start is called before the first frame update
    private void Start()
    {
        hintsScript = HintsScript.Instance;
        loadingElements = new List<Element>();
        click.onClick.AddListener(LoadingAssets);
    }

    /// <summary>
    /// ����� �������� � ��������� �������.
    /// </summary>
    public async void LoadingAssets()
    {
        if (!isLoaded)
        {
            if (HintsScript.hintId == 2)
            {
                hintsScript.Off();
            }
            await GetDataAsync(_label);

            Element currElement;
            for (int i = 0; i < loadingElements.Count; i++)
            {
                currElement = loadingElements[i];
                GameObject b = Instantiate(prefab, contentHandler.transform);
                b.GetComponent<ChooseObject>().chosedObj = currElement.ElementPrefab;
                b.GetComponent<Image>().sprite = currElement.ElementSprite;
            }
            isLoaded = true;
            if (HintsScript.hintId == 2)
            {
                hintsScript.DisplayHint("����� �������� �������������� ������ � ���������� ������.\n" +
                    "�������� � �������� �� ������ � ������������ �������.", 2);
            }
        }
    }

    /// <summary>
    /// ����� ��������� ������ � Firebase Storage.
    /// </summary>
    /// <param name="label">����� ��� ����������� ������.</param>
    public async Task GetDataAsync(string label)
    {
        //��������� ������.
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach (var item in locations)
        {
            var element = await Addressables.LoadAssetAsync<Element>(item).Task;
            loadingElements.Add(element);   //items
        }
        //txt.text += $"loadded {loadingElements.Count}"; 
    }

}
