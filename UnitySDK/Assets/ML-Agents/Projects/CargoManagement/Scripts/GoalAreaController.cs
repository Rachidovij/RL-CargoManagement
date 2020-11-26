using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoalAreaController : MonoBehaviour
{
	public string CargoTag; 
	[SerializeField] private List<GameObject> placementAreas;
	[SerializeField] private SceneController SceneController;
    
	public class CargoItem
	{
		public Transform place;	
		public bool isPlaced;

	}

	public List<CargoItem> CargoTruck = new List<CargoItem>();
	public bool isFull;



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject placementArea in placementAreas ) {
        	CargoTruck.Add(new CargoItem{ place = placementArea.transform, isPlaced = false});
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i< placementAreas.Count; i++) {

        	placementAreas[i].GetComponent<MeshRenderer>().enabled = CargoTruck[i].isPlaced;
        }
        if (CargoTruck.All(CargoItem => CargoItem.isPlaced))
        {
        	isFull = true;

        }


    }

    public void resetCargoTruck() {

    	foreach(CargoItem item in CargoTruck) {
    		item.isPlaced = false;
    	}
    	isFull =false;
    }

    public void addCargo(GameObject Cargo) {

    	foreach(CargoItem item in CargoTruck) {

    		if (item.isPlaced == false){
    			item.isPlaced = true;
    			Destroy(Cargo);
    			SceneController.spawnedCargoes.Remove(Cargo);
    			break;
    		}
    	}
    }
}
