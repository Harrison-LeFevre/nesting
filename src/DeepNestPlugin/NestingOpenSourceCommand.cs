using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Windows.Controls;
using DeepNestLib;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Command = Rhino.Commands.Command;
using Result = Rhino.Commands.Result;

namespace NestingOpenSource
{
    public class NestingOpenSourceCommand : Command
    {
        public NestingOpenSourceCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static NestingOpenSourceCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "Nesting"; }
        }

        NestingContext context = new NestingContext();
        Thread th;

        List<NFP> sheets { get { return context.Sheets; } }
        List<NFP> polygons { get { return context.Polygons; } }
        public SvgNest nest { get { return context.Nest; } }

        bool stop = false;
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            //  var cnt = GetCountFromDialog();
            //Random r = new Random();
            //for (int i = 0; i < 50; i++)
            //{
            //    var xx = r.Next(2000) + 100;
            //    var yy = r.Next(2000);
            //    var ww = r.Next(600) + 10;
            //    var hh = r.Next(600) + 5;
            //    NFP pl = new NFP();
            //    int src = 0;
            //    if (polygons.Any())
            //    {
            //        src = polygons.Max(z => z.source.Value) + 1;
            //    }
            //    polygons.Add(pl);
            //    pl.source = src;
            //    pl.x = xx;
            //    pl.y = yy;
            //    pl.Points = new SvgPoint[] { };
            //    pl.AddPoint(new SvgPoint(0, 0));
            //    pl.AddPoint(new SvgPoint(ww, 0));
            //    pl.AddPoint(new SvgPoint(ww, hh));
            //    pl.AddPoint(new SvgPoint(0, hh));
            //    pl.AddPoint(new SvgPoint(0, 0));
            //}

            var rDoc = RhinoDoc.ActiveDoc;
            var objs = doc.Objects.GetSelectedObjects(false, false).Where(obj => obj.ObjectType == Rhino.DocObjects.ObjectType.Curve).ToList();
            foreach (var obj in objs)
            {
                var crv = obj.Geometry as Curve;
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
            // UpdateList();


            List<Sheet> sh = new List<Sheet>();
            var srcAA = context.GetNextSheetSource();

            sh.Add(NewSheet(2400, 1200));
            foreach (var item in sh)
            {
                item.source = srcAA;
                context.Sheets.Add(item);
            }


            if (sheets.Count == 0 || polygons.Count == 0)
            {
                MessageBox.Show("There are no sheets or parts",  MessageBoxButtons.OK);
                return Result.Success; ;
            }
            stop = false;
          //  progressBar1.Value = 0;
          //  tabControl1.SelectedTab = tabPage4;
            context.ReorderSheets();
            RunDeepnest();

            return Result.Success;
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
                    //Background.displayProgress = displayProgress;

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

        internal void displayProgress(float progress)
        {
            RhinoApp.WriteLine(progress.ToString());
            progressVal = progress;

        }
        public float progressVal = 0;
        public void UpdateNestsList()
        {
            //if (nest != null)
            //{
            //    listView4.Invoke((Action)(() =>
            //    {
            //        listView4.BeginUpdate();
            //        listView4.Items.Clear();
            //        foreach (var item in nest.nests)
            //        {
            //            listView4.Items.Add(new ListViewItem(new string[] { item.fitness + "" }) { Tag = item });
            //        }
            //        listView4.EndUpdate();
            //    }));
            //}
        }




    }
}
