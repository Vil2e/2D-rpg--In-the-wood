using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    [SerializeField] Sprite fullHeart, threeQuatersHeart, halfHeart, quaterHeart, emptyHeart;
    Image heartImage;

    // Start is called before the first frame update
    void Awake()
    {
        heartImage = GetComponent<Image>();

    }

    

    public enum HeartStatus
    {
        Empty = 0,
        Quater = 1,
        Half = 2,
        ThreeQuaters = 3,
        Full = 4
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;

            case HeartStatus.Quater:
                heartImage.sprite = quaterHeart;
                break;

            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;

            case HeartStatus.ThreeQuaters:
                heartImage.sprite = threeQuatersHeart;
                break;

            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;

                

        }
    }

}
