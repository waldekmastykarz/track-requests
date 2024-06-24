var handler = new CodeLocationDelegatingHandler(new HttpClientHandler());
using var httpClient = new HttpClient(handler);

async Task Method1()
{
    var response = await httpClient.GetStringAsync("https://jsonplaceholder.typicode.com/posts");
    Console.WriteLine(response);
}

async Task Method2()
{
    var response = await httpClient.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1");
    Console.WriteLine(response);
}

await Method1();
await Method2();