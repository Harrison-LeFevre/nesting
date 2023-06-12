using DeepNestLib;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFrameUtilitiesLibrary.Geometry;
//using XFrameUtilitiesLibrary.Types;

namespace NestingOpenSource
{
    public class NestSheet
    {
        public List<(string, Curve)> PartCurves { get; set; }
        public int SheetWidth { get; set; }
        public int SheetLength { get; set; }

        NestingContext context = new NestingContext();
        Thread th;

        List<NFP> sheets { get { return context.Sheets; } }
        List<NFP> polygons { get { return context.Polygons; } }
        public SvgNest nest { get { return context.Nest; } }

        bool stop = false;

        public NestSheet(List<(string, Curve)> partCurves, int sheetWidth, int sheetLength)
        {
            PartCurves = partCurves;
            SheetWidth = sheetWidth;
            SheetLength = sheetLength;
        }
        public static List<Curve> RemoveArcsNotSimplified(Curve curve)
        {
            var lineSegements = new List<Curve>();

            var curveSegments = curve.DuplicateSegments();
            foreach (Curve segment in curveSegments)
            {
                if (segment.IsLinear(10))
                {
                    lineSegements.Add(segment);
                }
            }
            return lineSegements;

        }

        public void Nest(ProgressBar progressBar, Label label, bool useBoundingBoxes, bool useMultipleSheets)
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

                //// Cleanup and Simplify Curve
                var doc = RhinoDoc.ActiveDoc;
                Curve simpleCurve = crv.Item2;//.Simplify(CurveSimplifyOptions.All, doc.ModelAbsoluteTolerance, doc.ModelAngleToleranceRadians);
                List<Curve> curvesWithoutArcs = RemoveArcsNotSimplified(simpleCurve);
                Point3d[] arcIntersections = CurveHelpers.GetArcTangentIntersections(simpleCurve, Plane.WorldXY);
                Point3d[] points = CurveHelpers.GetCurveStartEndPoints(curvesWithoutArcs).ToArray();

                //// Create Simplified Curve
                var allPoints = points.ToList();
                allPoints.AddRange(arcIntersections);
                points = Point3d.CullDuplicates(allPoints, 0.01);
                List<Point3d> crvPoints = PointSort.AlongCurve(points, simpleCurve).ToList();
                crvPoints.Add(crvPoints[0]);

                if (useBoundingBoxes)
                {
                    var bBox = simpleCurve.GetBoundingBox(true);
                    var p1 = bBox.Corner(true, true, true);
                    var p2 = bBox.Corner(false, true, true);
                    var p3 = bBox.Corner(false, false, true);
                    var p4 = bBox.Corner(true, false, true);
                    crvPoints = new List<Point3d>
                    {
                        p1,
                        p2,
                        p3,
                        p4,
                        p1
                    };
                }
                foreach (var point in crvPoints)
                {
                    pl.AddPoint(new SvgPoint(point.X, point.Y));
                }

            }

            List<Sheet> sh = new List<Sheet>();
            var srcAA = context.GetNextSheetSource();

            //sh.Add(NewSheet(SheetLength, SheetWidth));
            if (useMultipleSheets)
            {
                RhinoApp.WriteLine(context.Polygons.Count.ToString());
                double numToAdd = Math.Ceiling(context.Polygons.Count / 25.0);
                if (numToAdd < 1)
                {
                    numToAdd = 1; 
                }
                for (int i = 0; i < numToAdd; i++)
                {
                    sh.Add(NewSheet(SheetLength, SheetWidth));
                }
            }
            else
            {
                sh.Add(NewSheet(SheetLength, SheetWidth));
            }
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
            RunDeepnest(progressBar, label);

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

        public void RunDeepnest(ProgressBar progressBar, Label label)
        {

            if (th == null)
            {
                th = new Thread(() =>
                {
                    context.StartNest();
                    RhinoApp.WriteLine("Starting Nest using {0} parts", context.Polygons.Count);
                    var sw = new Stopwatch();
                    sw.Start();

                    string curFit = "0.0";
                    while (true)
                    {
                        context.NestIterate();
                        if (!curFit.Equals(context.Current.fitness.ToString()))
                        {
                            curFit = context.Current.fitness.ToString();
                            RhinoApp.WriteLine("Iteration: {0} | Fitness: {1}", context.Iterations, context.Current.fitness.ToString());
                        }
                        MethodInvoker methodInvokerDelegate = delegate () 
                        { 
                            progressBar.Value += 1;
                            label.Text = string.Format("Iteration: {0}", context.Iterations);
                        } ;
                        if (progressBar.InvokeRequired)
                        {
                            progressBar.Invoke(methodInvokerDelegate);
                        }


                        if (context.Iterations % 500 == 0)
                        {
                            var doc = RhinoDoc.ActiveDoc;
                            var ids = AddPartsToDocument(doc, context, PartCurves);
                            doc.Views.Redraw();
                            var msg = new StringBuilder();
                            msg.AppendLine("Continue Nesting?");
                            msg.AppendLine(string.Format("{0}/{1} parts have been nested", context.PlacedPartsCount, context.Polygons.Count));
                            var result = MessageBox.Show(msg.ToString(), "Continue Nesting", MessageBoxButtons.YesNo);
                            MethodInvoker methodInvokerDelegateResetBar = delegate () { progressBar.Value = 0; } ;
                            if (progressBar.InvokeRequired)
                            {
                                progressBar.Invoke(methodInvokerDelegateResetBar);
                            }
                            if (result == DialogResult.Yes)
                            {
                                RemovePartsFromDocument(doc, ids);
                                doc.Views.Redraw();
                            }
                            else if (result == DialogResult.No)
                            {
                                stop = true;
                            }
                        }
                        if (stop) break;
                    }
                    th.Abort();
                    th = null;
                });

                th.IsBackground = true;
                th.Start();
            }
        }

        private static List<Guid> AddPartsToDocument(RhinoDoc doc, NestingContext context, List<(string, Curve)> PartCurves)
        {
            List<Guid> result = new List<Guid>();
            var stockLayer = doc.Layers.FindName("SHEETS_STOCK");
            foreach (var item in context.Sheets)
            {
                var points = new List<Point3d>();
                foreach (var point in item.Points)
                {
                    points.Add(new Point3d(point.x, point.y, 0));
                }
                points.Add(new Point3d(item.Points[0].x, item.Points[0].y, 0));
                var objAtts = new ObjectAttributes() { LayerIndex = stockLayer.Index };
                var id = doc.Objects.AddPolyline(points, objAtts);
                id = doc.Objects.Transform(id, Transform.Translation(new Vector3d(item.x, item.y, 0)), true);
                result.Add(id);
            }

            int j = 0;
            foreach (var part in PartCurves)
            {
                var iDef = doc.InstanceDefinitions.Find(part.Item1);
                var curve = context.Polygons[j];
                //var RhinoCurve = new Polyline();
                //foreach (var point in curve.Points)
                //{
                //    RhinoCurve.Add(new Point3d(point.x, point.y, 0));
                //}
                //var polyCurve = RhinoCurve.ToPolylineCurve();
                //var curveID = doc.Objects.AddCurve(polyCurve);
                var rotation = Transform.Rotation(curve.rotation * (Math.PI / 180), Point3d.Origin);
                var translation = Transform.Translation(new Vector3d(curve.x, curve.y, 0));
                //var Id = doc.Objects.Transform(curveID, rotation, true);
                //var finalId = doc.Objects.Transform(Id, translation, true);
                var objId = doc.Objects.AddInstanceObject(iDef.Index, rotation);
                var id = doc.Objects.Transform(objId, translation, true);
                result.Add(id);
                //result.Add(finalId);
                j++;
            }

            return result;
        }

        private static void RemovePartsFromDocument(RhinoDoc doc, List<Guid> guids)
        {
            doc.Objects.Delete(guids, true);
        }

        public static List<(string, Curve)> GetBlockBoundaryCurves(RhinoDoc doc, SortedDictionary<string, int> partNameAndCounts)
        {
            var curves = new List<(string, Curve)>(); 
            var boundaryLayer = doc.Layers.FindName("OUT_10_FULL");
            foreach (var part in partNameAndCounts)
            {
                var iDef = doc.InstanceDefinitions.Find(part.Key);
                var boundaryCurve = iDef.GetObjects().First(curve => curve.Attributes.LayerIndex == boundaryLayer.Index);
                var curveGeo = boundaryCurve.Geometry as Curve;
                for (int i = 0; i < part.Value; i++)
                {
                    var dupCurve = curveGeo.DuplicateCurve();
                    curves.Add((iDef.Name, dupCurve));
                }
            }
            return curves;
        }
    }
}
