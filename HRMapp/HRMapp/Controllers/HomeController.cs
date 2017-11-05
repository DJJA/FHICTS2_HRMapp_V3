﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using Microsoft.AspNetCore.Mvc;
using HRMapp.ViewModels;

namespace HRMapp.Controllers
{
    public class HomeController : Controller
    {
        //private static CrossActionMessageHolder errorMessage = null;
        private static CrossActionMessageHolder errorMessage = new CrossActionMessageHolder();  // Wordt niet opnieuw geïnstantieerd?
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private SkillsetLogic skillsetLogic = new SkillsetLogic();
        private TaskLogic taskLogic = new TaskLogic();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Skillset
        public IActionResult Skillset(int id)
        {
            var skillsets = skillsetLogic.GetAll().ToList();
            if (id == 0 && skillsets.Count > 0)    // Default id, no parameter passed
            {
                id = skillsets[0].Id;
            }
            var model = new SkillsetCollectionViewModel(id, skillsets) { InfoMessage = infoMessage.Message };    // Where do I use a List and where an IEnumerable? Where do I convert?
            return View(model);
        }

        public IActionResult SkillsetView(int id)
        {
            var skillset = skillsetLogic.GetById(id);
            return PartialView("../Partial/_SkillsetView", skillset);
        }

        public IActionResult NewSkillset()
        {
            return View("../Shared/SkillsetEditor", new SkillsetEditorViewModel() { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult NewSkillset(SkillsetEditorViewModel model)
        {
            try
            {
                var addedSkillsetId = skillsetLogic.Add(model.ToSkillset());
                infoMessage.Message = $"'{model.Title}' is toegevoegd aan het systeem.";
                return RedirectToAction("Skillset", new { id = addedSkillsetId });
            }
            catch (ArgumentException argEx)
            {
                errorMessage.Message = argEx.Message;
                return RedirectToAction("NewSkillset");
            }
        }

        public IActionResult EditSkillset(int id)
        {
            var skillset = skillsetLogic.GetById(id);
            return View("../Shared/SkillsetEditor", new SkillsetEditorViewModel(skillset) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult EditSkillset(SkillsetEditorViewModel model)
        {
            try
            {
                var success = skillsetLogic.Update(model.ToSkillset());
                infoMessage.Message = $"'{model.Title}' is bewerkt.";
                return RedirectToAction("Skillset", new { id = model.Id });
            }
            catch (ArgumentException argEx)
            {
                errorMessage.Message = argEx.Message;
                return RedirectToAction("EditSkillset", new { id = model.Id });
            }

        }
        #endregion
        #region Task
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
            return View("../Shared/TaskEditor", new TaskEditorViewModel(skillsetLogic.GetAll().ToList()) { ErrorMessage = errorMessage.Message });
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
            return View("../Shared/TaskEditor", new TaskEditorViewModel(skillsetLogic.GetAll().ToList(), task) { ErrorMessage = errorMessage.Message });
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
        #endregion
    }
}
