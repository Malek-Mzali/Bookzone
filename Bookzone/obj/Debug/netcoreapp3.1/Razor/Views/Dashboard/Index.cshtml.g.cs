#pragma checksum "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eaa07ae4ce0112ace33e433ca3e1e64f3ac0a900"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_Index), @"mvc.1.0.view", @"/Views/Dashboard/Index.cshtml")]
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
#nullable restore
#line 1 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\_ViewImports.cshtml"
using Ebook;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\_ViewImports.cshtml"
using Ebook.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\_ViewImports.cshtml"
using Ebook.Models.Entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Hosting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
using Ebook.Models.Entity.Document;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"eaa07ae4ce0112ace33e433ca3e1e64f3ac0a900", @"/Views/Dashboard/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e41db38901caa7ad4a8a3e952bc38543ed5da1b6", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Dashboard_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<global::Bookzone.Models.Entity.Statistics.Statistics>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Dashboard/js/plugins/chartjs.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Dashboard/js/plugins/threejs.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Dashboard/js/plugins/orbit-controls.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/Dashboard/Home/Index.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
  
    ViewBag.Title = "Dashboard";
    Layout = "_DashboardLayout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral(@"

<div class=""container-fluid py-4"">
<div class=""row"">
<div class=""col-lg-7 position-relative z-index-2"">
<div class=""card card-plain mb-4"">
  <div class=""card-body p-3"">
    <div class=""row"">
      <div class=""col-lg-6"">
        <div class=""d-flex flex-column h-100"">
          <h2 class=""font-weight-bolder mb-0"">General Statistics</h2>
        </div>
      </div>
    </div>
  </div>
</div>
  <div class=""row"">
    <div class=""col-lg-5 col-sm-6"">
");
#nullable restore
#line 27 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
           if (!User.IsInRole("Editor"))
          {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <div class=""card  mb-4"">
              <div class=""card-body p-3"">
                <div class=""row"">
                  <div class=""col-8"">
                    <div class=""numbers"">
                      <p class=""text-sm mb-0 text-capitalize font-weight-bold"">Total Users</p>
                      <h5 class=""font-weight-bolder mb-0"" id=""TotalDocuments"">
                        ");
#nullable restore
#line (36,26)-(36,42) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TotalUsers);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

                      </h5>
                    </div>
                  </div>
                  <div class=""col-4 text-end"">
                    <div class=""icon icon-shape bg-gradient-primary shadow text-center border-radius-md"">
                      <i class=""fa fa-users text-lg opacity-10"" aria-hidden=""true""></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
");
#nullable restore
#line 49 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"

          }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"      <div class=""card "">
        <div class=""card-body p-3"">
          <div class=""row"">
            <div class=""col-8"">
              <div class=""numbers"">
                <p class=""text-sm mb-0 text-capitalize font-weight-bold"">Total Sales</p>
                <h5 class=""font-weight-bolder mb-0"">
                  ");
#nullable restore
#line (58,20)-(58,36) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TotalSales);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                </h5>
              </div>
            </div>
            <div class=""col-4 text-end"">
              <div class=""icon icon-shape bg-gradient-primary shadow text-center border-radius-md"">
                <i class=""ni ni-cart text-lg opacity-10"" aria-hidden=""true""></i>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class=""col-lg-5 col-sm-6 mt-sm-0 mt-4"">
");
#nullable restore
#line 72 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
                 if (!User.IsInRole("Editor"))
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                  <div class=""card  mb-4"">
                    <div class=""card-body p-3"">
                      <div class=""row"">
                        <div class=""col-8"">
                          <div class=""numbers"">
                            <p class=""text-sm mb-0 text-capitalize font-weight-bold"">Today's Users</p>
                            <h5 class=""font-weight-bolder mb-0"">
                              ");
#nullable restore
#line (81,32)-(81,48) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TodayUsers);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                              <span class=\"text-success text-sm font-weight-bolder\">+");
#nullable restore
#line (82,88)-(82,170) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write((float) Math.Round((double) Model.TodayUsers / (double) Model.TotalUsers, 2) * 100);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"%</span>

                            </h5>
                          </div>
                        </div>
                        <div class=""col-4 text-end"">
                          <div class=""icon icon-shape bg-gradient-primary shadow text-center border-radius-md"">
                            <i class=""fa fa-neuter text-lg opacity-10"" aria-hidden=""true""></i>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
");
#nullable restore
#line 95 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"

                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"      <div class=""card "">
        <div class=""card-body p-3"">
          <div class=""row"">
            <div class=""col-8"">
              <div class=""numbers"">
                <p class=""text-sm mb-0 text-capitalize font-weight-bold"">Today's Sales</p>
                <h5 class=""font-weight-bolder mb-0"">
                  ");
#nullable restore
#line (104,20)-(104,36) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TodaySales);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                  <span class=\"text-success text-sm font-weight-bolder\">+");
#nullable restore
#line (105,76)-(105,265) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(!float.IsNaN(((float) Math.Round((double) Model.TodaySales / (double) Model.TotalSales, 2) * 100)) ? ((float) Math.Round((double) Model.TodaySales / (double) Model.TotalSales, 2) * 100) : 0);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"%</span>
                </h5>
              </div>
            </div>
            <div class=""col-4 text-end"">
              <div class=""icon icon-shape bg-gradient-primary shadow text-center border-radius-md"">
                <i class=""ni ni-trophy text-lg opacity-10"" aria-hidden=""true""></i>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
<div class=""row mt-4"">
  <div class=""col-12 col-lg-10"">
    <div class=""card "">
      <div class=""card-header pb-0 p-3"">
        <div class=""d-flex justify-content-between"">
          <h6 class=""mb-2"">Most Sold Documents</h6>
        </div>
      </div>
      <div class=""table-responsive"">
        <table class=""table align-items-center "">
          <tbody>

");
#nullable restore
#line 131 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
           foreach (Tuple<int, DocumentInfo> de in Model.TopDocuments)
          {
            var key = de.Item1;
            DocumentInfo value = de.Item2;

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n              <td class=\"w-30\">\r\n                <div class=\"d-flex px-2 py-1 align-items-center\">\r\n                  <div>\r\n                    <img");
            BeginWriteAttribute("src", " src=\"", 5324, "\"", 5374, 2);
            WriteAttributeValue("", 5330, "/img/document/", 5330, 14, true);
#nullable restore
#line (139,45)-(139,75) 30 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
WriteAttributeValue("", 5344, value.DocumentGroup.CoverPage, 5344, 30, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("alt", " alt=\"", 5375, "\"", 5381, 0);
            EndWriteAttribute();
            WriteLiteral(" width=\"35px\">\r\n                  </div>\r\n                  <div class=\"ms-4\">\r\n                    <p class=\"text-xs font-weight-bold mb-0\">Name:</p>\r\n                    <h6 class=\"text-sm mb-0\"><a");
            BeginWriteAttribute("href", " href=\"", 5581, "\"", 5612, 1);
#nullable restore
#line (143,55)-(143,79) 30 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
WriteAttributeValue("", 5588, value.DocumentGroup.Url, 5588, 24, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line (143,82)-(143,115) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(value.DocumentGroup.OriginalTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</a></h6>
                  </div>
                </div>
              </td>
              <td>
                <div class=""text-center"">
                  <p class=""text-xs font-weight-bold mb-0"">Sales:</p>
                  <h6 class=""text-sm mb-0"">");
#nullable restore
#line (150,45)-(150,48) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(key);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                </div>\r\n              </td>\r\n              <td>\r\n                <div class=\"text-center\">\r\n                  <p class=\"text-xs font-weight-bold mb-0\">Price:</p>\r\n                  <h6 class=\"text-sm mb-0\">");
#nullable restore
#line (156,45)-(156,70) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(value.DocumentGroup.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" DT</h6>
                </div>
              </td>
              <td class=""align-middle text-sm"">
                <div class=""col text-center"">
                  <p class=""text-xs font-weight-bold mb-0"">Value:</p>
                  <h6 class=""text-sm mb-0"">");
#nullable restore
#line (162,46)-(162,81) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(value.DocumentGroup.Price*(int) key);

#line default
#line hidden
#nullable disable
            WriteLiteral(" DT</h6>\r\n                </div>\r\n              </td>\r\n            </tr>\r\n");
#nullable restore
#line 166 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"


          }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n          </tbody>\r\n        </table>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n</div>\r\n</div>\r\n<div class=\"row mt-4\">\r\n");
#nullable restore
#line 179 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
   if (!User.IsInRole("Editor"))
  {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"      <div class=""col-lg-6 mb-lg-0 mb-4"">
    <div class=""card z-index-2"">
      <div class=""card-header pb-0"">
        <h6>Visitors overview</h6>
      </div>

      <div class=""card-body p-3"">
        <div class=""bg-gradient-dark border-radius-lg pe-1 mb-3"">
          <div class=""chart"">
            <canvas id=""chart-bars"" class=""chart-canvas"" height=""170"" style=""display: block; box-sizing: border-box; height: 170px; width: 730px;"" width=""730""></canvas>
          </div>
        </div>
        <div class=""container border-radius-lg"">
          <div class=""row"">
            <div class=""col-6 col-md-3 py-3 ps-0"">
              <div class=""d-flex mb-2"">
                <div class=""icon icon-shape icon-xxs shadow border-radius-sm bg-gradient-primary text-center me-2 d-flex align-items-center justify-content-center"">
                  <svg width=""10px"" height=""10px"" viewBox=""0 0 40 44"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"">
                  ");
            WriteLiteral(@"  <title>document</title>
                    <g stroke=""none"" stroke-width=""1"" fill=""none"" fill-rule=""evenodd"">
                      <g transform=""translate(-1870.000000, -591.000000)"" fill=""#FFFFFF"" fill-rule=""nonzero"">
                        <g transform=""translate(1716.000000, 291.000000)"">
                          <g transform=""translate(154.000000, 300.000000)"">
                            <path class=""color-background"" d=""M40,40 L36.3636364,40 L36.3636364,3.63636364 L5.45454545,3.63636364 L5.45454545,0 L38.1818182,0 C39.1854545,0 40,0.814545455 40,1.81818182 L40,40 Z"" opacity=""0.603585379""></path>
                            <path class=""color-background"" d=""M30.9090909,7.27272727 L1.81818182,7.27272727 C0.814545455,7.27272727 0,8.08727273 0,9.09090909 L0,41.8181818 C0,42.8218182 0.814545455,43.6363636 1.81818182,43.6363636 L30.9090909,43.6363636 C31.9127273,43.6363636 32.7272727,42.8218182 32.7272727,41.8181818 L32.7272727,9.09090909 C32.7272727,8.08727273 31.9127273,7.27272727 30.9090909,7.2");
            WriteLiteral(@"7272727 Z M18.1818182,34.5454545 L7.27272727,34.5454545 L7.27272727,30.9090909 L18.1818182,30.9090909 L18.1818182,34.5454545 Z M25.4545455,27.2727273 L7.27272727,27.2727273 L7.27272727,23.6363636 L25.4545455,23.6363636 L25.4545455,27.2727273 Z M25.4545455,20 L7.27272727,20 L7.27272727,16.3636364 L25.4545455,16.3636364 L25.4545455,20 Z""></path>
                          </g>
                        </g>
                      </g>
                    </g>
                  </svg>
                </div>
                <p class=""text-xs mt-1 mb-0 font-weight-bold"">Documents</p>
              </div>
              <h4 class=""font-weight-bolder"">");
#nullable restore
#line (214,47)-(214,67) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TotalDocuments);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
              <div class=""progress w-75 h-auto"">
                <div class=""progress-bar bg-dark w-60"" role=""progressbar"" aria-valuenow=""60"" aria-valuemin=""0"" aria-valuemax=""100""></div>
              </div>
            </div>
            <div class=""col-6 col-md-3 py-3 ps-0"">
              <div class=""d-flex mb-2"">
                <div class=""icon icon-shape icon-xxs shadow border-radius-sm bg-gradient-info text-center me-2 d-flex align-items-center justify-content-center"">
                  <svg width=""10px"" height=""10px"" viewBox=""0 0 40 40"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"">
                    <title>spaceship</title>
                    <g stroke=""none"" stroke-width=""1"" fill=""none"" fill-rule=""evenodd"">
                      <g transform=""translate(-1720.000000, -592.000000)"" fill=""#FFFFFF"" fill-rule=""nonzero"">
                        <g transform=""translate(1716.000000, 291.000000)"">
                          <g transform=""trans");
            WriteLiteral(@"late(4.000000, 301.000000)"">
                            <path class=""color-background"" d=""M39.3,0.706666667 C38.9660984,0.370464027 38.5048767,0.192278529 38.0316667,0.216666667 C14.6516667,1.43666667 6.015,22.2633333 5.93166667,22.4733333 C5.68236407,23.0926189 5.82664679,23.8009159 6.29833333,24.2733333 L15.7266667,33.7016667 C16.2013871,34.1756798 16.9140329,34.3188658 17.535,34.065 C17.7433333,33.98 38.4583333,25.2466667 39.7816667,1.97666667 C39.8087196,1.50414529 39.6335979,1.04240574 39.3,0.706666667 Z M25.69,19.0233333 C24.7367525,19.9768687 23.3029475,20.2622391 22.0572426,19.7463614 C20.8115377,19.2304837 19.9992882,18.0149658 19.9992882,16.6666667 C19.9992882,15.3183676 20.8115377,14.1028496 22.0572426,13.5869719 C23.3029475,13.0710943 24.7367525,13.3564646 25.69,14.31 C26.9912731,15.6116662 26.9912731,17.7216672 25.69,19.0233333 L25.69,19.0233333 Z""></path>
                            <path class=""color-background"" d=""M1.855,31.4066667 C3.05106558,30.2024182 4.79973884,29.7296005 6.43969145,30.");
            WriteLiteral(@"1670277 C8.07964407,30.6044549 9.36054508,31.8853559 9.7979723,33.5253085 C10.2353995,35.1652612 9.76258177,36.9139344 8.55833333,38.11 C6.70666667,39.9616667 0,40 0,40 C0,40 0,33.2566667 1.855,31.4066667 Z""></path>
                            <path class=""color-background"" d=""M17.2616667,3.90166667 C12.4943643,3.07192755 7.62174065,4.61673894 4.20333333,8.04166667 C3.31200265,8.94126033 2.53706177,9.94913142 1.89666667,11.0416667 C1.5109569,11.6966059 1.61721591,12.5295394 2.155,13.0666667 L5.47,16.3833333 C8.55036617,11.4946947 12.5559074,7.25476565 17.2616667,3.90166667 L17.2616667,3.90166667 Z"" opacity=""0.598539807""></path>
                            <path class=""color-background"" d=""M36.0983333,22.7383333 C36.9280725,27.5056357 35.3832611,32.3782594 31.9583333,35.7966667 C31.0587397,36.6879974 30.0508686,37.4629382 28.9583333,38.1033333 C28.3033941,38.4890431 27.4704606,38.3827841 26.9333333,37.845 L23.6166667,34.53 C28.5053053,31.4496338 32.7452344,27.4440926 36.0983333,22.7383333 L36.0983333,22.7383");
            WriteLiteral(@"333 Z"" opacity=""0.598539807""></path>
                          </g>
                        </g>
                      </g>
                    </g>
                  </svg>
                </div>
                <p class=""text-xs mt-1 mb-0 font-weight-bold"">Editors</p>
              </div>
              <h4 class=""font-weight-bolder"">");
#nullable restore
#line (240,47)-(240,65) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TotalEditors);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
              <div class=""progress w-75 h-auto"">
                <div class=""progress-bar bg-dark w-90"" role=""progressbar"" aria-valuenow=""90"" aria-valuemin=""0"" aria-valuemax=""100""></div>
              </div>
            </div>
            <div class=""col-6 col-md-3 py-3 ps-0"">
              <div class=""d-flex mb-2"">
                <div class=""icon icon-shape icon-xxs shadow border-radius-sm bg-gradient-warning text-center me-2 d-flex align-items-center justify-content-center"">
                  <svg");
            BeginWriteAttribute("class", " class=\"", 13405, "\"", 13413, 0);
            EndWriteAttribute();
            WriteLiteral(@" width=""10px"" height=""20px"" viewBox=""0 0 42 42"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"">
                    <title>box-3d-50</title>
                    <g stroke=""none"" stroke-width=""1"" fill=""none"" fill-rule=""evenodd"">
                      <g transform=""translate(-2319.000000, -291.000000)"" fill=""#FFFFFF"" fill-rule=""nonzero"">
                        <g transform=""translate(1716.000000, 291.000000)"">
                          <g transform=""translate(603.000000, 0.000000)"">
                            <path class=""color-background"" d=""M22.7597136,19.3090182 L38.8987031,11.2395234 C39.3926816,10.9925342 39.592906,10.3918611 39.3459167,9.89788265 C39.249157,9.70436312 39.0922432,9.5474453 38.8987261,9.45068056 L20.2741875,0.1378125 L20.2741875,0.1378125 C19.905375,-0.04725 19.469625,-0.04725 19.0995,0.1378125 L3.1011696,8.13815822 C2.60720568,8.38517662 2.40701679,8.98586148 2.6540352,9.4798254 C2.75080129,9.67332903 2.90771305,9.83023153 3.10122239,9.9");
            WriteLiteral(@"269862 L21.8652864,19.3090182 C22.1468139,19.4497819 22.4781861,19.4497819 22.7597136,19.3090182 Z""></path>
                            <path class=""color-background"" d=""M23.625,22.429159 L23.625,39.8805372 C23.625,40.4328219 24.0727153,40.8805372 24.625,40.8805372 C24.7802551,40.8805372 24.9333778,40.8443874 25.0722402,40.7749511 L41.2741875,32.673375 L41.2741875,32.673375 C41.719125,32.4515625 42,31.9974375 42,31.5 L42,14.241659 C42,13.6893742 41.5522847,13.241659 41,13.241659 C40.8447549,13.241659 40.6916418,13.2778041 40.5527864,13.3472318 L24.1777864,21.5347318 C23.8390024,21.7041238 23.625,22.0503869 23.625,22.429159 Z"" opacity=""0.7""></path>
                            <path class=""color-background"" d=""M20.4472136,21.5347318 L1.4472136,12.0347318 C0.953235098,11.7877425 0.352562058,11.9879669 0.105572809,12.4819454 C0.0361450918,12.6208008 6.47121774e-16,12.7739139 0,12.929159 L0,30.1875 L0,30.1875 C0,30.6849375 0.280875,31.1390625 0.7258125,31.3621875 L19.5528096,40.7750766 C20.0467945,41.0220531 20.");
            WriteLiteral(@"6474623,40.8218132 20.8944388,40.3278283 C20.963859,40.1889789 21,40.0358742 21,39.8806379 L21,22.429159 C21,22.0503869 20.7859976,21.7041238 20.4472136,21.5347318 Z"" opacity=""0.7""></path>
                          </g>
                        </g>
                      </g>
                    </g>
                  </svg>
                </div>
                <p class=""text-xs mt-1 mb-0 font-weight-bold"">Organizations</p>
              </div>
              <h4 class=""font-weight-bolder"">");
#nullable restore
#line (265,47)-(265,71) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TotalOrganizations);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
              <div class=""progress w-75 h-auto"">
                <div class=""progress-bar bg-dark w-30"" role=""progressbar"" aria-valuenow=""30"" aria-valuemin=""0"" aria-valuemax=""100""></div>
              </div>
            </div>
            <div class=""col-6 col-md-3 py-3 ps-0"">
              <div class=""d-flex mb-2"">
                <div class=""icon icon-shape icon-xxs shadow border-radius-sm bg-gradient-danger text-center me-2 d-flex align-items-center justify-content-center"">
                  <i class=""fa fa-users""></i>
                </div>
                <p class=""text-xs mt-1 mb-0 font-weight-bold"">Individuals</p>
              </div>
              <h4 class=""font-weight-bolder"">");
#nullable restore
#line (277,47)-(277,69) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Model.TotalIndividuals);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
              <div class=""progress w-75 h-auto"">
                <div class=""progress-bar bg-dark w-50"" role=""progressbar"" aria-valuenow=""50"" aria-valuemin=""0"" aria-valuemax=""100""></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
");
#nullable restore
#line 287 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"

  }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"  <div class=""col-lg-6"">
    <div class=""card z-index-2"">
      <div class=""card-header pb-0"">
        <h6>Sales overview</h6>
      </div>
      <div class=""card-body p-3"">
        <div class=""chart"">
          <canvas id=""chart-line"" class=""chart-canvas"" height=""300"" style=""display: block; box-sizing: border-box; height: 300px; width: 1050px;"" width=""1050""></canvas>
        </div>
      </div>
    </div>
  </div>
</div>
<div class=""row"">
  <div class=""col-12"">
    <div id=""globe"" class=""position-absolute end-0 top-10 mt-sm-3 mt-7 me-lg-7 peekaboo"">
      <canvas width=""700"" height=""655"" class=""w-lg-100 h-lg-100 w-75 h-75 me-lg-0 me-n10 mt-lg-5"" style=""width: 700px; height: 655.594px;""></canvas>
    </div>
  </div>
</div>
</div>

");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eaa07ae4ce0112ace33e433ca3e1e64f3ac0a90029182", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eaa07ae4ce0112ace33e433ca3e1e64f3ac0a90030222", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eaa07ae4ce0112ace33e433ca3e1e64f3ac0a90031262", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n<script>\r\n    var EbookSales = ");
#nullable restore
#line (315,23)-(315,72) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Html.Raw(Json.Serialize(Model.SalesByMonthEbook)));

#line default
#line hidden
#nullable disable
            WriteLiteral("; \r\n    var EjournalSales = ");
#nullable restore
#line (316,26)-(316,78) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Html.Raw(Json.Serialize(Model.SalesByMonthEjournal)));

#line default
#line hidden
#nullable disable
            WriteLiteral(";\r\n    var VisitorCounter = ");
#nullable restore
#line (317,27)-(317,73) 6 "D:\Pfe\Bookzone\Bookzone\Bookzone\Views\Dashboard\Index.cshtml"
Write(Html.Raw(Json.Serialize(Model.VisitorCounter)));

#line default
#line hidden
#nullable disable
            WriteLiteral(";\r\n</script>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eaa07ae4ce0112ace33e433ca3e1e64f3ac0a90033166", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IWebHostEnvironment _webHostEnvironment { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<global::Bookzone.Models.Entity.Statistics.Statistics> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
