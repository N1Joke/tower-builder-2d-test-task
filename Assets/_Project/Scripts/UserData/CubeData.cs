using Newtonsoft.Json;
using UnityEngine;

namespace Assets._Project.Scripts.UserData
{
    [System.Serializable]
    public class CubeData
    {
        public int id;
        public float x;
        public float y;
        public float z;

        [JsonIgnore]
        public Vector3 Position
        {
            get { return new Vector3(x, y, z); }
            set { x = value.x; y = value.y; z = value.z; }
        }
    }
}
