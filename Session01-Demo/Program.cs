using Session01_Demo.Middleware;

namespace Session01_Demo
{
    public class Program
    {
        // Notes
        #region What is MVC ,Flow,Rules,Serach-Order
        /*
         MVC : is an archicture pattern splits the programm into(models - view - controllers)

         MVC Flow:
                    1. User sends Request
                    |
                    v
            2. Routing
               - Receives the Request
               - Determines which Controller and Action should handle it.
                    |
                    v
            3. Controller
               - Contacts the Model.
                    |
                    v
            4. Model
               - Communicates with the Database.
               - Retrieves the required data.
               - Returns data to the Controller.
                    |
                    v
            5. Controller
               - Passes the Model (data) to the View.
                    |
                    v
            6. View
               - Displays the data as HTML to the User.
        -------------------------------------------------------------
        *Model Rules:
        
        1- A plan C# class with propreties
        2- Lives in the Model folder
         

        * View Rules
         1- A cshtml file (HTML + Razor c#)
         2- Lives in Views/<Controller Name>
         3- Only small display logic


        * Controllers Rules
        1- Class names end with 'Controller' and inherits from Controller
        2- Lives in the Controller folder
        3- Every public Method = an action

        -------------------------------------------------------------------
        * Search-Order
        * EX:
        * 1) Views/Products/Details.cshtml
        * 2) views/Shared/Details.cshtml
        * 3) Not Found = error listing all searched paths
         */
        #endregion

        #region Controller Action Method Requirements
        /*
         
      # Controller Action Method Requirements

        In order to be exposed as an Action, a method must meet the following requirements:

        1. The method must be **Public**.
        2. The method cannot be a **Static Method**.
        3. The method cannot be an **Extension Method**.
        4. The method cannot be a **Constructor, Getter, or Setter**.
        5. The method cannot have **Open Generic Types**.
        6. The method must not be inherited from the **Controller Base Class**.
        7. The method cannot contain **ref or out parameters**.

        ## 1. Return Type

        There are no restrictions on the return type of a Controller Action.

        A Controller Action can return:

        - `string`
        - `DateTime`
        - An instance of the `Random` class.
        - `void`
        - `ActionResult`
        - `IActionResult`

        **Important:**

        - ASP.NET MVC converts any return type that is not an `ActionResult` into a string and renders it in the browser.
        - When returning an `ActionResult`, the framework executes the specified result, such as displaying a View.

        ## 2. Generic Types – Important

        The method cannot have **Open Generic Types**.

        ### Open Generic – Not Allowed

        A Generic Method that has an unspecified type parameter:

        ```csharp
        public T GetData<T>()
        {
            // ...
        }
        ```

        Here, `T` is not specified.

        ### Closed Generic – Allowed

        A method that uses a Generic Type with a specified type:

        public ActionResult<string> GetName()
        {
            return "Abdelrahman";
        }
        

        Here, `T = string`, so the generic type is fully specified.

        **Remember:**

        | Example                | Status                            |
        | ---------------------- | --------------------------------- |
        | `GetData<T>()`         | Not Allowed                       |
        | `ActionResult<T>`      | Open Generic Type (unspecified T) |
        | `ActionResult<string>` | Closed Generic Type (Allowed)     |

        **Important Difference:**

        - Generic Method: The method itself has a type parameter `<T>`.
        - Generic Type: A class or type uses a generic parameter.
        - Closed Generic Type: The generic parameter has been specified.

        ## 3. Important Notes

        - Any method that does not violate the requirements above is exposed as a Controller Action.
        - A Controller Action can be invoked by anyone connected to the Internet if it is publicly accessible and not protected by authorization.
        - Never create dangerous actions, such as `DeleteMyWebsite()`, without proper authorization and security checks.

         
         
         */
        #endregion

        #region Middleware & Request Pipeline

        /*
     # Middleware & Request Pipeline

        ## 1. What is Middleware?

        Middleware is a component in the **Request Pipeline** that can:

        1. Execute code when a request arrives.
        2. Inspect or modify the request.
        3. Pass the request to the next middleware.
        4. Inspect or modify the response.
        5. Stop the request and return a response.

        ## 2. Middleware Pipeline

        The request passes through multiple middleware components in order.

        **Example:**

        1. **Logging Middleware:** Writes a log line.
        2. **Authentication Middleware:** Checks who you are.
        3. **Routing Middleware:** Finds the right endpoint.
        4. **Controller Action:** Your MVC code runs here.

        ### Request & Response Flow

        - Request travels from the first middleware to the last.
        - Response travels back in the reverse direction.

        ## 3. The Role of next()

        The `next()` function controls whether the request continues to the next middleware.

        ### Before next()
        - Work on the way in.

        ### After next()
        - Work on the way out.

        ### next()
        - Passes the request to the next middleware.

        **Important Note:**

        If middleware does NOT call `next()`, the request ends there (Short-Circuit).

        ## 4. Example

        app.Use(async (context, next) =>
        {
            // Before next(): Request phase
            Console.WriteLine("Before");

            await next();

            // After next(): Response phase
            Console.WriteLine("After");
        });

        **Output:**

        ```text
        Before
        After
        ```

        Remember:
        - Code before `next()` executes while the request is going in.
        - Code after `next()` executes while the response is coming back.
        - Without `next()`, the pipeline stops at that middleware.
         */
        #endregion

        #region Middleware Methods: Use, Run, Map
        /*
       # Middleware Methods: Use, Run, Map

        ## 1. app.Use()

        - Executes code before and after the next middleware.
        - Calls `next()` to pass the request to the next middleware.
        - After `await next()`, execution returns to the previous middleware.

        **Flow:**
        1. Request enters middleware.
        2. Executes code before `next()`.
        3. Passes request to the next middleware.
        4. After `await next()`, executes code again.

        **Example:**


        app.Use(async (context, next) =>
        {
            Console.WriteLine("Before");

            await next();

            Console.WriteLine("After");
        });


        ## 2. app.Run()

        - Terminal Middleware.
        - Handles the request and stops the pipeline.
        - Does not call `next()`.
        - Middleware after `app.Run()` will never be reached.

        **Example:**


        app.Run(async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });


        ## 3. app.Map()

        - Creates a separate branch in the middleware pipeline based on a URL path prefix.
        - If the request matches the specified path, it enters that branch.
        - If there is no match, the request continues through the main pipeline.
        - The matched branch does not rejoin the main pipeline.

        **Example:**


        app.Map("/admin", adminApp =>
        {
            adminApp.Run(async context =>
            {
                await context.Response.WriteAsync("Admin Page");
            });
        });


        If the request URL starts with `/admin`, it enters the branch.

        Otherwise, it skips the branch and continues through the main pipeline.

        ## 4. Comparison

        | Method | Description |
        |---|---|
        | `app.Use()` | Runs before and after the next middleware. |
        | `app.Run()` | Terminal middleware; stops the pipeline. |
        | `app.Map()` | Creates a separate branch based on URL path prefix. |

        ## 5. Important Exam Notes

        - `Use()` → Can call `next()` and continue the pipeline.
        - `Run()` → Terminal middleware; no `next()`.
        - `Map()` → Creates a branch based on the request path.
        - `Use()` → Can execute code before and after `next()`.
        - `Run()` → Middleware after it is never reached.
        - `Map()` → If the path matches, the request enters the branch. 
         */
        #endregion

        #region Common Middleware Ordering Mistakes

        /*
     # Common Middleware Ordering Mistakes

        Middleware order is very important in ASP.NET Core because each middleware executes in a specific order in the Request Pipeline.

        ## 1. Authorization Before Authentication

        **Mistake:** Placing `UseAuthorization()` before `UseAuthentication()`.

        **What breaks?**
        - The user is always treated as anonymous.
        - Authorization cannot identify the authenticated user correctly.

        **Correct Order:**

        app.UseAuthentication();
        app.UseAuthorization();

        ## 2. CORS After Authorization

        **Mistake:** Placing `UseCors()` after `UseAuthorization()`.

        **What breaks?**
        - Preflight requests may receive `401 Unauthorized`.
        - CORS headers are not added to the response.

        **Correct Order:**

        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();


        ## 3. StaticFiles After Authorization

        **Mistake:** Placing `UseStaticFiles()` after `UseAuthorization()`.

        **What breaks?**
        - Every CSS/JS file may require login.
        - Static files may become inaccessible to unauthenticated users.

        **Correct Order (for public static files):**

        ```csharp
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();
        ```

        ## 4. ExceptionHandler Placed Late

        **Mistake:** Placing `UseExceptionHandler()` late in the pipeline.

        **What breaks?**
        - Earlier middleware errors go unhandled by the exception handler.
        - The application may return unexpected errors.

        **Correct Order:**

        app.UseExceptionHandler("/Home/Error");

        Place it early in the pipeline.

        ## 5. Custom Middleware After Endpoints

        **Mistake:** Placing custom middleware after endpoint execution.

        **What breaks?**
        - Custom middleware may be skipped when a route matches.
        - The endpoint executes before reaching the custom middleware.

        **Correct Order:**

        app.UseMiddleware<CustomMiddleware>();

        app.MapControllers();

        ## 6. Summary – Important for Exam

        | Common Mistake | What Breaks? |
        |---|---|
        | Authorization before Authentication | User is always anonymous |
        | CORS after Authorization | Preflight gets 401, no headers |
        | StaticFiles after Authorization | Every CSS/JS file needs login |
        | ExceptionHandler placed late | Earlier errors go unhandled |
        | Custom Middleware after Endpoints | Skipped when a route matches |

        ### Remember

        - Authentication → Identifies the user.
        - Authorization → Checks user permissions.
        - CORS → Controls cross-origin requests.
        - StaticFiles → Serves CSS, JS, images and other static files.
        - ExceptionHandler → Handles unhandled exceptions.
        - Custom Middleware → Should be placed before endpoint execution if it must run for matched routes.
         */
        #endregion

        #region Note about Model binding
        /*
         Important Notes

        Model Binding attempts to convert incoming values to the required data types.

        If conversion fails, a ModelState error is added.

        For a non-nullable int, the default value is 0.

        ModelState.IsValid can be used to check whether binding and validation succeeded.
         */
        #endregion
        public static void Main(string[] args)
        {
            // Dependancy Injection 
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Service registration
            // register MVC Services[Controllers,Views]
            builder.Services.AddControllersWithViews();
            // register services for API controllers [API]
            //builder.Services.AddControllers();

            //register services for full MVC framework
            //builder.Services.AddMvc();

            // register the core MVC Services
            //builder.Services.AddMvcCore();

            //register services required for razor pages
            //builder.Services.AddRazorPages();
            var app = builder.Build();
            app.UseMiddleware<RequestTimeMiddleware>();

            // Configure the HTTP request pipeline.
            //1] Exception handling middleware
            if (!app.Environment.IsDevelopment())
            {
                
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // Tell the browser to use HTTPS for future requests
                //2] HSTS --> HTTP STRICT TRANSPORT SECURITY
                app.UseHsts();
            }
            // 3] Redirect any incoming HTTP request to HTTPS
            app.UseHttpsRedirection();
            // 4] Serve public assets/ files [wwwroot]
            //app.UseStaticFiles();

            // [5] Match the incoming URL with the available endpoints/actions
            app.UseRouting();

            // Inline middleware [Custom middleware]
            app.Use(async (context, next) =>
            {
                // Before next [Request]
                Console.WriteLine($"Hello");

                // next
                await next(context);

                // After next [Response]
                Console.WriteLine($"Bye");
            });

            // [6] Cross-Origin Resource Sharing
            // app.UseCors();

            // [7] Identifies who the current user is [Login | JWT | Cookies]
            // app.UseAuthentication();

            // [8] Enable the app to read and write session data
            //app.UseSession();

            // [9] Authenticated user is allowed to access resource
            app.UseAuthorization();

            app.MapStaticAssets();

            #region make custom Route
            // app.MapControllerRoute(
            //     name: "shop",
            //     pattern: "{controller}/{action}/{id:int?}",
            //     defaults: new
            //     {
            //         controller = "Products",
            //         action = "TestRouting"
            //     },
            //     constraints: new
            //     {
            //         id = new IntRouteConstraint()
            //     }
            // ); 
            #endregion

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
