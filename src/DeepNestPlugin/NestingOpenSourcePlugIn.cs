using System;
using Rhino.PlugIns;
using RMA.UI;

namespace NestingOpenSource
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class NestingOpenSourcePlugIn : Rhino.PlugIns.PlugIn
    {
        //private MRhinoUiDockBar UIContainer = null;
        public NestingOpenSourcePlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the NestingOpenSourcePlugIn plug-in.</summary>
        public static NestingOpenSourcePlugIn Instance
        {
            get; private set;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            //MRhinoDockBarManager.CreateRhinoDockBar(
            //    this, UIContainer,
            //    true,
            //    MRhinoUiDockBar.DockLocation.floating,
            //    MRhinoUiDockBar.DockStyle.any,
            //    new System.Drawing.Point(300, 300));

            return base.OnLoad(ref errorMessage);
        }
    }
}