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
            return RedirectToAction("Task");
        }

        public IActionResult Task(int id)
        {
            var tasks = taskLogic.GetAll().ToList();
            if (id == 0 && tasks.Count > 0)
            {
                id = tasks[0].Id;
            }
            var model = new TaskCollectionViewModel(id, tasks.ToList()) { InfoMessage = infoMessage.Message };    // Where do I use a List and where an IEnumerable? Where do I convert?
            return View(model);
        }

        public IActionResult TaskView(int id)
        {
            var task = taskLogic.GetById(id);
            return PartialView("../Partial/_TaskView", task);
        }

        public IActionResult NewTask()
        {
            return View("TaskEditor", new TaskEditorViewModel(skillsetLogic.GetAll().ToList()) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult NewTask(TaskEditorViewModel model)
        {
            try
            {
                var addedTaskId = taskLogic.Add(model.ToTask(skillsetLogic.GetAll().ToList()));
                infoMessage.Message = $"'{model.Title}' is toegevoegd aan het systeem.";
                return RedirectToAction("Task", new { id = addedTaskId });
            }
            catch (ArgumentException ex)
            {
                errorMessage.Message = ex.Message;
                return RedirectToAction("NewTask");
            }
        }

        public IActionResult EditTask(int id)
        {
            var task = taskLogic.GetById(id);
            return View("TaskEditor", new TaskEditorViewModel(skillsetLogic.GetAll().ToList(), task) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult EditTask(TaskEditorViewModel model)
        {
            try
            {
                var success = taskLogic.Update(model.ToTask(skillsetLogic.GetAll().ToList()));
                infoMessage.Message = $"'{model.Title}' is bewerkt.";
                return RedirectToAction("Task", new { id = model.Id });
            }
            catch (ArgumentException ex)
            {
                errorMessage.Message = ex.Message;
                return RedirectToAction("EditTask", new { id = model.Id });
            }
        }
    }
}