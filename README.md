OST Todos Reference Application
===============================
A domain driven .NET application built on [OST's](http://www.ostusa.com/app-dev) [NDriven](https://github.com/OSTUSA/ndriven) framework.
This application is developed using ASP.Net MVC 4, WebApi and Angular JS.  

Here are wireframes that describe the functionality of
this simple but very extensible sample: https://ost.mybalsamiq.com/projects/glsecdomaindrivendesignapp/grid

Running
-------
Running OST Todos is a snap.

###Step 1###
Clone.

###Step2###
Build the solution. This will fetch all dependencies. This of course requires Package Restore. Make sure you set "Allow NuGet to download missing packages during build" in Package Manager settings.

###Step3###
Make sure you have a database on your local machine called OstTodos

###Step4###
Run migrations

Start in the root of the ost-todos folder
```
bin\Migrate.exe -c "server=.\SQLExpress;database=OstTodos;Integrated Security=SSPI" -db sqlserver2008
-a "src\Infrastructure.Migrations\bin\Debug\Infrastructure.Migrations.dll" -t migrate:up --profile=Development
```

The `--profile=Development` ensures you have a user with the following credentials to play with:
email: test1@test.com
password: password

###Step5###
Enjoy.


Highlights
----------
The presentation layer is powered by WebApi, Angular JS, and Bootstrap. It supports client side login and registration. The application module
exposes some angular powers for kicking the user to the login screen on all `401` responses.

Todo
----
This is a demo after all. The login/register methods post to the "out of box" register
and login methods of NDriven - that is they are not set up to return JSON results. Feedback on these two views are currently lacking.
