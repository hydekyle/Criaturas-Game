using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : NetworkBehaviour {

	void Start()
    {
        if (isLocalPlayer) print("Soy local player");
    }
}
