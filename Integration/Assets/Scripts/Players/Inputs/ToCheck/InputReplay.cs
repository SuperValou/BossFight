using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.ToCheck
{
    public enum Mode
    {
        Record, PlayBack
    };

    public class InputReplay : MonoBehaviour
    {
        // -- Editor

        public bool active = false;

        public Mode mode = Mode.Record;
    
        public string FilePath = "Temp/input.json";

        // if true, only configure at start and wait for start
        public bool manualStart = false;

        // virtual button and axis support, list the InputManager's inputs you want to track
        public List<string> AxisList = new List<string>();
        public List<string> ButtonList = new List<string>();


        // -- Class

        // public methods and properties for input access
        public Func<KeyCode, bool> GetKey;
        public Func<KeyCode, bool> GetKeyDown;
        public Func<KeyCode, bool> GetKeyUp;
        public Func<int, bool> GetMouseButton;
        public Func<int, bool> GetMouseButtonDown;
        public Func<int, bool> GetMouseButtonUp;
        public Func<string, bool> GetButton;
        public Func<string, bool> GetButtonDown;
        public Func<string, bool> GetButtonUp;
        public Func<string, float> GetAxis;

        private Func<Vector3> _mousePosition;
        private Func<Vector3> _mouseWorldPosition;
        private Func<Vector2> _mouseScrollDelta;
        public Vector3 mousePosition { get { return _mousePosition(); } }
        public Vector3 mouseWorldPosition { get { return _mouseWorldPosition(); } }
        public Vector2 mouseScrollDelta { get { return _mouseScrollDelta(); } }

        private Func<bool> _anyKey;
        private Func<bool> _anyKeyDown;
	
        // Streams
        private StreamReader _inputPlaybackStream;
        private StreamWriter _inputRecordStream;

        // Input sequences
        private InputSequence _oldFrame;
        private InputSequence _currentFrame;
        private InputSequence _nextFrame;

        // delegate to switch from record to play activity
        private Action<float> _work;

        // start time of the record or playback
        private float _startTime = 0.0f;



        // Use this for initialization
        void Start ()
        {
            if (!active)	// if not active, act as UnityEngine.Input
            {
                SetInputStd();
                _work = Pause;
                return;
            }

            _work = Pause;
            Configure(mode, FilePath);

            if (manualStart)
            {
                return;
            }

            switch (mode) {
                case Mode.Record:
                    StartRecord ();
                    break;

                case Mode.PlayBack:
                    StartPlayBack ();
                    break;
            }
        }

        // Update is called once per frame
        void Update ()
        {
            _work(Time.time - _startTime);
        }

        // FixedUpdate is called every physics update
        void FixedUpdate ()
        {
            _work(Time.fixedTime - _startTime);
        }

        void OnDestroy()
        {
            Stop();
        }

        /*
	 * PUBLIC METHODS
	 * */
        public bool Configure(Mode m, string p)
        {
            FilePath = p;
            mode = m;

            switch (mode)
            {
                case Mode.Record:
                    _oldFrame.Init ();
                    _currentFrame.Init ();

                    _inputRecordStream = new StreamWriter (FilePath, false);	// will overwrite new file Stream
                    if (_inputRecordStream.ToString () == "")
                    {
                        Stop ();
                        Debug.Log ("InputReplay: StreamWriter(" + FilePath + "), file not found ?");
                        return false;
                    }
                    else
                    {
                        _inputRecordStream.AutoFlush = true;
                        SetInputStd ();
                    }
                    break;

                case Mode.PlayBack:
                    _oldFrame.Init ();
                    _currentFrame.Init ();
                    _nextFrame.Init ();

                    _inputPlaybackStream = new StreamReader (FilePath, false);
                    if (_inputPlaybackStream.ToString () == "")
                    {
                        Stop ();
                        Debug.Log ("InputReplay: StreamReader(" + FilePath + "), file not found ?");
                        return false;
                    }
                    else if (!ReadLine ()) // read the first line to check
                    {
                        Stop ();
                        Debug.Log ("InputReplay: empty file");
                        return false;
                    }
                    break;
            }

            return true;
        }

        public void Stop()
        {
            SetInputStd();	// switch back to direct inputs

            active = false;

            switch (mode) {	// close streams
                case Mode.Record:
                    _inputRecordStream.Close();
                    break;
                case Mode.PlayBack:
                    _inputPlaybackStream.Close ();
                    break;
            }

            _work = Pause;
        }

        public void StartRecord()
        {
            active = true;
            _startTime = Time.time;
            SetInputStd ();
            _work = Record;
        }

        public void StartPlayBack()
        {
            active = true;
            _startTime = Time.time;
            SetInputFake ();
            _work = Play;
        }

        /*
	 * PRIVATE METHODS
	 * */
	
        private void SetInputStd()	// redirect public methods and properties to actual UnityEngine.Input
        {
            GetKey = Input.GetKey;
            GetKeyDown = Input.GetKeyDown;
            GetKeyUp = Input.GetKeyUp;
            GetMouseButton = Input.GetMouseButton;
            GetMouseButtonDown = Input.GetMouseButtonDown;
            GetMouseButtonUp = Input.GetMouseButtonUp;

            GetButton = Input.GetButton;
            GetButtonDown = Input.GetButtonDown;
            GetButtonUp = Input.GetButtonUp;
            GetAxis = Input.GetAxis;

            _mousePosition = delegate { return Input.mousePosition; };
            _mouseWorldPosition = delegate { return Camera.main.ScreenToWorldPoint (Input.mousePosition); };
            _mouseScrollDelta = delegate { return Input.mouseScrollDelta; };

            _anyKey = delegate { return Input.anyKey; };
            _anyKeyDown = delegate { return Input.anyKeyDown; };
        }

        private void SetInputFake()	// redirect public methods and properties to our replay system
        {
            GetKey = FakeGetKey;
            GetKeyDown = FakeGetKeyDown;
            GetKeyUp = FakeGetKeyUp;
            GetMouseButton = FakeGetMouseButton;
            GetMouseButtonDown = FakeGetMouseButtonDown;
            GetMouseButtonUp = FakeGetMouseButtonUp;

            GetButton = delegate (string name) { return _currentFrame.vB.Contains (name); };
            GetButtonDown = delegate (string name) { return _currentFrame.vBD.Contains (name); };
            GetButtonUp = delegate (string name) { return _currentFrame.vBU.Contains (name); };
            GetAxis = delegate (string name) { return _currentFrame.vA.ElementAt (AxisList.FindIndex(str => str == name) ); };

            _mousePosition = delegate { return _currentFrame.mP; };
            _mouseWorldPosition = delegate { return _currentFrame.mWP; };
            _mouseScrollDelta = delegate { return _currentFrame.mSD; };

            _anyKey = delegate { return _currentFrame.gK.Any<KeyCode>(); };
            _anyKeyDown = delegate { return _currentFrame.gKD.Any<KeyCode>(); };
        }

        private void Pause(float time)
        {
            // nothing to do
        }

        /* RECORD
	 * */
        private void Record(float time)
        {
            _currentFrame.Init ();
            _currentFrame.t = time;

            // store only true boolean
            foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey (vkey))
                    _currentFrame.gK.Add (vkey);
                if (Input.GetKeyDown (vkey))
                    _currentFrame.gKD.Add (vkey);
                if (Input.GetKeyUp (vkey))
                    _currentFrame.gKU.Add (vkey);
            }

            _currentFrame.mP = Input.mousePosition;

            _currentFrame.mWP = Camera.main.ScreenToWorldPoint (_currentFrame.mP);

            _currentFrame.mSD = Input.mouseScrollDelta;

            foreach (string virtualAxis in AxisList)
                _currentFrame.vA.Add (Input.GetAxis (virtualAxis));

            foreach (string ButtonName in ButtonList)
            {
                if (Input.GetButton (ButtonName))
                    _currentFrame.vB.Add (ButtonName);
                if (Input.GetButtonDown (ButtonName))
                    _currentFrame.vBD.Add (ButtonName);
                if (Input.GetButtonUp (ButtonName))
                    _currentFrame.vBU.Add (ButtonName);
            }

            // only write if something changed
            if (AnyChange(_oldFrame, _currentFrame))
            {
                //Debug.Log (JsonUtility.ToJson (newSequence));
                _inputRecordStream.WriteLine (JsonUtility.ToJson (_currentFrame));
                _oldFrame = _currentFrame;
            }
        }

        // check if 
        private bool AnyChange(InputSequence seqA, InputSequence seqB)
        {
            if(!Enumerable.SequenceEqual(seqA.gK, seqB.gK)) return true;
            else if (!Enumerable.SequenceEqual(seqA.vB, seqB.vB)) return true;
            else if (!Enumerable.SequenceEqual(seqA.vA, seqB.vA)) return true;
            else if (seqA.mP != seqB.mP) return true;
            else if (seqA.mWP != seqB.mWP) return true;
            else if (seqA.mSD != seqB.mSD) return true;
            else return false;
        }

        /* PLAYBACK
	 * */
        private void Play(float time)
        {
            if (time >= _nextFrame.t)
            {
                _oldFrame = _currentFrame;
                _currentFrame = _nextFrame;
                //Debug.Log (time);

                _nextFrame.Init ();
                if (!ReadLine ())
                {
                    Stop ();
                    Debug.Log ("InputPlayback: EndOfFile");
                }
            }
        }

        private bool ReadLine()	// read a new line in file for the next sequence to play
        {
            string newline = _inputPlaybackStream.ReadLine ();

            if (newline == null)
                return false;

            _nextFrame = JsonUtility.FromJson<InputSequence> (newline);
            return true;
        }

        private bool GetKeyCodeInList (KeyCode code, List<KeyCode> list)
        {
            foreach (KeyCode vkey in list)
            {
                if (vkey == code)
                    return true;
            }
            return false;
        }

        /*
	 * PRIVATE FAKE INPUT
	 * */
        private bool FakeGetKey(KeyCode code) { return GetKeyCodeInList (code, _currentFrame.gK); }
        private bool FakeGetKeyDown(KeyCode code) { return GetKeyCodeInList (code, _currentFrame.gKD); }
        private bool FakeGetKeyUp(KeyCode code) { return GetKeyCodeInList (code, _currentFrame.gKU); }
        private bool FakeGetMouseButton(int button) { return GetKeyCodeInList (KeyCode.Mouse0+button, _currentFrame.gK); }
        private bool FakeGetMouseButtonDown(int button) { return GetKeyCodeInList (KeyCode.Mouse0+button, _currentFrame.gKD); }
        private bool FakeGetMouseButtonUp(int button) { return GetKeyCodeInList (KeyCode.Mouse0+button, _currentFrame.gKU); }
        /* in case of removed up and down lists, compare old and current sequence
	public bool FakeGetKeyDown(KeyCode code) { return !GetKeyCodeInList (code, oldSequence.getKey) & GetKeyCodeInList (code, currentSequence.getKey); }
	public bool FakeGetKeyUp(KeyCode code) { return GetKeyCodeInList (code, oldSequence.getKey) & !GetKeyCodeInList (code, currentSequence.getKey); }
	public bool FakeGetMouseButtonDown(int button) { return !GetKeyCodeInList (KeyCode.Mouse0+button, oldSequence.getKey) & GetKeyCodeInList (KeyCode.Mouse0+button, currentSequence.getKey); }
	public bool FakeGetMouseButtonUp(int button) { return GetKeyCodeInList (KeyCode.Mouse0+button, oldSequence.getKey) & !GetKeyCodeInList (KeyCode.Mouse0+button, currentSequence.getKey); }
	*/
    }
}