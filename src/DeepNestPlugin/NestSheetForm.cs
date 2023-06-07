using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NestingOpenSource
{
    public partial class NestSheetForm : Form
    {
        public NestSheetForm()
        {
            InitializeComponent();
        }

        private void NestSheetForm_Load(object sender, EventArgs e)
        {
            Rhino.RhinoDoc.SelectObjects += ShowSelectedParts;
            Rhino.RhinoDoc.DeselectAllObjects += HideSelectedParts;
        }

        private void ShowSelectedParts(object sender, EventArgs e)
        {
            var doc = RhinoDoc.ActiveDoc;
            var partCounts = new SortedDictionary<string, int>();
            var objs = doc.Objects.GetSelectedObjects(false, false)
                .Where(obj => obj.ObjectType == ObjectType.InstanceReference)
                .Select(obj => obj as InstanceObject)
                .ToList();
            foreach (var obj in objs)
            {
                if (partCounts.ContainsKey(obj.InstanceDefinition.Name))
                {
                    partCounts[obj.InstanceDefinition.Name]++;
                }
                else
                {
                    partCounts.Add(obj.InstanceDefinition.Name, 1);
                }
            }

            dataGridView1.Rows.Clear();
            foreach (var part in partCounts)
            {
                dataGridView1.Rows.Add(new string[] { part.Key, part.Value.ToString() });
            }

        }

        private void HideSelectedParts(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
