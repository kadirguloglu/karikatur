#pragma checksum "D:\Proje\Karikatur\karikatür\karikatur-web\Views\Home\UnusedPictures.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "132beab9e91f2785d71ed0c5c02614959c117427"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_UnusedPictures), @"mvc.1.0.view", @"/Views/Home/UnusedPictures.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/UnusedPictures.cshtml", typeof(AspNetCore.Views_Home_UnusedPictures))]
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
#line 1 "D:\Proje\Karikatur\karikatür\karikatur-web\Views\_ViewImports.cshtml"
using karikatur_web;

#line default
#line hidden
#line 2 "D:\Proje\Karikatur\karikatür\karikatur-web\Views\_ViewImports.cshtml"
using karikatur_db.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"132beab9e91f2785d71ed0c5c02614959c117427", @"/Views/Home/UnusedPictures.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"094ac281c821da79fcc2dbb35c80d65ed2e02b50", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_UnusedPictures : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<string>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "D:\Proje\Karikatur\karikatür\karikatur-web\Views\Home\UnusedPictures.cshtml"
  
    ViewData["Title"] = "UnusedPictures";

#line default
#line hidden
            BeginContext(71, 163, true);
            WriteLiteral("\r\n<div class=\"row\">\r\n    <a class=\"btn btn-primary\" href=\"/Home/DeleteUnusedPictures\">Kullanılmayan görselleri sil</a>\r\n</div>\r\n<div class=\"row\" id=\"image-list\">\r\n");
            EndContext();
#line 10 "D:\Proje\Karikatur\karikatür\karikatur-web\Views\Home\UnusedPictures.cshtml"
     foreach (var item in Model)
    {

#line default
#line hidden
            BeginContext(275, 177, true);
            WriteLiteral("        <div class=\"col-md-3 p-1 mb-1\">\r\n            <div class=\"border border-primary text-center h-100 d-flex justify-content-center align-items-center\">\r\n                <img");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 452, "\"", 463, 1);
#line 14 "D:\Proje\Karikatur\karikatür\karikatur-web\Views\Home\UnusedPictures.cshtml"
WriteAttributeValue("", 458, item, 458, 5, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(464, 59, true);
            WriteLiteral(" class=\"img-fluid\" />\r\n            </div>\r\n        </div>\r\n");
            EndContext();
#line 17 "D:\Proje\Karikatur\karikatür\karikatur-web\Views\Home\UnusedPictures.cshtml"
    }

#line default
#line hidden
            BeginContext(530, 6, true);
            WriteLiteral("</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<string>> Html { get; private set; }
    }
}
#pragma warning restore 1591
