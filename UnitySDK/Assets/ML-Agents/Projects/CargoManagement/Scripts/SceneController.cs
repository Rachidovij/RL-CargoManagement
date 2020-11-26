using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneController : MonoBehaviour
{

	public class SpawnLocation {

		public bool used;
		public Transform transform;
		public LandOwner landOwner;

	}

	public enum LandOwner {
		None,
		CargoArea
	}

	[SerializeField] private List<GameObject> CargoAreas;
	[SerializeField] private List<GameObject> Cargoes;
    [SerializeField] private List<GameObject> CargoToSpawn;
    public List<GameObject> spawnedCargoes;
	[SerializeField] private GameObject spawnPoints;
	[SerializeField] private List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
	[SerializeField] private TagMaster agent;



    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform spawnpoint in spawnPoints.transform) {
        	spawnLocations.Add(new SpawnLocation {used = false, transform=spawnpoint, landOwner=LandOwner.None});
        }
        
        foreach(GameObject Cargo in Cargoes) {

        	CargoToSpawn.Add(Cargo);
        	
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (spawnedCargoes.Count <=1){
        	SpawnCargo();
        	
        }


    }



    private void SpawnCargo() {

    	
    	removeCargoToSpawnIfAreaIsFull();
    	for (; ; ) {

    		if (spawnLocations.Count <=0 || CargoToSpawn.Count <= 0) {
    			break;}

    		int randomSpawnPoint = Random.Range(0, spawnLocations.Count);
    		int randomCargo = Random.Range(0, CargoToSpawn.Count);
    		if (spawnLocations[randomSpawnPoint].used== false && spawnLocations[randomSpawnPoint].landOwner== LandOwner.None) {
    			GameObject Cargo =Instantiate(CargoToSpawn[randomCargo], spawnLocations[randomSpawnPoint].transform.position, Quaternion.identity );
    			spawnedCargoes.Add(Cargo);
    			EmptyAllSpawnPointsExceptCargoareas();
    			spawnLocations[randomSpawnPoint].used =true;
    			break;

    		}
    		
    	}
    }

    void removeCargoToSpawnIfAreaIsFull() {

    	foreach(GameObject CargoArea in CargoAreas) {
    		GoalAreaController CargoTruck = CargoArea.GetComponent<GoalAreaController>();
    		if (CargoTruck.isFull) {
                
    			foreach(GameObject Cargo in CargoToSpawn.ToList())  {

    				if (Cargo.gameObject.name == CargoTruck.CargoTag) {
    					CargoToSpawn.Remove(Cargo);

    				}  } 
    		}
    	}

    }

    void EmptyAllSpawnPointsExceptCargoareas() {

    	foreach (SpawnLocation spawnLocation in spawnLocations ) 
    	{
    		if (spawnLocation.landOwner != LandOwner.CargoArea) {

    			spawnLocation.landOwner = LandOwner.None;
    			spawnLocation.used = false;

    		}
    	}


    }


    public void randomlyPlaceCargoAreas() {

    	foreach(GameObject CargoArea in CargoAreas) {

    		for (; ; ) {

    			int randomSpawnLocation = Random.Range(0, spawnLocations.Count);
    			if (spawnLocations[randomSpawnLocation].used==false )
    			{
    				agent.transform.position= spawnLocations[randomSpawnLocation].transform.position;
    				spawnLocations[randomSpawnLocation].used= true;
    				spawnLocations[randomSpawnLocation].landOwner= LandOwner.CargoArea;
    				break;
    			}
    		}
    	}
    }


public void MoveAgentToRandomLocation() {

	for (; ; ) { 
		int randomSpawnLocation = Random.Range(0, spawnLocations.Count);

		if (spawnLocations[randomSpawnLocation].used == false ) 
		{
			agent.transform.position = spawnLocations[randomSpawnLocation].transform.position;
			spawnLocations[randomSpawnLocation].used=true;
			break;

		}
	
	}

}

    public void despawnAllCargoes(){
    	foreach (GameObject Cargo in spawnedCargoes) {

    		Destroy(Cargo);
    	}
    	spawnedCargoes.Clear();

    }

    public void ResetCargoTrucks() {

    	foreach(GoalAreaController CargoTruck in CargoAreas.Select(CargoArea => CargoArea.GetComponent<GoalAreaController>())) 
    	{
    		CargoTruck.resetCargoTruck();

    	}
    }


    public void RestoreCargoToSpawn()
    {
    	CargoToSpawn.Clear();
    	foreach(GameObject Cargo in Cargoes)
    	{
    		CargoToSpawn.Add(Cargo);
    	}
    }

    public void EmptySpawnPoints() {

    	foreach(SpawnLocation spawnLocation in spawnLocations) {
    		spawnLocation.used = false;
    		spawnLocation.landOwner= LandOwner.None;
    	}
    }

    public bool AllCargoAreasFilled() {

    	return CargoAreas.All(CargoArea =>CargoArea.gameObject.GetComponent<GoalAreaController>().isFull);
    }

}
