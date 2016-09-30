# RequestServiceExtender

ASP.NET Core comes with a beautiful dependency injection system. However, it's not possible to provide additional objects depending on the request, especially when your factory function is async.

This function replaces the RequestServices in HttpContext with a proxy class in order to provide this functionality.

For example, if you want to connect to db1 when the Referer header is customer1.myapp.com, and to db2 when the header is customer2.myapp.com:

```csharp
using Tamturk.AspNetCore.RequestServiceExtender;

app
    .Use(async (context, next) => {
        string strConnectionString = null;

        var header = new Uri(context.Request.Headers["Referer"]);
        if(header.Host == "customer1.myapp.com") {
            strConnectionString = "connection string of db1";
        } else if(header.Host == "customer2.myapp.com") {
            strConnectionString = "connection string of db2";
        } else {
            throw new Exception("No such client");

            // or your custom routine, it can be also async (e.g. getting the info from cache or getting it from master db. etc.)
        }

        // you need to dispose it manually either with using or try/finally. The library will not dispose it for you.
        using(var db = new ApplicationDbConnect(strConnectionString)) {
            context.AddScoped(db);
            await next();
        }

		/*
			// only if you don't want to construct ApplicationDbConnect when it's not needed.
			ApplicationDbConnect db = null;
			try {
				context.AddScoped(() => db ?? (db = new ApplicationDbConnect(strConnectionString)));

				await next();
			} finally {
				if(db != null) {
					db.Dispose();
				}
			}
		*/
    })
    .UseMvc()
```

ApplicationDbConnect will be resolved on your controller:

```csharp
public class AccountController : Controller {
    ApplicationDbContext db;
    
    public AccountController(ApplicationDbContext db) {
		this.db = db;
    }
    
    // ...
}
```

You can do anything you want, e.g. changing your storage depending on "testing=true" query string etc. I wrote it to provide varying Azure storage account (CloudStorageAccount) and YouTubeService depending on the Referer on my projects.

# Download

Install [Tamturk.AspNetCore.RequestServiceExtender](https://www.nuget.org/packages/Tamturk.AspNetCore.RequestServiceExtender/) package from Nuget.

# The MIT License (MIT)

Copyright (c) 2016 Burak Tamtürk

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
