
namespace UCM.IAV.Navegacion
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ColaPrioridad
    {
        private ArrayList nodes = new ArrayList();
        public int Length
        {
            get { return this.nodes.Count; }
        }
        public bool Contains(object node)
        {
            return this.nodes.Contains(node);
        }
        public Nodo First()
        {
            if (this.nodes.Count > 0)
            {
                return (Nodo)this.nodes[0];
            }
            return null;
        }
        public void Push(Nodo node)
        {
            this.nodes.Add(node);
            this.nodes.Sort();
        }
        public void Remove(Nodo node)
        {
            this.nodes.Remove(node);
            //Ensure the list is sorted
            this.nodes.Sort();
        }
    }

}
