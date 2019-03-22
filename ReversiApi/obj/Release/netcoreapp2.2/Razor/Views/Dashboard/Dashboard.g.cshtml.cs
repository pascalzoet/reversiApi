#pragma checksum "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "438eba7316018d7eca09c2d795412841a69559f9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_Dashboard), @"mvc.1.0.view", @"/Views/Dashboard/Dashboard.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Dashboard/Dashboard.cshtml", typeof(AspNetCore.Views_Dashboard_Dashboard))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"438eba7316018d7eca09c2d795412841a69559f9", @"/Views/Dashboard/Dashboard.cshtml")]
    public class Views_Dashboard_Dashboard : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ReversiApi.Models.Game>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
  
    Layout = "_Layout";
    var games = (List<ReversiApi.Models.Game>) ViewData["games"];

#line default
#line hidden
#line 6 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
  
    ViewData["Title"] = "Dashboard";

#line default
#line hidden
            BeginContext(175, 225, true);
            WriteLiteral("<div class=\"row\">\r\n    <div class=\"col-md-auto\">\r\n        <div class=\"card\" style=\"width: 18rem;\">\r\n            <div class=\"card-header\">\r\n                Mijn games\r\n            </div>\r\n            <div class=\"list-group\">\r\n");
            EndContext();
#line 16 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                 if (Model != null)
                {

#line default
#line hidden
            BeginContext(456, 18, true);
            WriteLiteral("                <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 474, "\"", 509, 2);
            WriteAttributeValue("", 481, "/game/enter/", 481, 12, true);
#line 18 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
WriteAttributeValue("", 493, Model.GameToken, 493, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(510, 193, true);
            WriteLiteral(" class=\"list-group-item list-group-item-action flex-column align-items-start\">\r\n                    <div class=\"d-flex w-100 justify-content-between\">\r\n                        <h5 class=\"mb-1\">");
            EndContext();
            BeginContext(704, 10, false);
#line 20 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                                    Write(Model.Name);

#line default
#line hidden
            EndContext();
            BeginContext(714, 71, true);
            WriteLiteral("</h5>\r\n                    </div>\r\n                    <p class=\"mb-1\">");
            EndContext();
            BeginContext(786, 17, false);
#line 22 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                               Write(Model.Description);

#line default
#line hidden
            EndContext();
            BeginContext(803, 28, true);
            WriteLiteral("</p>\r\n                    <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 831, "\"", 867, 2);
            WriteAttributeValue("", 838, "/game/remove/", 838, 13, true);
#line 23 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
WriteAttributeValue("", 851, Model.GameToken, 851, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(868, 38, true);
            WriteLiteral(">Verwijder</a>\r\n                </a>\r\n");
            EndContext();
#line 25 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                }

#line default
#line hidden
            BeginContext(925, 254, true);
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-auto\">\r\n        <div class=\"card\" style=\"width: 18rem;\">\r\n            <div class=\"card-header\">\r\n                Open games\r\n            </div>\r\n            <div class=\"list-group\">\r\n");
            EndContext();
#line 35 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                 foreach (var game in @games)
                {

#line default
#line hidden
            BeginContext(1245, 22, true);
            WriteLiteral("                    <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1267, "\"", 1300, 2);
            WriteAttributeValue("", 1274, "/game/join/", 1274, 11, true);
#line 37 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
WriteAttributeValue("", 1285, game.GameToken, 1285, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1301, 201, true);
            WriteLiteral(" class=\"list-group-item list-group-item-action flex-column align-items-start\">\r\n                        <div class=\"d-flex w-100 justify-content-between\">\r\n                            <h5 class=\"mb-1\">");
            EndContext();
            BeginContext(1503, 9, false);
#line 39 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                                        Write(game.Name);

#line default
#line hidden
            EndContext();
            BeginContext(1512, 142, true);
            WriteLiteral("</h5>\r\n                            <small>10 minuten geleden</small>\r\n                        </div>\r\n                        <p class=\"mb-1\">");
            EndContext();
            BeginContext(1655, 16, false);
#line 42 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                                   Write(game.Description);

#line default
#line hidden
            EndContext();
            BeginContext(1671, 34, true);
            WriteLiteral("</p>\r\n\r\n                    </a>\r\n");
            EndContext();
#line 45 "C:\Users\pascal\source\repos\windesheim\server technology\reversi\ReversiApi\ReversiApi\Views\Dashboard\Dashboard.cshtml"
                }

#line default
#line hidden
            BeginContext(1724, 56, true);
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ReversiApi.Models.Game> Html { get; private set; }
    }
}
#pragma warning restore 1591
