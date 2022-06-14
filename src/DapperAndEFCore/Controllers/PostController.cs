using DapperAndEFCore.Entities;
using DapperAndEFCore.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperAndEFCore.Controllers;

[ApiController]
[Route("[Controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    public ActionResult<Post> Get()
    {
        var results = _postRepository.GetAll();

        return Ok(results);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await _postRepository.GetByIdAsync(id);

        if (entity is null)
            return NotFound($"Entity with Id = {id} not found.");

        return Ok(entity);
    }

    [HttpGet("{id:int}/Detail")]
    public async Task<IActionResult> GetByIdWithDetail(int id)
    {
        var entity = await _postRepository.GetRelationOneToOneAsync(id);

        if (entity is null)
            return NotFound($"Entity with Id = {id} not found.");

        return Ok(entity);
    }

    [HttpGet("{id:int}/Comments")]
    public async Task<IActionResult> GetByIdWithComments(int id)
    {
        var entity = await _postRepository.GetRelationOneToManyAsync(id);

        if (entity is null)
            return NotFound($"Entity with Id = {id} not found.");

        return Ok(entity);
    }

    [HttpGet("{id:int}/CommentsAndDetail")]
    public async Task<IActionResult> GetByIdMultiMappingAsync(int id)
    {
        var entity = await _postRepository.GetMultiMappingAsync(id);

        if (entity is null)
            return NotFound($"Entity with Id = {id} not found.");

        return Ok(entity);
    }

    [HttpGet("{text}/Search")]
    public async Task<IActionResult> SearchPostByText(string text)
    {
        var entities = await _postRepository.SearchPostByText(text);

        return Ok(entities);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> Post([FromForm] Post entity)
    {
        if (entity is null)
            return BadRequest(ModelState);

        _postRepository.Add(entity);

        var result = await _postRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Your changes have no[t been saved.");

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
    }

    [HttpPost("Transaction")]
    public async Task AddPostWithCommentsTransaction()
    {
        await _postRepository.SampleTransaction();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Post entity)
    {
        if (entity is null)
            return BadRequest(ModelState);

        if (id != entity.Id)
            return BadRequest("Identifier is not valid or Identifiers don't match.");

        var existEntity = await _postRepository.GetByIdAsync(id);

        if (existEntity is null)
            return NotFound($"Entity with Id = {id} not found.");

        var result = await _postRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Your changes have not been saved.");

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Post> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest(ModelState);

        var existEntity = await _postRepository.GetByIdAsync(id);
        if (existEntity is null)
            return NotFound($"Entity with Id = {id} not found");

        patchDoc.ApplyTo(existEntity, ModelState);

        var isValid = TryValidateModel(existEntity);
        if (!isValid)
            return BadRequest(ModelState);

        try
        {
            await _postRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = await _postRepository.GetByIdAsync(id);

        if (entity is null)
            return NotFound($"Entity with Id = {id} not found");

        _postRepository.Delete(entity);

        var result = await _postRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest();

        return NoContent();
    }
}
