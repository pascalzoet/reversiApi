#pragma checksum "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Auth\Register.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a9b064e969a9cec3b516886549d2bf3fee2bb90c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auth_Register), @"mvc.1.0.view", @"/Views/Auth/Register.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Auth/Register.cshtml", typeof(AspNetCore.Views_Auth_Register))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a9b064e969a9cec3b516886549d2bf3fee2bb90c", @"/Views/Auth/Register.cshtml")]
    public class Views_Auth_Register : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Auth\Register.cshtml"
  
    Layout = "_Layout";
    ViewData["Title"] = "Register";

#line default
#line hidden
            BeginContext(69, 1239, true);
            WriteLiteral(@"
<div class=""row"">
    <div class=""col"">
        <div class=""card"">
            <div class=""card-header"">Registreren</div>
            <div class=""card-body"">
                <form action=""/register"" method=""post"">
                    <div class=""form-group"">
                        <label for=""Gebruikersnaam"">Gebruikersnaam</label>
                        <input type=""text"" class=""form-control"" id=""Gebruikersnaam"" aria-describedby=""usernameHelp"" placeholder=""Gebruikersnaam"" name=""UserName"">
                    </div>
                    <div class=""form-group"">
                        <label for=""email"">Email</label>
                        <input type=""email"" class=""form-control"" id=""email"" aria-describedby=""emailHelp"" placeholder=""Email"" name=""Email"">
                    </div>
                    <div class=""form-group"">
                        <label for=""password"">Password</label>
                        <input type=""password"" class=""form-control"" name=""Password"" id=""password"" placehold");
            WriteLiteral("er=\"Password\">\r\n                    </div>\r\n                    <input type=\"submit\" class=\"btn btn-primary\" value=\"registreer\">\r\n                </form>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n</div>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
