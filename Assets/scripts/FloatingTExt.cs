using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The behaviour of the pop up text when ther ee is an attack colllision between the enemy and the player
/// </summary>
public class FloatingTExt : MonoBehaviour
{
    private float destroyTime=3f;
   
    private Vector3 randomiseIntensity = new Vector3(0.5f, 0f, 0);
    // Start is called before the first frame update
    void Start()
    {
      
        StartCoroutine(DestroyObject());

        transform.localPosition += new Vector3(Random.Range(-randomiseIntensity.x, randomiseIntensity.x),
            0.6f, Random.Range(-randomiseIntensity.z, randomiseIntensity.z));

    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
