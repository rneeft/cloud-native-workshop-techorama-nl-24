
using Dometrain.Monolith.Api.Sdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateApplicationBuilder();

host.Services.AddDometrainApi("http://localhost:5148", "ThisIsAlsoMeantToBeSecret");

var app = host.Build();

var coursesClient = app.Services.GetRequiredService<ICoursesApiClient>();

var studentsClient = app.Services.GetRequiredService<IStudentsApiClient>();

var course = await coursesClient.GetAsync("47f1011a-20cb-411c-9ca0-4aed27c3566a");
var student = await studentsClient.GetAsync("3b0a8175-f8f1-4ea0-821b-568993604da7");


Console.WriteLine();
