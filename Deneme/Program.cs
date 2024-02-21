// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using Deneme;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");


HttpClient client = new HttpClient();

var result =client.GetStringAsync("https://localhost:7106/api/Todos").GetAwaiter().GetResult();

List<Todo> todos = JsonConvert.DeserializeObject<List<Todo>>(result);

foreach (var todo in todos) 
{
    Console.WriteLine(todo.Title);
}

Console.ReadLine();