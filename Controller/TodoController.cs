

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MeuTodo.Controller
{
        [ApiController]
        [Route(template:"v1")]
    public class TodoController : ControllerBase
    {


           [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
            context.Todos.Remove(todo);
            await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [HttpPut("todos/{id}")]
          public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model,
            [FromRoute] int id )
            {
                 if (!ModelState.IsValid)
                return BadRequest();

                 var todo = await context.Todos.FirstOrDefaultAsync(x=> x.Id == id);

                 if (todo == null)
                    return NotFound();

                try
                {
                    
                    todo.Title = model.Title;

                    context.Todos.Update(todo);
                    await context.SaveChangesAsync();

                    return Ok(todo);

                }
                catch (System.Exception ex)
                {
                    return BadRequest();
                }   
            }

        
        [HttpGet]
        [Route(template:"todos")]
        public  async Task<IActionResult> GetAsync([FromServices]AppDbContext context) 
        {
           var todos = await context
                    .Todos
                    .AsNoTracking()
                    .ToListAsync(); 
                    
          return Ok(todos);
        }



        [HttpGet]
        [Route(template:"todos/{id}")]
        public  async Task<IActionResult> GetbyIdAsync([FromServices]AppDbContext context, [FromRoute] int id) 
        {
           var todo = await context
                    .Todos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id); 
                    
         return todo == null
                ? NotFound()
                : Ok(todo);
        }
       
        [HttpPost("todos")]
        public async Task<IActionResult> PostAsync([FromServices]AppDbContext context, [FromBody] CreateTodoViewModel model)
        {
            if(!ModelState.IsValid)
                return  BadRequest();

            var todo = new Todo{
                Data = DateTime.Now,
                Done = false, 
                Title =  model.Title
            };
            try
            {
                    //salva na memoria
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();

                return Created(uri:$"v1/todos/{todo.Id}", todo);

            }
            catch (System.Exception ex)
            {
                 // TODO
                 return BadRequest();
            }

            
            
            }
       
        }
    }
