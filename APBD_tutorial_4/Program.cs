using APBD_tutorial_4.Models;

var animals = new List<Animal>();
var visits = new List<Visit>();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/animals", () => Results.Ok(animals))
    .WithName("GetAnimals")
    .WithOpenApi();

app.MapGet("/animals/{id}", (int id) =>
    {
        var animal = animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return Results.NotFound($"Animal with id {id} not found");

        return Results.Ok(animal);
    })
    .WithName("GetAnimal")
    .WithOpenApi();

app.MapPost("/animals", (Animal animal) =>
    {
        animal.Id = animals.Count > 0 ? animals.Max(a => a.Id) + 1 : 1;
        animals.Add(animal);
        return Results.Created($"/animals/{animal.Id}", animal);
    })
    .WithName("AddAnimal")
    .WithOpenApi();

app.MapPut("/animals/{id}", (int id, Animal animal) =>
    {
        var existingAnimal = animals.FirstOrDefault(a => a.Id == id);
        if (existingAnimal == null)
            return Results.NotFound($"Animal with id {id} not found");
        
        existingAnimal.Name = animal.Name;
        existingAnimal.Category = animal.Category;
        existingAnimal.Weight = animal.Weight;
        existingAnimal.FurColor = animal.FurColor;

        return Results.Ok(existingAnimal);
    })
    .WithName("UpdateAnimal")
    .WithOpenApi();

app.MapDelete("/animals/{id}", (int id) =>
    {
        var animal = animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return Results.NotFound($"Animal with id {id} not found");
        
        animals.Remove(animal);
        return Results.NoContent();
    })
    .WithName("DeleteAnimal")
    .WithOpenApi();

app.MapGet("/animals/{id}/visits", (int id) =>
    {
        var animalVisits = visits.Where(v => v.AnimalId == id).ToList();
        return Results.Ok(animalVisits);
    })
    .WithName("GetVisits")
    .WithOpenApi();

app.MapPost("/animals/{id}/visits", (int id, Visit visit) =>
    {
        visit.Id = visits.Count > 0 ? visits.Max(v => v.Id) + 1 : 1;
        visit.AnimalId = id;
        visits.Add(visit);
        return Results.Created($"/animals/{id}/visits/{visit.Id}", visit);
    })
    .WithName("CreateVisit")
    .WithOpenApi();

app.Run();