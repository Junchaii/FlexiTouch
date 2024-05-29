using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowContoller : MonoBehaviour
{
   public GameObject Rock;
   float time = 0f;
   
   public float destroyTime = 3f;
   private float elapsedTime = 0.0f; // 已過的時間
   // Use this for initialization
   void Start() {

   }

   // Update is called once per frame
   void Update() {
       time += Time.deltaTime;
       if (Input.GetKeyUp(KeyCode.Space)) {
           if (time > 0.5f) {
               Instantiate(Rock, this.transform.position, this.transform.rotation);
               time = 0;
           }

       }
   }
}
