using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    float secToWait = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().enabled = false;
            collision.GetComponent<Animator>().SetBool("isWalking", false);
            SFXManager.instance.StairsDown();
            StartCoroutine("GoToNextLevel");
        }
    }

    //等待幾秒再載入下一關
    IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(secToWait);
        gameManager.LoadNextScene();

    }
}
