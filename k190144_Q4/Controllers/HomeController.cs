using k190144_Q4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml;

namespace k190144_Q4.Controllers;

public class HomeController : Controller
{
    private List<ScriptModel> scripts;
    private string path;

    public HomeController()
    {
        scripts = new List<ScriptModel>();
        path = System.Configuration.@ConfigurationManager.AppSettings["path"];

    }

    public IActionResult Index(string SelectedCategory = "")
    {

        var model = new CategoryViewModel();
        model.CategorySelectList = new List<SelectListItem>();

        DirectoryInfo di = new(path);
        DirectoryInfo[] diArr = di.GetDirectories();
        foreach (DirectoryInfo dri in diArr)
            model.CategorySelectList.Add(new SelectListItem() { Text = dri.Name, Value = dri.Name});

        if (SelectedCategory == "")
        {
            foreach (DirectoryInfo dri in diArr)
            {
                ParseXml(dri.ToString());
            }
        }
        else
        {
            ParseXml(path + "\\" + SelectedCategory);
        }
        model.scripts = scripts;

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(CategoryViewModel model)
    {
        var SelectedCategory = model.SelectedCategory;
        if (SelectedCategory == "NoCategory")
            SelectedCategory = "";
        return RedirectToAction("Index", new { SelectedCategory });
    }


    public void ParseXml(string dri)
    {
        var directory = new DirectoryInfo(dri);
        var fileName = directory.GetFiles()
            .OrderByDescending(f => f.LastWriteTime)
            .First();

        XmlTextReader readFile = new XmlTextReader(fileName.ToString());
        string script = "", price = "";


        while (readFile.Read())
        {
            if (readFile.NodeType == XmlNodeType.Element && readFile.Name == "Script")
            {
                script = readFile.ReadElementString();
            }
            if (readFile.NodeType == XmlNodeType.Element && readFile.Name == "Price")
            {
                price = readFile.ReadElementString();
                scripts.Add(new ScriptModel() { Script = script, CurrentPrice = price });
            }
        }
    }

}
