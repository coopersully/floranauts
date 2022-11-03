using Player;
using UnityEngine;

/*Written by Thomas Walker
 11/1/2022
 
 Things to Work On
 - What planet abilities will there be?
 - What do the planet abilities do?
 - Does there need to be a copy of everything for each player? How are we doing that?
 */
public class MoundsManager : MonoBehaviour
{
    //Dropdown of planet abilities
    public MoundType planetAbility = new();
    public enum MoundType
    {
        None,
        Jetpack, 
        DoubleJump, 
        IncreaseSpeed,
        Grenade
    };

    [Header("-Players-")]
    public GameObject bluePlayer;
    //public GameObject redPlayer;
    
    // [Header("-Player Animations-")]
    // public Animation plantingTree;
    // public Animation destroyingTree;
    
    [Header("-Planet Variables-")]
    public GameObject planetTree;
    public GameObject planetTreeLeaves;
    public GameObject planet;
    public Material grayPlanet;
    public Material bluePlanet;
    public Material redPlanet;

    // [Header("-Tree Animations-")]
    // public Animation treeGrowing;
    // public Animation treeDying;
    
    //Boolean Checks
    private bool _playerIsPlanting;
    private bool _blueCaptured = false;
    private bool _redCaptured = false;
    private bool _blueHasAbility = false;
    private bool _redHasAbility = false;

    private void Start()
    {
        planet.GetComponent<MeshRenderer> ().material = grayPlanet;
        planetTreeLeaves.GetComponent<MeshRenderer> ().material = grayPlanet;
        planetTree.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Blue Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //If the planet is unclaimed
                if (_blueCaptured == false && _redCaptured == false)
                {
                    Debug.Log("Blue Player Claimed Planet");
                    _blueCaptured = true;
                    
                    //play plantingTree animation
                    //play treeGrowing animation
                
                    planetTree.SetActive(true);

                    PlayerScoreManager.BluePlayerPlanetsCount++;
                
                    planet.GetComponent<MeshRenderer> ().material = bluePlanet;
                    planetTreeLeaves.GetComponent<MeshRenderer> ().material = bluePlanet;
                
                    //Not sure what will happen exactly when a player gets these abilities...
                    _blueHasAbility = true;
                }
                
                //if the planet is claimed by the other player
                if (_redCaptured == true)
                {
                    Debug.Log("Blue Player Neutralized Planet");
                    _redCaptured = false;
                    
                    //play destroyingTree animation
                    //play treeDying animation
                
                    planetTree.SetActive(false);
                    
                    planet.GetComponent<MeshRenderer> ().material = grayPlanet;
                    planetTreeLeaves.GetComponent<MeshRenderer> ().material = grayPlanet;

                    _redHasAbility = false;
                }
            }
        }
        
        if (other.gameObject.CompareTag("Red Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //If the planet is unclaimed
                if (_redCaptured == false && _blueCaptured == false)
                {
                    Debug.Log("Red Player Claimed Planet");
                    _redCaptured = true;
                    
                    //play plantingTree animation
                    //play treeGrowing animation
                
                    planetTree.SetActive(true);

                    PlayerScoreManager.RedPlayerPlanetsCount++;
                
                    planet.GetComponent<MeshRenderer> ().material = redPlanet;
                    planetTreeLeaves.GetComponent<MeshRenderer> ().material = redPlanet;
                
                    //Not sure what will happen exactly when a player gets these abilities...
                    _redHasAbility = true;
                }
                
                //if the planet is claimed by the other player
                if (_blueCaptured == true)
                {
                    Debug.Log("Blue Player Neutralized Planet");
                    _blueCaptured = false;
                    
                    //play destroyingTree animation
                    //play treeDying animation
                
                    planetTree.SetActive(false);
                    
                    planet.GetComponent<MeshRenderer> ().material = grayPlanet;
                    planetTreeLeaves.GetComponent<MeshRenderer> ().material = grayPlanet;

                    _blueHasAbility = false;
                }
            }
        }
  
    }
}
