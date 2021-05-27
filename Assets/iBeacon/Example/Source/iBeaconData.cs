using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//using GameMission;
//using View;

public class iBeaconData : MonoBehaviour
{
	[SerializeField]
    private Text _statusText;
	[SerializeField]
	private Button _bluetoothButton;
	[SerializeField]
	private GameObject _statusScreen;
	[SerializeField]
	private GameObject _menuScreen;

	private Text _bluetoothText;

	/** Input **/
	// beacontype
	private BeaconType bt_PendingType;
	private BeaconType bt_Type;

	/*** Beacon Properties ***/
	// Beacontype
	[SerializeField] //to access in editor, avoiding public varible declaration
	private Text txt_actualType;
	// Region
	[SerializeField]
	private Text txt_actualRegion;
	private string s_Region;
	[SerializeField]
	private Text txt_actualUUID;
	private string s_UUID;
	[SerializeField]
	private Text txt_actualMajor;
	private string s_Major;
	[SerializeField]
	private Text txt_actualMinor;
	private string s_Minor;

	// Beacon BroadcastMode default Send
	private BroadcastMode bm_Mode;

	// Beacon BroadcastState (Start, Stop)
	[SerializeField]
	private Image img_ButtonBroadcastState;
	[SerializeField]
	private Text txt_BroadcastState_ButtonText;
	[SerializeField]
	private Text txt_BroadcastState_LabelText;
	private BroadcastState bs_State;

	// GameObject for found Beacons
	[SerializeField]
	private GameObject go_ScrollViewContent;
	[SerializeField]
	private GameObject go_FoundBeacon;

	List<GameObject> go_FoundBeaconCloneList = new List<GameObject>();
	GameObject go_FoundBeaconClone;
	private float f_ScrollViewContentRectWidth;
	private float f_ScrollViewContentRectHeight;
	private int i_BeaconCounter = 0;

	// Receive
	private static List<Beacon> _mybeacons = new List<Beacon>();

    public static List<Beacon> mybeacons
    {
        get
        {
            if(_mybeacons != null) { return _mybeacons; }

			return null;

		}
    }

	private void Start()
	{
		setBeaconPropertiesAtStart();
		BluetoothStatus();
	}

    private void setBeaconPropertiesAtStart()
    {
		bm_Mode = BroadcastMode.receive;
		bt_Type = BeaconType.iBeacon;

		if (iBeaconReceiver.regions.Length != 0)
		{
			Debug.Log("check iBeaconReceiver-inspector");
			s_Region = iBeaconReceiver.regions[0].regionName;
			bt_Type = iBeaconReceiver.regions[0].beacon.type;
			s_UUID = iBeaconReceiver.regions[0].beacon.UUID;
			s_Major = iBeaconReceiver.regions[0].beacon.major.ToString();
			s_Minor = iBeaconReceiver.regions[0].beacon.minor.ToString();
		}

		bs_State = BroadcastState.inactive;
		SetBeaconProperties();
		SetBroadcastState();
		Debug.Log("Beacon properties and modes restored");
	}

    private void SetBeaconProperties()
    {
		// setText
		txt_actualType.text = bt_Type.ToString();
		txt_actualRegion.text = s_Region;

        if(bt_Type == BeaconType.iBeacon)
        {
			txt_actualUUID.text = s_UUID;
			txt_actualMajor.text = s_Major;
			txt_actualMinor.text = s_Minor;
		}
	}

    private void SetBroadcastState()
    {
		// setText
		if (bs_State == BroadcastState.inactive)
			txt_BroadcastState_ButtonText.text = "Start";
		else
			txt_BroadcastState_ButtonText.text = "Stop";
		txt_BroadcastState_LabelText.text = bs_State.ToString();
	}

    private void BluetoothStatus()
    {
		_bluetoothButton.onClick.AddListener(delegate () {
			BluetoothState.EnableBluetooth();
		});
		_bluetoothText = _bluetoothButton.GetComponentInChildren<Text>();
		Debug.Log("BluetoothLowEnergyState: ");
		BluetoothState.BluetoothStateChangedEvent += delegate(BluetoothLowEnergyState state)
		{
            switch (state)
            {
				case BluetoothLowEnergyState.TURNING_OFF:
				case BluetoothLowEnergyState.TURNING_ON:
					break;

				case BluetoothLowEnergyState.UNKNOWN:
				case BluetoothLowEnergyState.RESETTING:
					SwitchToStatus();
					_statusText.text = "Checking Device…";
					break;
				case BluetoothLowEnergyState.UNAUTHORIZED:
					SwitchToStatus();
					_statusText.text = "You don't have the permission to use beacons.";
					break;
				case BluetoothLowEnergyState.UNSUPPORTED:
					SwitchToStatus();
					_statusText.text = "Your device doesn't support beacons.";
					break;
				case BluetoothLowEnergyState.POWERED_OFF:
					SwitchToMenu();
					StarReceive();
					_bluetoothButton.interactable = true;
					_bluetoothText.text = "Enable Bluetooth";
					break;
				case BluetoothLowEnergyState.POWERED_ON:
					SwitchToMenu();
					StarReceive();
					_bluetoothButton.interactable = false;
					_bluetoothText.text = "Bluetooth already enabled";
					break;
				case BluetoothLowEnergyState.IBEACON_ONLY:
					SwitchToMenu();
					StarReceive();
					_bluetoothButton.interactable = false;
					_bluetoothText.text = "iBeacon only";
					break;
				default:
					SwitchToStatus();
					_statusText.text = "Unknown Error";
					break;
			}
		};

		f_ScrollViewContentRectWidth = ((RectTransform)go_FoundBeacon.transform).rect.width;
		f_ScrollViewContentRectHeight = ((RectTransform)go_FoundBeacon.transform).rect.height;
		BluetoothState.Init();
	}

	private void SwitchToStatus()
	{
		_statusScreen.SetActive(true);
		_menuScreen.SetActive(false);
	}

	private void SwitchToMenu()
	{
		_statusScreen.SetActive(false);
		_menuScreen.SetActive(true);
	}

	private void StarReceive()
    {
		/*** Beacon will start ***/
        if(bs_State == BroadcastState.inactive)
        {
			// ReceiveMode
            if(bm_Mode == BroadcastMode.receive)
            {
				iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;

                if(bt_Type == BeaconType.iBeacon)
                {
                    // compare uuid
					iBeaconReceiver.regions = new iBeaconRegion[] { new iBeaconRegion(s_Region, new Beacon(s_UUID ,Convert.ToInt32(s_Major), Convert.ToInt32(s_Minor))) };
                }
                else
					Debug.Log("type is not ibeacon");
			}

			// !!! Bluetooth has to be turned on !!! TODO
			iBeaconReceiver.Scan();
			Debug.Log("Listening for beacons");

			bs_State = BroadcastState.active;
			img_ButtonBroadcastState.color = Color.red;
		}
        else
        {
			if (bm_Mode == BroadcastMode.receive)
            {
				iBeaconReceiver.Stop();
                iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
				removeFoundBeacons();
			}

			bs_State = BroadcastState.inactive;
			img_ButtonBroadcastState.color = Color.green;
		}

		SetBroadcastState();
	}

    private void OnBeaconRangeChanged(Beacon[] beacons)
    {
        foreach(Beacon b in beacons)
        {
			var index = _mybeacons.IndexOf(b);
			if (index == -1)
				_mybeacons.Add(b);
			else
				_mybeacons[index] = b;
        }

        for(int i = _mybeacons.Count - 1; i >= 0; i--)
        {
			// we delete the beacon if it was last seen more than 10 seconds ago
			if (_mybeacons[i].lastSeen.AddSeconds(10) < DateTime.Now)
            {
				_mybeacons.RemoveAt(i);
            }
        }

		DisplayOnBeaconFound();
		iBeaconMissionSetting.Instance.MissionSearch(mybeacons);
	}

	private void DisplayOnBeaconFound()
    {
		removeFoundBeacons();
		RectTransform rt_Content = (RectTransform)go_ScrollViewContent.transform;

		foreach (Beacon b in _mybeacons)
		{
			go_FoundBeaconClone = Instantiate(go_FoundBeacon);
			// make it child of the ScrollView
			go_FoundBeaconClone.transform.SetParent(go_ScrollViewContent.transform);
			float f_scaler = go_FoundBeaconClone.transform.localScale.y;
			Vector2 v2_scale = new Vector2(1,1);

			go_FoundBeaconClone.transform.localScale = v2_scale; //reset scalefactor
			Vector3 pos = go_ScrollViewContent.transform.position; // get anchor position
			pos.y -= f_ScrollViewContentRectHeight / f_scaler * i_BeaconCounter;
			go_FoundBeaconClone.transform.position = pos;
			i_BeaconCounter++;

			rt_Content.sizeDelta = new Vector2(f_ScrollViewContentRectWidth, f_ScrollViewContentRectHeight * i_BeaconCounter); // resize scrollviewcontent
			go_FoundBeaconClone.SetActive(true);
			go_FoundBeaconCloneList.Add(go_FoundBeaconClone);
			Text[] Texts = go_FoundBeaconClone.GetComponentsInChildren<Text>(); // get text components

			foreach (Text t in Texts)
				t.text = "";
			Debug.Log("found Beacon: " + b.ToString());

			if (b.type == BeaconType.iBeacon && b.accuracy >= 0)
			{
				Texts[0].text = "UUID:" + b.UUID.ToString();
				Texts[1].text = "Major:" + b.major.ToString();
				Texts[2].text = "Minor:" + b.minor.ToString();
				Texts[3].text = "Strength:" + b.strength.ToString() + " db";
				Texts[4].text = "Accuracy:" + b.accuracy.ToString().Substring(0, 10) + " m";
				Texts[5].text = "Rssi:" + b.rssi.ToString() + " db";
				Debug.Log("found Beacon rssi: " + b.rssi.ToString() + " db");
			}
			else
			{
				Texts[0].text = "Find iBeacon Error";
				Debug.Log("Find iBeacon Error");
			}
		}
	}

	private void removeFoundBeacons()
    {
		Debug.Log("removing all found Beacons");

		// set scrollviewcontent to standardsize
		RectTransform rt_Content = (RectTransform)go_ScrollViewContent.transform;
		rt_Content.sizeDelta = new Vector2(f_ScrollViewContentRectWidth, f_ScrollViewContentRectHeight);

		// destroying each clone
        foreach(GameObject go in go_FoundBeaconCloneList)
			Destroy(go);

        go_FoundBeaconCloneList.Clear();
		i_BeaconCounter = 0;
	}

	// PlayerPrefs
	private void SaveiBeaconInfo()
    {
		PlayerPrefs.SetInt("Type", (int)bt_Type);
		PlayerPrefs.SetString("Region", s_Region);
		PlayerPrefs.SetString("UUID", s_UUID);
		PlayerPrefs.SetString("Major", s_Major);
		PlayerPrefs.SetString("Minor", s_Minor);
		PlayerPrefs.SetInt("BroadcastMode", (int)bm_Mode);
	}
}
