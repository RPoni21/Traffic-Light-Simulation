   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class Follower : MonoBehaviour
{
   [SerializeField] private PathCreator[] pathCreator;
   private float speed = 15;
   [SerializeField] private bool stop;
   [SerializeField] private float maxDistance;
   [SerializeField] private bool rightSide;
   private float saveSpeed;
   private float distanceTravelled;
   private PathCreator thisPath;
   private bool seen = false;
   private bool gone = true;
   private TrafficLoad trafficLoad;
   
   private float timeAlive = 0;


   void Start() {
      if (Random.Range(0,10) >= 5) {
         thisPath = pathCreator[0];
      } else{
       thisPath = pathCreator[1];
      }

      saveSpeed = speed;
   }
   void Update() {
      
    timeAlive += Time.deltaTime;
    distanceTravelled += speed * Time.deltaTime;

    
    transform.position = thisPath.path.GetPointAtDistance(distanceTravelled);
    transform.rotation = thisPath.path.GetRotationAtDistance(distanceTravelled);

    Ray ray = new Ray(transform.position, transform.forward);

    
      if (Physics.Raycast(ray, out RaycastHit hit, maxDistance)) {
         
         if(hit.collider.gameObject.layer == 9 || hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.layer == 7 || hit.collider.gameObject.layer == 14
            || hit.collider.gameObject.layer == 15) {

            IncreaseSpeed();

            if(hit.collider.gameObject.layer == 14 && !seen) {
               if(hit.collider.GetComponent<TrafficLoad>() != null) {
               trafficLoad = hit.collider.GetComponent<TrafficLoad>();
               

               trafficLoad.Increment();
               seen = true;
               gone = false;
               }
            }
         
            if(hit.collider.gameObject.layer == 15 && !gone) {
               if(trafficLoad != null) {
               trafficLoad.Decrement(timeAlive);
               gone = true;
               }
            }
         }
         else if (hit.collider.gameObject.layer == 6) {
            Destroy(gameObject);
         }
         else {

         if(hit.collider.gameObject.layer == 10 && rightSide) {
            IncreaseSpeed();
         } else if (hit.collider.gameObject.layer == 12 && rightSide)
          IncreaseSpeed();
         else {
          DecreaseSpeed();
          if(Vector3.Distance(transform.position, hit.collider.transform.position) < 10f) {
            speed = 0;
          }
         }

       }
    }
         else {
            IncreaseSpeed();
         }
   
    }

    public void IncreaseSpeed() {
      speed += 0.4f * saveSpeed *Time.deltaTime*Time.timeScale;
            if (speed > saveSpeed) {
            speed = saveSpeed;
         }
    }

    public void DecreaseSpeed() {
      speed -= saveSpeed*0.1f*Time.timeScale;
         if (speed < 0) {speed = 0;}
    }
    

    private void OnTriggerEnter(Collider other) {
      Debug.Log(other.name);
    }
   }

