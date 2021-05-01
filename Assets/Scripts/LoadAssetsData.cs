using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class LoadAssetsData : MonoBehaviour
{
    [SerializeField]
    private GameObject contentHandler;
    [SerializeField]
    private Button click;
    [SerializeField]
    private GameObject prefab;
    private bool isLoaded;

    private void Start()
    {
        click.onClick.AddListener(LoadingAssets);
    }
    public async void LoadingAssets()
    {

        if (!isLoaded)
        {
            await Get(_label);
            //for (int i = 0; i < 3; i++)
            //{
            //    GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
            //    // GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
            //    Debug.Log(contentHandler == null);
            //    GameObject b = Instantiate(prefab, contentHandler.transform);
            //    b.GetComponent<ChooseObject>().chosedObj = prefabCr;
            //    isLoaded = true;
            //}

            Element currElement;
            for (int i = 0; i < els.Count; i++)//items
            {
                //GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
                // GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
                //Debug.Log(contentHandler == null);
                currElement = els[i];
                GameObject b = Instantiate(prefab, contentHandler.transform);
                b.GetComponent<ChooseObject>().chosedObj = currElement.ElementPrefab;//items[i];
                b.GetComponent<Image>().sprite = currElement.ElementSprite;
            }
            isLoaded = true;
        }
    }

    public Text txt;
    [SerializeField]
    private string _label;
    private List<GameObject> items = new List<GameObject>();
    private List<Element> els = new List<Element>();
    public async Task Get(string label)
    {
        //Загружаем ссылки.
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach (var item in locations)
        {
            var element = await Addressables.LoadAssetAsync<Element>(item).Task;
            els.Add(element);   //items
        }
        txt.text = els.Count.ToString();
    }

}
