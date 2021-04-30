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

            for (int i = 0; i < items.Count; i++)
            {
                //GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
                // GameObject prefabCr = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Furniture/{i}.prefab", typeof(GameObject)) as GameObject;
                //Debug.Log(contentHandler == null);
                GameObject b = Instantiate(prefab, contentHandler.transform);
                b.GetComponent<ChooseObject>().chosedObj = items[i];
                isLoaded = true;
            }
        }
    }

    public Text txt;

    [SerializeField]
    private string _label;
    private List<GameObject> items = new List<GameObject>();
    public async Task Get(string label)
    {
        //Загружаем ссылки.
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach (var item in locations)
        {
            var element = await Addressables.LoadAssetAsync<GameObject>(item).Task;
            items.Add(element);
        }
        txt.text = items.Count.ToString();
    }
    
}
