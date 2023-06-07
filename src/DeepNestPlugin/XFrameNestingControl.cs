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
    [System.Runtime.InteropServices.Guid("8F60D623-CD07-4F02-9AFA-90EB4CC8188C")]
    public partial class XFrameNestingControl : UserControl
    {
        public XFrameNestingControl()
        {
            InitializeComponent();
            this.Load += new EventHandler(SetRhinoCommands);
        }

        private void SetRhinoCommands(object sender, EventArgs e)
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

        private void NestPartsButton_Click(object sender, EventArgs e)
        {
            var doc = RhinoDoc.ActiveDoc;
            var parts = new SortedDictionary<string, int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    continue;
                }
                parts.Add(row.Cells[0].Value.ToString(), int.TryParse(row.Cells[1].Value.ToString(), out int result) ? result : -1);
            }

            var curves = NestSheet.GetBlockBoundaryCurves(doc, parts);
            var nest = new NestSheet(curves, 1200, 2400);
            nest.Nest();
            doc.Objects.UnselectAll();
            doc.Views.Redraw();
        }
    }
}
