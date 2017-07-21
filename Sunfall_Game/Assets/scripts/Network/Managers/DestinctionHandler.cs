//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class DestinctionHandler : PhotonManager<DestinctionHandler>
//{
//    [SerializeField, Tooltip("Team colors, neutral,1,2,3 etc.")]
//    private Material[] teamColors;

//    [SerializeField, Tooltip("Drag and drop the prefabs here NOT the ones from scene view hierarchy")]
//    private GameObject[] startLocations; // TODO: find a solution for different maps with different positions

//    [SerializeField]
//    private List<GameObject> players;

//    [SerializeField, Tooltip("the distance the camera starts behind the home town and when space is pushed")]
//    private float camDistance = 40f;

//    //[SerializeField, Tooltip("The astar pathing for scanning.")]
//    //private AstarPath aPath;

//    private Vector3 camPos;

//    private float camY;

//    private bool isPositioned;
//    private bool isDistinct;

//    public List<GameObject> Players
//    { get { return players; } }

//    public bool IsPositioned
//    { get { return isPositioned; } set { isPositioned = value; } }

//    public bool IsDistinct
//    { get { return isDistinct; } set { isDistinct = value; } }

//    /// <summary>
//    /// Runs through the players and makes sure the name, number,startlocation and color is set correctly and updated for all players
//    /// </summary>
//    [PunRPC]
//    public void DifferentiatePlayers()
//    {
//        players.Clear(); // fresh start
//        players.Add(GameObject.Find("NeutralPlayer")); // add the neutral player, he has a coliur too
//        players.AddRange(GameObject.FindGameObjectsWithTag("Player")); // find the players

//        foreach (var p in Players) // run through the players
//        {
//            Player player = p.GetComponent<Player>(); // ease of access
//            PhotonView playerView = p.GetComponent<PhotonView>();

//            if (p.gameObject.name != "NeutralPlayer") // for the actual players
//            {
//                player.Username = playerView.owner.name; // set their values
//                player.TeamNumber = playerView.ownerId;
//                player.TeamColor = teamColors[player.TeamNumber];

//                if (p.GetComponentInChildren<Town>()) // if they have a town
//                {
//                    player.HomeTown = p.GetComponentInChildren<Town>().gameObject; // make it their home

//                    for (int s = 0; s < startLocations.Length; s++) // locate them accordinly
//                    {
//                        player.HomeTown.transform.localPosition = startLocations[player.TeamNumber - 1].transform.localPosition; // TODO: Randomize starting location?
//                        player.HomeTown.transform.localRotation = startLocations[player.TeamNumber - 1].transform.localRotation;

//                        if (!isPositioned) // give their camera variables for positioning
//                        {
//                            camPos = new Vector3(startLocations[player.TeamNumber - 1].transform.localPosition.x, Camera.main.transform.position.y, startLocations[player.TeamNumber - 1].transform.position.z);
//                            camY = startLocations[player.TeamNumber - 1].transform.localRotation.eulerAngles.y;

//                            CameraAtHomeTown();
//                            aPath.Scan();
//                            isPositioned = true; // run once.
//                        }
//                    }
//                }
//            }

//            if (player.TeamNumber <= 0) // if the player is neutral player.. should never happen
//            {
//                player.TeamNumber = 0;
//            }

//            SetColor(); // change the colour,
//        }
//    }
//    /// <summary>
//    /// puts the camera over the home town
//    /// </summary>
//    public void CameraAtHomeTown()
//    {
//        // main cameras angles  = its own X angle, towns Y angle and the cameras own Z angle
//        Camera.main.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, camY, Camera.main.transform.eulerAngles.z);

//        // change the position of the camera
//        Camera.main.transform.position = camPos;

//        // temp empty vector
//        Vector3 movement = new Vector3(0, 0, 0);

//        movement.z -= camDistance; // the z of the movement set to the distance we want to be behind the town when we recall

//        movement = Camera.main.transform.TransformDirection(movement); // set movement to the cameras local position to world space
//        movement.y = 0; // reset movement y

//        Vector3 origin = Camera.main.transform.position; // set a local temporary var origin to the cameras pos

//        Vector3 destination = origin; // destination ( where we want to end up) starts out as the origin
//        destination.x += movement.x; // add the movement to the destination
//        destination.y += movement.y;
//        destination.z += movement.z;

//        Camera.main.transform.position = Vector3.MoveTowards(origin, destination, 40); // move the camera - starting point, where we are going, and how fast.
//    }

//    /// <summary>
//    /// change the colour of objects to match the ownership
//    /// </summary>
//    [PunRPC]
//    public void SetColor()
//    {
//        foreach (var p in Players) // run through the players
//        {
//            Player player = p.GetComponent<Player>();
//            var renderers = p.GetComponentsInChildren<Renderer>(); // get all the renderers in the player
//            foreach (var r in renderers)
//            {
//                if (r.gameObject.tag == "PlayerColor") // change the ones that need to be changed
//                {
//                    if (r.gameObject.GetComponentInParent<Unit>())
//                    {
//                        r.material = player.TeamColor; // paint it //TODO: COMMENT THIS BACK IN!
//                    }
//                }
//            }
//        }
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//    }
//}