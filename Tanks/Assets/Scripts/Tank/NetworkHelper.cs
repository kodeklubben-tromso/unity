using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHelper : NetworkBehaviour
{
	private GameManager m_GameManager;

	public void SendMissingTanksToClient()
	{
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

	[ClientRpc]
	private void RpcAddTankOnClient(TankManager newTankManager)
	{
		newTankManager.SetMovementScipt(newTankManager.m_Instance.GetComponent<TankMovement>());
		newTankManager.SetShootingScipt(newTankManager.m_Instance.GetComponent<TankShooting>());
		newTankManager.SetNetworkHelperScipt(newTankManager.m_Instance.GetComponent<NetworkHelper>());
		newTankManager.SetCanvasGameObject(newTankManager.m_Instance.GetComponentInChildren<Canvas>().gameObject);

		AddTankToArray(newTankManager);
	}

	private void AddTankToArray(TankManager newTankManager)
	{
		if (m_GameManager == null)
			m_GameManager = (GameManager) GameObject.FindWithTag("GameManager").GetComponent(typeof(GameManager));

		//Dont add existing tanks
		if(m_GameManager.m_Tanks.Count(lt=>lt.m_PlayerNumber == newTankManager.m_PlayerNumber) > 0)
		{
			return;
		}
		var m_TanksTmp = new TankManager[m_GameManager.m_Tanks.Length + 1];
		m_GameManager.m_Tanks.CopyTo(m_TanksTmp, 0);

		newTankManager.m_PlayerNumber = m_TanksTmp.Length; //Increase player number by one
		m_TanksTmp[m_TanksTmp.Length - 1] = newTankManager; //add new tank manager as the last item
		m_GameManager.m_Tanks = m_TanksTmp;
	}
}
