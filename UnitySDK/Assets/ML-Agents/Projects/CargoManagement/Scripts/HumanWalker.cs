/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWalker : MonoBehaviour {
     
     public float speed;
     public float area;
     private Vector2 newWayPoint;
     private Vector3 wayPoint;
     private Vector3 oldWayPoint;
     public float timeSmooth;
     private float time;
     private CharacterController controller;
     // Use this for initialization
     void Start () {
         newWayPoint = Random.insideUnitCircle * area;
         wayPoint = new Vector3(newWayPoint.x, transform.position.y, newWayPoint.y);
         controller = GetComponent<CharacterController>();
         oldWayPoint = wayPoint;
     }
     
     // Update is called once per frame
     void Update () {        
         
         MoveRandomly();
         
     }
     
     void MoveRandomly(){
         
         Vector3 smoothLookAt = Vector3.Slerp(oldWayPoint, wayPoint, time/timeSmooth);
         time += Time.deltaTime;
         smoothLookAt.y = wayPoint.y;
         
         if(Vector3.Distance(transform.position, wayPoint)>20.0f && time/timeSmooth < 1.0f){
             transform.LookAt(smoothLookAt);
             controller.Move(transform.forward * speed);
         }
         else {
             newWayPoint = Random.insideUnitCircle * area;
             oldWayPoint = wayPoint;
             wayPoint = new Vector3(newWayPoint.x, wayPoint.y, newWayPoint.y);
             transform.LookAt(smoothLookAt);
             controller.Move(transform.forward * speed);
             time = 0;
         }
     }
     
     void OnGUI() {
         //GUI.Label(new Rect(Screen.width-400, 0, 400, 200), "" + oldWayPoint.ToString() + " | " + wayPoint.ToString() + " | " + time/timeSmooth);
     }
 }*/



using UnityEngine;
using System.Collections;
 
public class HumanWalker : MonoBehaviour
{
   Vector3 dest;
public float range = 10f;
private float speed = 0.5f;
Vector3 x;
void Start() {
   x = transform.position;
  dest = x + new Vector3(Random.Range(-range, range), 1.8f, Random.Range(-range, range));
}

void Update() {
  float step = speed * Time.deltaTime;
  transform.position = Vector3.MoveTowards(transform.position, dest, step);
  if(Vector3.Distance(transform.position, dest) < 1f) {
    dest  = x + new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
  }
  
}
 
    
}

