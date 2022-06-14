    // vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using UnityEngine;
using UnityEngine.UI;

    namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Manager HUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;
        
        public InputField ipInputField;
        public Text statusText;
        public GameObject connectionPanel;
        public GameObject stopButtonsPanel;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void FixedUpdate()
        {
            if (NetworkClient.isConnected && NetworkServer.active)
            {
                StatusLabels();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                NetworkClient.Ready();
                if (NetworkClient.localPlayer == null)
                {
                    NetworkClient.AddPlayer();
                }
            }
        }

        void StatusLabels()
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                //GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
                statusText.text = $"<b>Host</b>: running via {Transport.activeTransport}";
            }
            // server only
            else if (NetworkServer.active)
            {
                //GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
                statusText.text = $"<b>Server</b>: running via {Transport.activeTransport}";
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                //GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
                statusText.text = $"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}";
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }
        
        //
        //UI SYSTEM
        //

        public void HostServerButton()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    manager.StartHost();
                    connectionPanel.SetActive(false);
                    stopButtonsPanel.SetActive(true);
                }
            }
        }
        
        public void ClientConnectionButton()
        {
            if (!NetworkClient.active)
            {
                // Client + IP
                manager.StartClient();
                manager.networkAddress = ipInputField.text;
                connectionPanel.SetActive(false);
                stopButtonsPanel.SetActive(true);
            }
        }

        public void StopServerClientButton()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                manager.StopHost();
                connectionPanel.SetActive(true);
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                manager.StopClient();
                connectionPanel.SetActive(true);
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                manager.StopServer();
                connectionPanel.SetActive(true);
            }
        }
    }
}
