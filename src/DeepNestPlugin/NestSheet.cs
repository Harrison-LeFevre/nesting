using DeepNestLib;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NestingOpenSource
{
    public class NestSheet
    {
        public List<Curve> PartCurves { get; set; }
        public int SheetWidth { get; set; }
        public int SheetLength { get; set; }

        NestingContext context = new NestingContext();
        Thread th;

        List<NFP> sheets { get { return context.Sheets; } }
        List<NFP> polygons { get { return context.Polygons; } }
        public SvgNest nest { get { return context.Nest; } }

        bool stop = false;

        public NestSheet(List<Curve> partCurves, int sheetWidth, int sheetLength)
        {
            PartCurves = partCurves;
            SheetWidth = sheetWidth;
            SheetLength = sheetLength;
        }

        public void Nest()
        {
            foreach (var crv in PartCurves)
            {
                NFP pl = new NFP();
                int src = 0;
                if (polygons.Any())
                {
                    src = polygons.Max(z => z.source.Value) + 1;
                }
                polygons.Add(pl);
                pl.source = src;
                foreach (var point in crv.ToPolyline(0.01, 0.01, 10, 5000).ToPolyline())
                {
                    pl.AddPoint(new SvgPoint(point.X, point.Y));
                }

            }

            List<Sheet> sh = new List<Sheet>();
            var srcAA = context.GetNextSheetSource();

            sh.Add(NewSheet(SheetLength, SheetWidth));
            foreach (var item in sh)
            {
                item.source = srcAA;
                context.Sheets.Add(item);
            }


            if (sheets.Count == 0 || polygons.Count == 0)
            {
                MessageBox.Show("There are no sheets or parts");
            }
            stop = false;
            context.ReorderSheets();
            RunDeepnest();

        }

        public Sheet NewSheet(int w = 2400, int h = 1200)
        {
            var tt = new RectangleSheet();
            tt.Name = "rectSheet" + (sheets.Count + 1);
            tt.Height = h;
            tt.Width = w;
            tt.Rebuild();

            return tt;
        }

        public void RunDeepnest()
        {

            if (th == null)
            {
                th = new Thread(() =>
                {
                    context.StartNest();
                    var sw = new Stopwatch();
                    sw.Start();

                    string curFit = "0.0";
                    int i = 0;
                    while (true)
                    {
                        context.NestIterate();
                        if (!curFit.Equals(context.Current.fitness.ToString()))
                        {
                            curFit = context.Current.fitness.ToString();
                            RhinoApp.WriteLine("Iteration: {0} | Fitness: {1}", context.Iterations, context.Current.fitness.ToString());
                        }

                        if (context.Iterations == 2500)
                        {
                            RhinoApp.WriteLine("Finished");
                            stop = true;
                            var doc = RhinoDoc.ActiveDoc;
                            foreach (var item in context.Sheets)
                            {
                                var points = new List<Point3d>();
                                foreach (var point in item.Points)
                                {
                                    points.Add(new Point3d(point.x, point.y, 0));
                                }
                                doc.Objects.AddPolyline(points);
                            }

                            foreach (var part in context.Polygons)
                            {
                                var points = new List<Point3d>();
                                foreach (var point in part.Points)
                                {
                                    points.Add(new Point3d(point.x, point.y, 0));
                                }
                                var polyline = new Polyline(points);
                                var crv = polyline.ToPolylineCurve();
                                crv.MakeClosed(0.01);
                                crv.Rotate(part.rotation * (Math.PI / 180), Vector3d.ZAxis, Point3d.Origin);
                                crv.Translate(new Vector3d(part.x, part.y, 0));
                                doc.Objects.AddCurve(crv);
                            }
                            RhinoApp.WriteLine("Material Utilization: {0}", context.MaterialUtilization.ToString());
                            RhinoApp.WriteLine("Total Time: {0}s", Math.Round(sw.Elapsed.TotalSeconds));
                            doc.Views.Redraw();
                            sw.Stop();
                        }
                        i++;
                        if (stop) break;
                    }
                    th = null;
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        public static List<Curve> GetBlockBoundaryCurves(RhinoDoc doc, SortedDictionary<string, int> partNameAndCounts)
        {
            var curves = new List<Curve>();
            var boundaryLayer = doc.Layers.FindName("OUT_10_FULL");
            foreach (var part in partNameAndCounts)
            {
                var iDef = doc.InstanceDefinitions.Find(part.Key);
                var boundaryCurve = iDef.GetObjects().First(curve => curve.Attributes.LayerIndex == boundaryLayer.Index);
                var curveGeo = boundaryCurve.Geometry as Curve;
                for (int i = 0; i < part.Value; i++)
                {
                    var dupCurve = curveGeo.DuplicateCurve();
                    curves.Add(dupCurve);
                }
            }
            return curves;
        }
    }
}
