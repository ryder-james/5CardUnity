using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerServerComponent : MonoBehaviour {
	[SerializeField] int port;
	[SerializeField] private Player player = null;
	SimpleHttpServer server;

	//private Card card;
	void Start() {
		server = new SimpleHttpServer(port);
		server.RegisterPostAction("discard", Discard);
		server.RegisterPostAction("bet", Bet);
		server.RegisterGetAction("", Root);
		server.RegisterGetAction("version", Version);
		server.Start();

	}
	private void OnDestroy() {
		if (server != null) {
			server.Stop();
		}
	}

	private string Root() {
		return "{\"Root\": \"page\"}";
	}

	private string Version() {
		return "{\"Version\": \"PAGE\"";
	}

	private string Bet(string jsonIn) {
		var sPlayer = JsonUtility.FromJson<SerializablePlayer>(jsonIn);
		player.MinimumRaiseAmount = sPlayer.minimumRaiseAmount;
		player.CallAmount = sPlayer.callAmount;
		//certain functions can not be called from this callback (particularly unity functions like GetComponent<>(), since it gets called from the server's thread
		int bet = player.GetBet();
		Debug.Log(bet);

		return bet.ToString();
	}

	private string Discard(string jsonIn) {
		int[] discards = player.GetDiscards();
		Debug.Log(discards);

		string discardString = "[";
		if (discards.Length > 0) {
			foreach (int i in discards) {
				discardString += $"{i}, ";
			}
			discardString = discardString.Substring(0, discardString.Length - 2);
		}
		discardString += "]";

		Debug.Log(discardString);

		return discardString;
	}
}
