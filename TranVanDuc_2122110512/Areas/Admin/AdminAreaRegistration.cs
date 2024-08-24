using System.Web.Mvc;

public class AdminAreaRegistration : AreaRegistration
{
    public override string AreaName
    {
        get { return "Admin"; }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
        context.MapRoute(
            "Admin_default",
            "Admin/{controller}/{action}/{id}",
            new { action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "TranVanDuc_2122110512.Areas.Admin.Controllers" } // Namespace của các controller trong Admin
        );
    }
}
