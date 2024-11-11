using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI CodeText;
    [SerializeField] private TMP_InputField roomCodeInput;

    private Relay _relay;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;

        _relay = FindObjectOfType<Relay>();

        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostButton.onClick.AddListener( () =>
        {
            NetworkManager.Singleton.StartHost();
           // _relay.CreateRelay();
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            _relay.JoinRelay(roomCodeInput.text);
            Debug.Log(roomCodeInput.text);
        });
    }
    private void Update()
    {
        if (_relay.inServer)
        {
            CodeText.text = _relay.joinCode;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            CodeText.text = "Put In Room Code Before Clicking Client";
        }
    }

}
