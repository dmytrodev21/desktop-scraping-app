using Microsoft.AspNetCore.Mvc.Rendering;

namespace k190144_Q4.Models;

public class CategoryViewModel
{
    public List<SelectListItem> CategorySelectList { get; set; }

    public string SelectedCategory { get; set; }

    public List<ScriptModel> scripts { get; set; }

}
