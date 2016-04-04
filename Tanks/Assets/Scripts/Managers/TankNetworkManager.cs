using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TankNetworkManager : NetworkManager
{
	//Called when a player is added
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("OnServerAddPlayer");

		//Create and spawn the new tank
		GameManager gm = (GameManager) this.gameObject.GetComponent(typeof(GameManager));

		TankManager tm = gm.AddTankManager();

		NetworkServer.AddPlayerForConnection(conn, tm.m_Instance, playerControllerId);

		//We have to spawn the tanks on client AFTER the Tank have been spawned in the network
		tm.SpawnTanksOnClients();
	}

	// called when a player is removed for a client
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		if (player.gameObject != null)
		{
			NetworkServer.Destroy(player.gameObject);
		}
	}

	public override void OnStopServer()
	{
		GameManager gm = (GameManager) this.gameObject.GetComponent(typeof(GameManager));
		gm.RemoveAllTankManagers();

		base.OnStopServer();
	}

	// called when connected to a server
	public override void OnClientConnect(NetworkConnection conn)
	{
		//ClientScene.Ready(conn);
		//ClientScene.AddPlayer(0);
		base.OnClientConnect(conn);
	}

	// called when disconnected from a server
	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
	}
}
