using System.Collections.Generic;

namespace UnityEngine.AI
{
    [ExecuteInEditMode]
    [AddComponentMenu("Navigation/NavMeshModifierVolume", 31)]
    [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
    public class NavMeshModifierVolume : MonoBehaviour
    {
        [SerializeField] private Vector3 m_Size = new Vector3(4.0f, 3.0f, 4.0f);

        [SerializeField] private Vector3 m_Center = new Vector3(0, 1.0f, 0);

        [SerializeField] private int m_Area;

        // List of agent types the modifier is applied for.
        // Special values: empty == None, m_AffectedAgents[0] =-1 == All.
        [SerializeField] private List<int> m_AffectedAgents = new List<int>(new[] {-1}); // Default value is All

        public Vector3 size
        {
            get => m_Size;
            set => m_Size = value;
        }

        public Vector3 center
        {
            get => m_Center;
            set => m_Center = value;
        }

        public int area
        {
            get => m_Area;
            set => m_Area = value;
        }

        public static List<NavMeshModifierVolume> activeModifiers { get; } = new List<NavMeshModifierVolume>();

        private void OnEnable()
        {
            if (!activeModifiers.Contains(this))
                activeModifiers.Add(this);
        }

        private void OnDisable()
        {
            activeModifiers.Remove(this);
        }

        public bool AffectsAgentType(int agentTypeID)
        {
            if (m_AffectedAgents.Count == 0)
                return false;
            if (m_AffectedAgents[0] == -1)
                return true;
            return m_AffectedAgents.IndexOf(agentTypeID) != -1;
        }
    }
}