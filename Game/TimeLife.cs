using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLife : MonoBehaviour
{
    public float Timelife ;
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= Timelife)
        {
            if (board.instance.PoolQueue.ContainsKey(this.gameObject.name)) // neu trong pool co chua item nay thi se cho quay tro lai pool
            {
               
                timer = 0;
                board.instance.SetObjectPool(this.gameObject);
            }
            else // xoa neu trong pool khong chua item nay
            {
                Destroy(gameObject);
            }
        }
    }
}
