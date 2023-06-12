using System;
using Rhino;
using Rhino.Commands;
using Rhino.UI;
using RMA.UI;

namespace NestingOpenSource
{
    public class XFrameNesting : Command
    {
        public XFrameNesting()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static XFrameNesting Instance { get; private set; }

        public override string EnglishName => "XFrameNesting";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var UIContainer = new MRhinoUiDockBar(Guid.NewGuid()/*new Guid("218100D1-3558-42F5-908A-5D74F2E9C975")*/, "XFrame Nesting", new XFrameNestingControl());
            UIContainer.SetInitialSizeFloating(new System.Drawing.Size(303, 620));
            MRhinoDockBarManager.CreateRhinoDockBar(
                NestingOpenSourcePlugIn.Instance, UIContainer,
                true,
                MRhinoUiDockBar.DockLocation.floating,
                MRhinoUiDockBar.DockStyle.any,
                new System.Drawing.Point(300, 300));
            return Result.Success;
        }
    }
}