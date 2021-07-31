using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class NumberShower : MonoBehaviour
{
    public Sprite[] numberSprites;
    public Image imgPrefabs;
    private List<Image> imgList = new List<Image>();
    private Image[] imgArr;
    private Vector3 oriPos;
    // Start is called before the first frame update
    void Awake()
    {
        oriPos = new Vector3(430, 115, 0);
        imgArr = GetComponentsInChildren<Image>();
        DisplayScore(0);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    int value = Random.Range(0, 200);
        //    DisplayScore(value);
        //    Debug.Log(value);
        //}
    }

    public void DisplayScore(int score)
    {
        int i = 0;
        int temp = (score / (int)Mathf.Pow(10, i));
        while (temp > 0)
        {
            if (i >= imgArr.Length)
            {
                CreateImgObj();
            }
            int singleNumber = temp % 10;
            Image image = imgArr[i];

            SetScoreImg(singleNumber, i);

            if (image.enabled == false)
            {
                imgArr[i].enabled = true;
            }
            temp = (score / (int)Mathf.Pow(10, ++i));
        }

        while (i < imgArr.Length && imgArr[i] != null && imgArr[i].enabled)
        {
            //imgArr[i].enabled = false;
            SetScoreImg(0, i);
            i++;
        }
    }

    void CreateImgObj()
    {
        var newImg = Instantiate(imgPrefabs, this.transform);
        foreach (Image img in imgArr)
        {
            img.GetComponent<RectTransform>().localPosition += Vector3.right * numberSprites[0].rect.width;
        }
        newImg.GetComponent<RectTransform>().localPosition = Vector3.zero;
        imgList.Add(newImg);
        imgArr = imgList.ToArray();
    }

    void SetScoreImg(int singleNumber, int index)
    {
        Image img = imgArr[index];
        if (img != null)
        {
            img.sprite = numberSprites[singleNumber];
        }

        float dis = 0;
        for (int i = index - 1; i >= 1 && i < imgArr.Length; i--)
        {
            dis += imgArr[i].sprite.rect.width;
        }
        if (index > 0)
        {
            dis += img.sprite.rect.width / 2 + imgArr[0].sprite.rect.width / 2;
        }
        dis *= img.GetComponent<RectTransform>().localScale.x;
        //img.transform.localPosition = Vector3.zero - (Vector3.right * dis);
        img.color = Color.white;
        img.SetNativeSize();
    }

}
