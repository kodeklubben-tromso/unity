using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TankNetworkManager : NetworkManager {

	public TankManager[] GetTankManagerList()
	{
		GameManager gm = (GameManager)this.gameObject.GetComponent(typeof(GameManager));
		return gm.m_Tanks;
	}
	
	//Called when a player is added 
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("OnServerAddPlayer");

		//Create and spawn the new tank
		GameManager gm = (GameManager)this.gameObject.GetComponent(typeof(GameManager));
		TankManager tm = gm.AddTankManager(playerControllerId);
		gm.SpawnSingleTank(tm);

	    NetworkServer.AddPlayerForConnection(conn, tm.m_Instance, playerControllerId);
	   
		//OnServerAddPlayer(conn, playerControllerId);
	}

	// called when a player is removed for a client
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		Debug.Log("OnServerRemovePlayer");
		if (player.gameObject != null) 
	    {
	         NetworkServer.Destroy(player.gameObject);
	    }

		//GameManager gm = (GameManager)this.gameObject.GetComponent(typeof(GameManager));	    
		//gm.RemoveTankManager(player.gameObject.);

	}

	public override void OnStopServer ()
	{
		GameManager gm = (GameManager)this.gameObject.GetComponent(typeof(GameManager));
		gm.RemoveAllTankManagers();

		base.OnStopServer ();
	}

	// called when connected to a server
	public override void OnClientConnect(NetworkConnection conn)
	{
		Debug.Log("OnClientConnect");
	    //ClientScene.Ready(conn);
	    //ClientScene.AddPlayer(0);

		//GameManager gm = (GameManager)this.gameObject.GetComponent(typeof(GameManager));
		//gm.GetTankManagerListFromServer();

		//TankManager tm = gm.AddTankManager(0);
		//gm.SpawnSingleTank(tm,gm.m_Tanks.Length,true);

	    base.OnClientConnect(conn);
	}

	// called when disconnected from a server
	public override void OnClientDisconnect(NetworkConnection conn)
	{
		

		Debug.Log("OnClientConnect");
	    base.OnClientDisconnect(conn);
	}
}
