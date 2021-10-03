#pragma checksum "C:\Users\ga_bo\source\repos\MyBlog\MyBlog\Views\Admin\UserList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fef4d49f40909515084ce4a310fe70d8ece85458"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_UserList), @"mvc.1.0.view", @"/Views/Admin/UserList.cshtml")]
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
#line 1 "C:\Users\ga_bo\source\repos\MyBlog\MyBlog\Views\_ViewImports.cshtml"
using MyBlog;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ga_bo\source\repos\MyBlog\MyBlog\Views\_ViewImports.cshtml"
using MyBlog.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ga_bo\source\repos\MyBlog\MyBlog\Views\_ViewImports.cshtml"
using MyBlog.Extentions;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fef4d49f40909515084ce4a310fe70d8ece85458", @"/Views/Admin/UserList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3343ff78042d9bf4160e78c93b503885db88699c", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_UserList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""row"">
    <div class=""col-6"">
        <h2 class=""text-info"">Users</h2>
    </div>
    <div class=""col-6 text-right"">

    </div>
</div>
<br />
<div class=""border bg-white"">
    <br />
    <div>
        <table id=""users"" class=""table table-striped border"">
            <thead>
            <th>Firstname</th>
            <th>Lastname</th>
            <th>Email</th>
            <th>Username</th>
            <th></th>
            </thead>
        </table>
    </div>
</div>
");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>

        var dataTable;

        $(document).ready(
            function () {
                loadDataTable();
            });


        function loadDataTable() {

            dataTable = $('#users').DataTable({
                ""ajax"": {
                    ""url"": ""/admin/getUserList/"",
                    ""type"": ""GET"",
                    ""datatype"": ""json""
                },
                ""columns"": [
                    {
                        ""data"": ""firstName"",
                        ""width"": ""20%""
                    },
                    {
                        ""data"": ""lastName"",
                        ""width"": ""25%""
                    },
                    {
                        ""data"": ""email"",
                        ""width"": ""25%""
                    },
                    {
                        ""data"": ""userName"",
                        ""width"": ""25%""
                    },
                    {
                        ""data""");
                WriteLiteral(@": ""id"",
                        ""render"": function (data) {
                            return `<a href=""/admin/EditUserRole?userId=${data}"" title=""Edit Role""><i class=""fas fa-user-lock""></i></a>`
                        },
                        ""width"": ""5%""
                    }
                ],
                ""language"": {
                    ""emptyTable"": ""no data found""
                },
                ""width"": ""100%""
            });

        }

    </script>
");
            }
            );
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
