OST Todos Reference Application
===============================
A domain driven .NET application built on <a href="http://www.ostusa.com/app-dev" target="_blank">OST's</a> [NDriven](https://github.com/OSTUSA/ndriven) framework.
This application is developed using ASP.Net MVC 4, WebApi and Angular JS.  

We developed some <a href="https://ost.mybalsamiq.com/projects/glsecdomaindrivendesignapp/grid" target="_blank">wireframes</a> that describe the functionality of
this simple but very extensible sample. 

Running
-------
Running OST Todos is a snap.

###Step 1###
Clone.

###Step 2###
Build the solution. This will fetch all dependencies. This of course requires Package Restore. Make sure you set "Allow NuGet to download missing packages during build" in Package Manager settings.

###Step 3###
Make sure you have a database on your local machine called OstTodos.  Look in the web.config file within /src/presentation.web to update 
your connectionString.  Here are a couple of examples.

```
SQL Express (Default in web.config)
<connectionStrings>
    <add name="DefaultConnection" connectionString="server=.\SQLExpress;database=OstTodos;Integrated Security=SSPI" 
    providerName="System.Data.SqlClient" />
</connectionStrings>

SQL Server Standard with Integrated Security
<connectionStrings>
    <add name="DefaultConnection" connectionString="server=localhost;database=OstTodos;Integrated Security=SSPI" 
    providerName="System.Data.SqlClient" />
</connectionStrings>
```

Migrations are run via the IPersistenceSetup service when the application starts. Just start the app up, register a user, and
start making todos!

###Step4###
Enjoy.


Highlights
----------
The presentation layer is powered by WebApi, Angular JS, and Bootstrap. It supports client side login and registration. The application module
exposes some angular powers for kicking the user to the login screen on all `401` responses.

Todo
----
This is a demo after all. The login/register methods post to the "out of box" register
and login methods of NDriven - that is they are not set up to return JSON results. Feedback on these two views are currently lacking.
