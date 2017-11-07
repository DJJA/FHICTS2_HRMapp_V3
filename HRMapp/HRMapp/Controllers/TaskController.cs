using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using Microsoft.AspNetCore.Mvc;
using HRMapp.ViewModels;

namespace HRMapp.Controllers
{
    public class TaskController : Controller
    {
        private static CrossActionMessageHolder errorMessage = new CrossActionMessageHolder();  // Wordt niet opnieuw geïnstantieerd?
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private SkillsetLogic skillsetLogic = new SkillsetLogic();
        private TaskLogic taskLogic = new TaskLogic();

        public IActionResult Index(int id)
        {
            var tasks = taskLogic.GetAll().ToList();
            if (id == 0 && tasks.Count > 0)
            {
                id = tasks[0].Id;
            }
            var model = new TaskCollectionViewModel(id, tasks.ToList()) { InfoMessage = infoMessage.Message };    // Where do I use a List and where an IEnumerable? Where do I convert?
            return View("Task", model);
        }

        public IActionResult TaskView(int id)
        {
            var task = taskLogic.GetById(id);
            return PartialView("../Partial/_TaskView", task);
        }

        public IActionResult New()
        {
            return View("TaskEditor", new TaskEditorViewModel(skillsetLogic.GetAll().ToList()) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult New(TaskEditorViewModel model)
        {
            try
            {
                var addedTaskId = taskLogic.Add(model.ToTask(skillsetLogic.GetAll().ToList()));
                infoMessage.Message = $"'{model.Title}' is toegevoegd aan het systeem.";
                return RedirectToAction("Index", new { id = addedTaskId });
            }
            catch (ArgumentException ex)
            {
                errorMessage.Message = ex.Message;
                return RedirectToAction("New");
            }
        }

        public IActionResult Edit(int id)
        {
            var task = taskLogic.GetById(id);
            return View("TaskEditor", new TaskEditorViewModel(skillsetLogic.GetAll().ToList(), task) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult Edit(TaskEditorViewModel model)
        {
            try
            {
                var success = taskLogic.Update(model.ToTask(skillsetLogic.GetAll().ToList()));
                infoMessage.Message = $"'{model.Title}' is bewerkt.";
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (ArgumentException ex)
            {
                errorMessage.Message = ex.Message;
                return RedirectToAction("Edit", new { id = model.Id });
            }
        }
    }
}