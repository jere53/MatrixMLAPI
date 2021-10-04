using UnityEngine;
using MLAPI;
using TMPro;
using System.Text;
using MLAPI.Spawning;
using UnityEngine.UI;

public class LogInUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private GameObject nameEntryUI;
        [SerializeField] private GameObject leaveButton;
        [SerializeField] private GameObject humanButton;
        [SerializeField] private GameObject agentButton;
        [SerializeField] private GameObject neoButton;


        public string GetName()
        {
            return nameInputField.text;
        }
        
        private string playerTypeSelection = "humano"; //el tipo de personaje que se spawneara

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void OnDestroy()
        {
            // Prevent error in the editor
            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void SelectHumanPlayer()
        {
            playerTypeSelection = "humano";
            neoButton.GetComponent<Image>().color = Color.white;
            humanButton.GetComponent<Image>().color = Color.green;
            agentButton.GetComponent<Image>().color = Color.white;
        }
        
        public void SelectNeoPlayer()
        {
            playerTypeSelection = "neo";
            neoButton.GetComponent<Image>().color = Color.green;
            humanButton.GetComponent<Image>().color = Color.white;
            agentButton.GetComponent<Image>().color = Color.white;
        }
        
        public void SelectAgentPlayer()
        {
            playerTypeSelection = "agente";
            neoButton.GetComponent<Image>().color = Color.white;
            humanButton.GetComponent<Image>().color = Color.white;
            agentButton.GetComponent<Image>().color = Color.green;
        }
        
        public void Server()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartServer();
        }

        public void Client()
        {
            // Le mandamos al servidor el tipo de personajes que queremos
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(playerTypeSelection);
            NetworkManager.Singleton.StartClient();
        }

        public void Leave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }

            nameEntryUI.SetActive(true);
            leaveButton.SetActive(false);
        }

        private void HandleServerStarted()
        {
            // Temporary workaround to treat host as client
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.ServerClientId);
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            // Are we the client that is connecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                nameEntryUI.SetActive(false);
                leaveButton.SetActive(true);
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            // Are we the client that is disconnecting?
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                nameEntryUI.SetActive(true);
                leaveButton.SetActive(false);
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;
            Debug.Log(connectionData);

            switch (NetworkManager.Singleton.ConnectedClients.Count)
            {
                case 1:
                    spawnPos = new Vector3(0f, 2f, 0f);
                    spawnRot = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case 2:
                    spawnPos = new Vector3(3f, 2f, 0f);
                    spawnRot = Quaternion.Euler(0f, 0f, 0f);
                    break;
            }


            ulong? prefabHash = 0;
            
            Debug.Log(Encoding.UTF8.GetString(connectionData));
            
            switch (Encoding.UTF8.GetString(connectionData))
            {
                    case "humano":
                        prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Humano");
                        break;
                    case "neo":
                        prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Neo");
                        break;
                    case "agente":
                        prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Smith");
                        break;
            }
            
            if (prefabHash != 0)
                callback(true, prefabHash, true, spawnPos, spawnRot);
            else
                callback(true, null, true, spawnPos, spawnRot);
            
        }
    }
