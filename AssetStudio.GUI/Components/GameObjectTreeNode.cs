using System.Windows.Forms;

namespace AssetStudio.GUI
{
    public class GameObjectTreeNode : TreeNode
    {
        public GameObject gameObject;

        public GameObjectTreeNode(GameObject gameObject)
        {
            this.gameObject = gameObject;
            Text = $"{gameObject.m_Name} ({gameObject.m_PathID})";
            if (gameObject.HasModel())
            {
                BackColor = System.Drawing.Color.LightBlue;
            }
        }

    }
}
