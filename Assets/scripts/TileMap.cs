using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D rb;

        if (!collision.gameObject.TryGetComponent(out rb)) return;
        //計算入射方向
        Vector2 dir = new Vector2(2, 1).normalized;
        //計算法線方向
        Vector2 normal = collision.contacts[0].normal;
        //計算反射方向
        Vector2 newDir = normal * -1;
        rb.AddForce(newDir * 2, ForceMode2D.Impulse);



    }
}
