using System.Collections.Generic;

namespace UnityEngine.AI
{
    [ExecuteInEditMode]
    [AddComponentMenu("Navigation/NavMeshModifier", 32)]
    [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
    public class NavMeshModifier : MonoBehaviour
    {
        [SerializeField] private bool m_OverrideArea;

        [SerializeField] private int m_Area;

        [SerializeField] private bool m_IgnoreFromBuild;

        // List of agent types the modifier is applied for.
        // Special values: empty == None, m_AffectedAgents[0] =-1 == All.
        [SerializeField] private List<int> m_AffectedAgents = new List<int>(new[] {-1}); // Default value is All

        public bool overrideArea
        {
            get => m_OverrideArea;
            set => m_OverrideArea = value;
        }

        public int area
        {
            get => m_Area;
            set => m_Area = value;
        }

        public bool ignoreFromBuild
        {
            get => m_IgnoreFromBuild;
            set => m_IgnoreFromBuild = value;
        }

        public static List<NavMeshModifier> activeModifiers { get; } = new List<NavMeshModifier>();

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