using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// some of those using directives are required for extension methods used below.
// we don't need them in "asp.net core projects" because the web sdk includes
// them as implicit global using directives.

// the only change made to the project file is adding the FrameworkReference.
// framework references are similar to package references, but draw their
// version from the target framework. i did this instead of changing to the
// web sdk because it seems less magical and invasive this way. the framework
// reference is of course part of the web sdk too.

while (true)
{
    Console.WriteLine("1) HTTP");
    Console.WriteLine("2) Stdin");
    Console.WriteLine("3) File");
    Console.WriteLine("4) Quit");
    Console.WriteLine();

    Console.Write("Choice: ");
    string choice = Console.ReadLine()!;

    string? data = null;
    if (choice is "1")
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        WebApplication app = builder.Build();
        app.Run(context =>
        {
            data = context.Request.Path;
            // this could be expanded to also write response data.
            // if it is, then the file-based code should probably also write to
            // a file so the analogy remains close.
            // that would involve making this delegate async,
            // in which case the file-based code should probably also be async.

            IHostApplicationLifetime lifetime = context.RequestServices
                .GetRequiredService<IHostApplicationLifetime>();
            // schedules the WebApplication to stop after this response is done
            lifetime.StopApplication();
            return Task.CompletedTask;
        });
        // this method blocks until the WebApplication shuts down
        app.Run(); // e.g. curl http://localhost:5000/data-from-http
    }
    else if (choice is "2")
    {
        // can draw attention to how the program waits indefinitely for
        // stdin, just like the WebApplication waits indefinitely for
        // a request
        Console.WriteLine("Provide data to stdin: ");
        data = Console.ReadLine();
    }
    else if (choice is "3")
    {
        // this could be expanded to use a streamreader so it
        // "feels" more similar to the http-based code
        Console.WriteLine("Reading data from input.txt...");
        data = File.ReadAllText("input.txt");
    }
    else if (choice is "4" or null)
    {
        break;
    }
    else
    {
        Console.WriteLine();
        continue;
    }

    Console.WriteLine($"Data: {data}");
    Console.WriteLine();
}
