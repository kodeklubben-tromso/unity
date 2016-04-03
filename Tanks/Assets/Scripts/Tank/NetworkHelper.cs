using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHelper : NetworkBehaviour
{
	private GameManager m_GameManager;

	public void SendMissingTanksToClient()
	{
		Debug.Log("SendMissingTanksToClient() ");
		if (m_GameManager == null)
			m_GameManager = (GameManager) GameObject.FindWithTag("GameManager").GetComponent(typeof(GameManager));
		if (m_GameManager.m_IsOnlineMultiplayer)
		{
			foreach(TankManager tm in m_GameManager.m_Tanks)
			{
				RpcAddTankOnClient(tm);		
			}

		}
	}
	public void AddTankOnClients(TankManager tm)//Color color, Transform spawnPoint)
	{
		Debug.Log("AddTankOnClients(): "+tm.m_PlayerNumber);
		if (m_GameManager == null)
			m_GameManager = (GameManager) GameObject.FindWithTag("GameManager").GetComponent(typeof(GameManager));
		if (m_GameManager.m_IsOnlineMultiplayer)
		{
			RpcAddTankOnClient(tm);
		}
	}

	[ClientRpc]
	private void RpcAddTankOnClient(TankManager newTankManager)
	{
		newTankManager.SetMovementScipt(newTankManager.m_Instance.GetComponent<TankMovement>());
		newTankManager.SetShootingScipt(newTankManager.m_Instance.GetComponent<TankShooting>());
		newTankManager.SetNetworkHelperScipt(newTankManager.m_Instance.GetComponent<NetworkHelper>());
		newTankManager.SetCanvasGameObject(newTankManager.m_Instance.GetComponentInChildren<Canvas>().gameObject);

		//Debug.Log("Recieved at client: isLocalPlayer: " + isLocalPlayer);
		//if (!isLocalPlayer)//make sure we dont add the same tank twice
		{
			AddTankToArray(newTankManager);
		}
	}

	private void AddTankToArray(TankManager newTankManager)
	{
		Debug.Log("AddTankToArray()");
		if (m_GameManager == null)
			m_GameManager = (GameManager) GameObject.FindWithTag("GameManager").GetComponent(typeof(GameManager));
		//Debug.Log("Add Tank with number?: "+newTankManager.m_PlayerNumber);

		//if(m_GameManager.m_Tanks.Count(lt=>lt.m_PlayerNumber == newTankManager.m_PlayerNumber) > 0)
		//{
		//	Debug.Log("NO");
		//	return;
		//}
		//Debug.Log("YES");

		var m_TanksTmp = new TankManager[m_GameManager.m_Tanks.Length + 1];
		m_GameManager.m_Tanks.CopyTo(m_TanksTmp, 0);

		newTankManager.m_PlayerNumber = m_TanksTmp.Length; //Increase player number by one
		m_TanksTmp[m_TanksTmp.Length - 1] = newTankManager; //add new tank manager as the last item
		m_GameManager.m_Tanks = m_TanksTmp;
	}
}
