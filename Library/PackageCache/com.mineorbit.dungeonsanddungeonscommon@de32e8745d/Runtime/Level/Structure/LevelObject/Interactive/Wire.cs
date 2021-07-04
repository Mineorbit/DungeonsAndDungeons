using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Wire : LevelObject
    {
        public static LevelObjectData wirePreset;

        public InteractiveLevelObject sender;
        public InteractiveLevelObject receiver;

        public LineRenderer lr;
        public MeshCollider mc;

        public Transform A;

        public Transform B;

        public int numberOfPoints = 20;

        private Vector3 start = new Vector3(0, 0, 0);

        private float t;

        public void Start()
        {
            lr.SetWidth(0.4f, 0.4f);
        }

        public void Update()
        {
            var curve = new AnimationCurve();
            t += Time.deltaTime;
            for (float f = 0; f < 1; f += 1f / numberOfPoints)
                curve.AddKey(f, 0.3f + 0.125f / 2 * Mathf.Sin(-8 * t + 8 * f));
            lr.widthCurve = curve;
        }

        public void OnDestroy()
        {
            if (receiver != null)
                receiver.inBoundWires.Remove(this);
            if (sender != null)
                sender.RemoveReceiver(receiver, this);
        }

        public override void OnInit()
        {
            base.OnInit();
            enabled = true;
            numberOfPoints = 16;
            lr.positionCount = numberOfPoints;

            SetWire(Level.instantiateType == Level.InstantiateType.Edit);
        }


        public static Wire Create(InteractiveLevelObject start, InteractiveLevelObject end, Color color)
        {
            if (wirePreset == null)
                wirePreset = Resources.Load<LevelObjectData>("LevelObjectData/Environment/Interactive/Wire");

            var wire = LevelManager.currentLevel.AddDynamic(wirePreset, new Vector3(0, 0, 0),
                new Quaternion(0, 0, 0, 0));
            var w = wire.GetComponent<Wire>();
            w.SetSender(start);
            w.SetReceiver(end);
            w.Render();

            return w;
        }

        public static Wire CreateDynamic(Vector3 start, Vector3 end, Color color)
        {
            if (wirePreset == null)
                wirePreset = Resources.Load<LevelObjectData>("LevelObjectData/Environment/Interactive/Wire");

            var wire = LevelManager.currentLevel.AddDynamic(wirePreset, new Vector3(0, 0, 0),
                new Quaternion(0, 0, 0, 0));
            var w = wire.GetComponent<Wire>();
            w.SetSenderPosition(start);
            w.SetReceiverPosition(end);
            //w.Render();

            return w;
        }


        public void Render()
        {
            var mesh = new Mesh();
            lr.SetWidth(1, 1);
            lr.BakeMesh(mesh, true);
            lr.SetWidth(0.4f, 0.4f);
            mc.sharedMesh = mesh;
        }

        public override void OnStartRound()
        {
            base.OnStartRound();
            SetWire(false);
        }

        public override void OnEndRound()
        {
            base.OnEndRound();
            SetWire(true);
        }

        private void SetWire(bool v)
        {
            lr.enabled = v;
            A.gameObject.SetActive(v);
            B.gameObject.SetActive(v);
            mc.enabled = v;
            enabled = v;
        }

        public void SetSenderPosition(Vector3 s)
        {
            start = s;
            lr.SetPosition(0, s);
            A.position = s;
        }

        public void SetReceiverPosition(Vector3 r)
        {
            for (var i = 1; i < numberOfPoints; i++)
            {
                var t = i / (float) (numberOfPoints - 1);
                var pos = (1 - t) * start + t * r;
                Debug.Log(pos);
                lr.SetPosition(i, pos);
            }

            B.position = r;
        }

        public void SetSender(InteractiveLevelObject s)
        {
            sender = s;
            SetSenderPosition(s.transform.position);
        }

        public void SetReceiver(InteractiveLevelObject r)
        {
            receiver = r;
            SetReceiverPosition(r.transform.position);
        }
    }
}