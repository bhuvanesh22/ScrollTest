using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public ScrollRect scrollView;       
    public RectTransform content;       
    public ObjectPooler pooler;         
    public int totalImages;       
    public float itemHeight;     

    private List<GameObject> activeItems = new List<GameObject>();

    void Start()
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalImages * itemHeight);
        UpdateVisibleItems();
    }

    void Update() => UpdateVisibleItems();

    void UpdateVisibleItems()
    {
        float scrollPosition = content.anchoredPosition.y;
        int firstVisibleIndex = Mathf.FloorToInt(scrollPosition / itemHeight),
            lastVisibleIndex = Mathf.Min(totalImages, Mathf.CeilToInt((scrollPosition + scrollView.viewport.rect.height) / itemHeight)) - 1;

        firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, totalImages - 1);
        lastVisibleIndex = Mathf.Clamp(lastVisibleIndex, 0, totalImages - 1);

        List<GameObject> itemsToRemove = new();

        for (int i = activeItems.Count - 1; i >= 0; i--)
        {
            GameObject item = activeItems[i];
            int itemIndex = item.GetComponent<ImageItem>().index;

            if (itemIndex < firstVisibleIndex || itemIndex > lastVisibleIndex)
                itemsToRemove.Add(item);
        }

        itemsToRemove.ForEach(x => { pooler.ReturnItem(x); activeItems.Remove(x); });

        for (int i = firstVisibleIndex; i <= lastVisibleIndex; i++)
        {
            if (!activeItems.Exists(x => x.GetComponent<ImageItem>().index == i))
            {
                GameObject newItem = pooler.GetItem(); 
                SetupItem(newItem, i);                
                activeItems.Add(newItem);             
            }
        }
    }

    void SetupItem(GameObject item, int index)
    {
        item.transform.SetParent(content);
        item.transform.localScale = Vector3.one;

        RectTransform itemRect = item.GetComponent<RectTransform>();
        itemRect.anchoredPosition = new Vector2(0, -index * itemHeight);

        item.GetComponent<ImageItem>().index = index;
        item.GetComponent<ImageItem>().SetText($"Image {index + 1}");

        StartCoroutine(LoadImageAsync(index, item.GetComponent<ImageItem>()));
    }

    IEnumerator LoadImageAsync(int index, ImageItem item)
    {
        string imagePath = $"Images/img_{index}";  
        ResourceRequest loadRequest = Resources.LoadAsync<Sprite>(imagePath);
        yield return loadRequest;

        if (loadRequest.asset != null) item.SetImage(loadRequest.asset as Sprite);
        else Debug.LogWarning($"Image {imagePath} not found.");
    }
}
