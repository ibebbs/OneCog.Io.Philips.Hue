﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneCog.Io.Philips.Hue.Tests {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OneCog.Io.Philips.Hue.Tests.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///	&quot;lights&quot;: {
        ///		&quot;1&quot;: {
        ///			&quot;state&quot;: {
        ///				&quot;on&quot;: false,
        ///				&quot;bri&quot;: 0,
        ///				&quot;hue&quot;: 0,
        ///				&quot;sat&quot;: 0,
        ///				&quot;xy&quot;: [0.0000, 0.0000],
        ///				&quot;ct&quot;: 0,
        ///				&quot;alert&quot;: &quot;none&quot;,
        ///				&quot;effect&quot;: &quot;none&quot;,
        ///				&quot;colormode&quot;: &quot;hs&quot;,
        ///				&quot;reachable&quot;: true
        ///			},
        ///			&quot;type&quot;: &quot;Extended color light&quot;,
        ///			&quot;name&quot;: &quot;Hue Lamp 1&quot;,
        ///			&quot;modelid&quot;: &quot;LCT001&quot;,
        ///			&quot;swversion&quot;: &quot;65003148&quot;,
        ///			&quot;pointsymbol&quot;: {
        ///				&quot;1&quot;: &quot;none&quot;,
        ///				&quot;2&quot;: &quot;none&quot;,
        ///				&quot;3&quot;: &quot;none&quot;,
        ///				&quot;4&quot;: &quot;none&quot;,
        ///				&quot;5&quot;: &quot;none&quot;,
        ///				&quot;6&quot;: &quot;none&quot;,
        ///				&quot;7&quot;: &quot;none&quot;,
        ///		 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ConfigurationResponse {
            get {
                return ResourceManager.GetString("ConfigurationResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///	{&quot;success&quot;:{&quot;/lights/1/state/bri&quot;:200}},
        ///	{&quot;success&quot;:{&quot;/lights/1/state/on&quot;:true}},
        ///	{&quot;success&quot;:{&quot;/lights/1/state/hue&quot;:50000}}
        ///].
        /// </summary>
        internal static string SetLightResponse {
            get {
                return ResourceManager.GetString("SetLightResponse", resourceCulture);
            }
        }
    }
}
