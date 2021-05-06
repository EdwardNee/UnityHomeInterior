using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

/// <summary>
/// Класс загрузки данных с Addressable на кнопки.
/// </summary>
public class LoadAssetsData : MonoBehaviour
{
    [SerializeField]
    private GameObject contentHandler;
    [SerializeField]
    private Button click;
    [SerializeField]
    private GameObject prefab;
    private bool isLoaded;

    [SerializeField]
    private string _label;
    private List<Element> loadingElements;

    public Text txt;

    private void Start()
    {
        loadingElements = new List<Element>();
        click.onClick.AddListener(LoadingAssets);
    }

    public async void LoadingAssets()
    {

        if (!isLoaded)
        {
            await GetDataAsync(_label);

            Element currElement;
            for (int i = 0; i < loadingElements.Count; i++)//items
            {
                //GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
                currElement = loadingElements[i];
                GameObject b = Instantiate(prefab, contentHandler.transform);
                b.GetComponent<ChooseObject>().chosedObj = currElement.ElementPrefab;//items[i];
                b.GetComponent<Image>().sprite = currElement.ElementSprite;
            }
            isLoaded = true;
        }
    }

    public async Task GetDataAsync(string label)
    {
        //Загружаем ссылки.
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach (var item in locations)
        {
            var element = await Addressables.LoadAssetAsync<Element>(item).Task;
            loadingElements.Add(element);   //items
        }
        txt.text = loadingElements.Count.ToString();
    }

}
