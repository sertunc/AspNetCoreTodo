using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
    //[Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index()
        {
            // Database'den değerleri getir
            var todoItems = await _todoItemService.GetIncompleteItemsAsync();

            // Gelen değerleri yeni modele koy
            var model = new TodoViewModel()
            {
                Items = todoItems
            };

            // Modeli Görünyüye ekle ve sayfayı göster.
            return View(model);
        }

        public async Task<IActionResult> AddItem(NewTodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var successful = await _todoItemService.AddItemAsync(newItem);
            if (!successful)
            {
                return BadRequest(new { error = "Yeni madde eklenemedi" });
            }

            return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var successful = await _todoItemService.MarkDoneAsync(id);

            if (!successful) return BadRequest();

            return Ok();
        }
    }
}